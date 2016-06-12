/*
 * Josh Jacoby
 * Shawn Ratcliff
 * Jonathan Saavedra
 */

#include "player.h"
#include <typeinfo>

using json = nlohmann::json;

Player::Player(std::string nom, int dir, Position startPos) {
    playerName = nom;
    direction = dir;
    queue.push_back(startPos);
    score = 0;
}

Position Player::head() const {
    return queue.back(); // The snake's head is at the back of the queue
}


bool Player::advance(const Position& food) {
    Position newHead;
    newHead = queue.back();
    switch(direction) {
        case UP:
            newHead.y--;
            break;
        case DOWN:
            newHead.y++;
            break;
        case LEFT:
            newHead.x--;
            break;
        case RIGHT:
            newHead.x++;
            break;
    }
    queue.push_back(newHead); // Add the new head
    if (food != head()) {
        queue.pop_front(); // Delete the old tail if we didn't get food
        return false;
    }
    else {
        score++;
        return true;
    }
}

json Player::getQueueJSON() const {
    // Iterate over the Positions in the queue, and add their respective JSON objects
    // to a JSON container object
    json output;
    for (auto iter = queue.begin(); iter != queue.end(); iter++ ) {
        output.push_back(iter->getJSON());
    }
    return output;
}

bool Player::occupies(const Position& pos) const {
    for (auto iter = queue.begin(); iter != queue.end(); iter++) {
        if (pos == *iter)
            return true;
    }
    return false;
}

bool Player::boundaryCheck() const {
    // Check if snake's head touches the perimeter
	return (head().x < 0 || head().x > COLS-1 || head().y < 0 || head().y > ROWS-1);
}


bool Player::collisionCheck(const Player &p1, const Player &p2) {
    // Check p1's head against p2's whole queue
    for (auto iter = p2.queue.begin(); iter != p2.queue.end(); iter++) {
        if (p1.head() == *iter)
            return true;
        
        // Check the other snake's head against our queue
        for (auto iter = p1.queue.begin(); iter != p1.queue.end(); iter++) {
            if (p2.head() == *iter)
                return true;
        }
    }
    // Otherwise, return false
    return false;
}

void Player::update_queue(json new_queue) {
	queue.clear();
	for (auto iter = new_queue.begin(); iter != new_queue.end(); iter++){
		auto position = iter.value();
		int x = position.at("x");
		int y = position.at("y");
		Position head(x,y);
		queue.push_front(head);
	}
}