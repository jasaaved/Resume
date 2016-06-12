/*
 * Josh Jacoby
 * Shawn Ratcliff
 * Jonathan Saavedra
 */

#include "position.h"

Position::Position() {
    x = 0;
    y = 0;
}

Position::Position(int a, int b) {
    x = a;
    y = b;
}

json Position::getJSON() const {
    return json({ {"x", x}, {"y", y} });
}


bool operator== (const Position& a, const Position& b) {
    return (a.x == b.x && a.y == b.y);
}

bool operator!= (const Position& a, const Position& b) {
    return !(a == b);
}
