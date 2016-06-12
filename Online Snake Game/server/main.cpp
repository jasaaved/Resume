//Joshua Jacoby
//Steven Ratcliff
//Jonathan Saavedra
#include <stdlib.h>
#include <iostream>
#include <string>
#include <sstream>
#include <time.h>
#include <vector>
#include <chrono>
#include "websocket.h"
#include "snakegame.h"
#include "json.hpp"
#include "MessageDelayer.h"

using namespace std;
using json = nlohmann::json;

const int UPDATE_CYCLE_LENGTH_MS = 300; // Game only updates once per cycle length

webSocket server;
SnakeGame *game_p = NULL; // A pointer to access the SnakeGame we'll eventually instantiate
json pregame_player_msgs; // Holding place for messages recevied from clients before game starts
unsigned long long lastUpdateTime = 0; // Keep track of when we last advanced the game state
MessageDelayer send_buffer(300); // Outgoing message delay buffer
MessageDelayer receive_buffer(300); // Incoming message delay buffer


/**** FORWARD FUNCTION DECLARATIONS ****/

/* Event handler for new network connection open event. 
 * @param int - client ID
 */
void openHandler(int);

/** Event handler for incoming message from network.
 *  @param int - client ID
 *  @param string - message payload
 */
void messageHandler(int, string);

/* Event handler for connection closed event.
 * @param int - client ID
 */
void closeHandler(int);

/* Callback function for main server loop. The underlying
 * socket library will call this function regularly, perhaps
 * hundreds of times per second. Any recurrent behavior that
 * is desired must be invoked here.
 */
void periodicHandler();

/** Resets, purges message buffers, deletes any pointers and resets to NULL */
void resetGame();

/** Sends json object as JSON string-formatted message to specified client
 *  @param int clientID
 *  @param json JSON object message
 */
void send_message(int, json);

/** Decodes incoming message and takes required action.
 *  @param int - client ID
 *  @param string - the message payload
 */
void read_message(int, string);


/**** FUNCTION DEFINITIONS ****/

/** Main **/
int main(int argc, char *argv[]){
    
    int port;
    
    cout << "Please set server port: ";
    cin >> port;
    
    /* set event handler */
    server.setOpenHandler(openHandler);
    server.setCloseHandler(closeHandler);
    server.setMessageHandler(messageHandler);
    server.setPeriodicHandler(periodicHandler);
    
    /* start the snake server, listen to ip '127.0.0.1' and port '8000' */
    server.startServer(port);
    
    return 1;
}

void openHandler(int clientID){
    
    json msg; // Our first message to the client
    
    // If Player 1 just connected...
    if (server.getClientIDs().size() == 1) {
        // Send msg: you've been assigned player 1
        msg["MESSAGE_TYPE"] = "PLAYER_ASSIGNMENT";
        msg["PLAYER_NUMBER"] = 1;
		msg["UPDATE_CYCLE_LENGTH"] = UPDATE_CYCLE_LENGTH_MS;
        send_message(clientID, msg);
    }
    
    // If Player 2 just connected...
    else if (server.getClientIDs().size() == 2) {
        // Send msg: you've been assigned player 2
        msg["MESSAGE_TYPE"] = "PLAYER_ASSIGNMENT";
        msg["PLAYER_NUMBER"] = 2;
		msg["UPDATE_CYCLE_LENGTH"] = UPDATE_CYCLE_LENGTH_MS;
        send_message(clientID, msg);
    }
    
    // Or if there are too many connections, reject it:
    else {
        msg["MESSAGE_TYPE"] = "CONNECTION_REJECTED";
        send_message(clientID, msg);
        server.wsClose(clientID);
    }
}

void messageHandler(int clientID, string message) {
    receive_buffer.putMessage(clientID, message);
}

void closeHandler(int clientID){
    
    // If game is ongoing, kill it and send out
    // an error to whomever is still connected:
    if (game_p != NULL && game_p->isActive()) {
        json errorMsg;
        errorMsg["MESSAGE_TYPE"] = "ERROR";
        errorMsg["ERROR_MSG"] = "Other player disconnected";
        
        // Send the message to whomever is connected
        vector<int> clientIDs = server.getClientIDs();
        for (int i = 0; i < clientIDs.size(); i++) {
            server.wsSend(clientIDs[i], errorMsg.dump()); // Don't buffer
        }
        
        // Close all open connections (must be done separately)
        clientIDs = server.getClientIDs();
        for (int i = 0; i < clientIDs.size(); i++) {
            server.wsClose(i);
        }
        resetGame();
    }
}

void periodicHandler(){
    
    // Poll the incoming & outgoing message buffers.
    std::pair <int, std::string> message_pair;
    if (send_buffer.isMessageReady()) {
        message_pair = send_buffer.getMessage();
        if (message_pair.first != -1) {
			json time_check = json::parse(message_pair.second);
			if (time_check["MESSAGE_TYPE"] == "TIME_STAMP_REPLY") {
				time_check["T3"] = chrono::system_clock::now().time_since_epoch() / chrono::milliseconds(1);
				message_pair.second = time_check.dump();
			}
            server.wsSend(message_pair.first, message_pair.second);
        }
    }
    if (receive_buffer.isMessageReady()) {
        message_pair = receive_buffer.getMessage();
        if (message_pair.first != -1) {
            read_message(message_pair.first, message_pair.second);
        }
    }
    
    // If the game is active, update the game state.
    // We want this to occur no sooner than UPDATE_CYCLE_LENGTH_MS.
    // WARNING: This sets the pace of the game, so Milestone 4 client features that
    // perform movement prediction MUST use the same clock speed.
    if (game_p != NULL && game_p->isActive()) {
        unsigned long long currentTime = chrono::system_clock::now().time_since_epoch() / chrono::milliseconds(1);
        if (currentTime - lastUpdateTime >= UPDATE_CYCLE_LENGTH_MS) {
            // Update the game
            json msg = game_p->update();
            
            // Broadcast the update to all clients
            vector<int> clientIDs = server.getClientIDs();
            for (int i = 0; i < clientIDs.size(); i++) {
                send_message(clientIDs[i], msg);
            }
            
            // Update the clock
            lastUpdateTime = chrono::system_clock::now().time_since_epoch() / chrono::milliseconds(1);
        }
    }
    
    // If there is a game object but the status is INACTIVE,
    // update the clients and then DESTROY the game object.
    if (game_p != NULL && !game_p->isActive()) {

        json msg = game_p->update();
        resetGame();
        
        // Broadcast the final update to all clients
        // and then disconnect clients
        cout << "GAME OVER BROADCASTING FINAL UPDATE" << endl;
        vector<int> clientIDs = server.getClientIDs();
        for (int i = 0; i < clientIDs.size(); i++) {
            server.wsSend(clientIDs[i], msg.dump()); // Don't buffer
        }
        for (int i = 0; i < clientIDs.size(); i++) {
            server.wsClose(i);
        }
    }
}

void resetGame() {
    delete game_p;
    game_p = NULL;
    pregame_player_msgs.clear();
    send_buffer.clear();
    receive_buffer.clear();
    lastUpdateTime = 0;
}

void send_message(int clientID, json msg) {
    send_buffer.putMessage(clientID, msg.dump());
}

void read_message(int clientID, string message){
    
    // Deserialize message from the string
    json msg = json::parse(message);
    
    if (msg["MESSAGE_TYPE"] == "CLIENT_UPDATE") {
        
        // Send a time stamp reply for client-side latency estimation
        json tStamps;
		tStamps["MESSAGE_TYPE"] = "TIME_STAMP_REPLY";
        tStamps["T2"] = chrono::system_clock::now().time_since_epoch() / chrono::milliseconds(1); // Time received
        tStamps["T1"] = msg["TIME_STAMP"]; // Origination time
        send_message(clientID, tStamps);
        
        // Now, process the client update:
        // Scenario A: We don't have a game ready yet. This is a pre-game message.
        if (game_p == NULL) {
            
            // Step 1: Put the message in the correct bin.
            pregame_player_msgs[clientID] = msg;
            
            // Step 2: If both bins are filled (2 players ready),
            // then start a new game.
            if (pregame_player_msgs.size() == 2) {
                game_p = new SnakeGame(pregame_player_msgs[0], pregame_player_msgs[1]);
                pregame_player_msgs.clear();
            }
        }
        
        // Scenario B: A game already exists. Just forward the message to it.
        else {
            game_p->handleClientInput(msg);
        }
    }
}