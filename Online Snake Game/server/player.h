/*
 * Josh Jacoby
 * Shawn Ratcliff
 * Jonathan Saavedra
 */

#ifndef player_h
#define player_h

#include "json.hpp"
#include <string>
#include <deque>
#include "position.h"

using json = nlohmann::json;

/* Representation of a player and his current state */
class Player {
    
public:
    
    std::string playerName;
    int direction;
    int score;
	std::deque<Position> queue;
    
    /** Constructor 
     *  @param string - player name
     *  @param int - direction
     *  @param Position - the player's starting position
     */
    Player(std::string, int, Position);

    /** Returns the position of this player's head */
    Position head() const;
    
    /** Advances the player's snake 1 unit based on current direction,
     *  grows the snake if it eats food, increments the score, and reports the event
     *  @param const Position& - the current food Position
     *  @return bool - whether the snake scored food
     */
    bool advance(const Position&);
    
    /** Returns the player's queue as a JSON object. */
    json getQueueJSON() const;
    
    /** Checks if this player's body occupies Position (x,y)
     *  @param Position - the position to check
     *  @return bool
     */
    bool occupies(const Position&) const;

    /** Checks if this player's head is touching a game boundary.
     *  @return bool 
     */
    bool boundaryCheck() const;
    
    /** Detects whether a collision is occurring between two snakes.
     *  @param const Player& - the other player
     */
    static bool collisionCheck(const Player&, const Player&);

	void update_queue(json new_queue);
    
protected:
    
    // We use a FIFO queue to model the snake. Counterintuitively,
    // The front of the queue is the TAIL of the snake; while the
    // back of the queue is the HEAD (i.e., the node that will be
    // last to pop off the queue as the snake moves.)
    
    
};


#endif /* player_h */
