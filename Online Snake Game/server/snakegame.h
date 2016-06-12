/*
 * Josh Jacoby
 * Shawn Ratcliff
 * Jonathan Saavedra
 */
 
#ifndef snakegame_h
#define snakegame_h

#include "json.hpp"
#include <deque>
#include <list>
#include <string>
#include "player.h"
#include "position.h"

using namespace std;
using json = nlohmann::json;

/* Snake Game class. Encapsulates game logic and state. */
class SnakeGame {

    public:
    
    /** Public constructor. Initializes new game.
     *  @param json - Initial status update from player 1
     *  @param json - Initial status update from player 2
     */
    SnakeGame(json, json);
    
    /** Receives JSON update from client and updates client's data.
     *  @param clientData - the json update message received from a client
     */
    void handleClientInput(json clientData);
    
    /** Increments the frame counter and advances the players' position by one unit based on
     *  their last-known direction.
     *  @return a JSON object containing the complete game state after the update (to be broadcast to both players)
     */
    json update();
    
    /** Returns whether game is active or not
     *  @return bool active or inactive
     */
    bool isActive() const;
    
    /** Destructor */
    ~SnakeGame();
    
    private:
    
    Player* player1;
    Player* player2;
    int currentFrame; // The current logical frame of the game
    bool gameActive; // Whether the game is still active or has terminated
    Position applePosition; // The current location of the apple
	bool player1Collision;
	bool player2Collision;
	bool player1Wrong;
	bool player2Wrong;
	std::list<unsigned long long> previous_apples;

    
    /** Sets a new apple position.
     */
    void setApple();
    
    /** Builds JSON object reporting current game status.
	 *  @param bool - force clients to resync/override local prediction
     *  @return json the game status
     */
    json statusObject() ;

	bool check_previous(unsigned long long time_stamp);
    
};


#endif /* snakegame_h */
