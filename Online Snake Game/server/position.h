/*
 * Josh Jacoby
 * Shawn Ratcliff
 * Jonathan Saavedra
 */

#ifndef position_h
#define position_h

#include "json.hpp"

using json = nlohmann::json;

/* Declare constants: these must be identical on the client side.
 * By convention, we will say that Player 1 always starts at the bottom facing up, and
 * Player 2 starts at the top, facing down. (As it was originally implemented in the JS snake game.)
 */
const int COLS = 26;
const int ROWS = 26;
const int CENTER_COLUMN = 13;
const int LEFT  = 0;
const int UP    = 1;
const int RIGHT = 2;
const int DOWN  = 3;

/* An (x, y) integer pair to represent game coordinates */
class Position {
    
public:
    
    int x;
    int y;
    
    // Constructors

    /** Default constructor */
    Position();
    
    /** Constructor
     *  @param int - x value
     *  @param int - y value
     */
    Position(int, int);
    
    /** Returns a json object in the form {"x" : value, "y" : value}
     *  @return json object
     */
    json getJSON() const;
};

/** Overloaded == operator */
bool operator== (const Position&, const Position&);

/** Overloaded != operator */
bool operator!= (const Position&, const Position&);

#endif /* position_h */
