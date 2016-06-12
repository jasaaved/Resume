// Joshua Jacoby
// Shawn Ratcliff
// Jonathan Saavedra

#include "MessageDelayer.h"

MessageDelayer::MessageDelayer(int Delay) {
    // Instantiate a binomial distribution with a maximum delay of Delay, and
    // a probability of 0.5 (i.e., average latency at the midpoint of [0, Delay]).
    latencyDistribution = std::binomial_distribution<int>(Delay, 0.5);
}

// Inserts a message into the MessageDelayer
void MessageDelayer::putMessage(int clientID, std::string message) {
	queued_msg_t msg(clientID, message, calcReleaseTime());
	messageQueue.push(msg);
}

// Fetches the next message from the MessageDelayer, if one is ready
// (If not, a special error value is sent. )
std::pair <int, std::string> MessageDelayer::getMessage() {
    if (isMessageReady()) {
		queued_msg_t msg = messageQueue.front();
        messageQueue.pop();
		return std::pair<int, std::string>(msg.clientID, msg.message);
    } else
        return std::pair <int, std::string>(-1, "");
}

bool MessageDelayer::isMessageReady() {
	return (!messageQueue.empty() && timeNow() >= messageQueue.front().releaseTime);
}

// Discard all messages in the buffer
void MessageDelayer::clear() {
    while (!messageQueue.empty()) {
        messageQueue.pop();
    }
}

// Calculate and set the next time a message will be ready to be released
unsigned long long int  MessageDelayer::calcReleaseTime(){
    int delay = latencyDistribution(randomEngine);
	return timeNow() + delay;
}

unsigned long long int MessageDelayer::timeNow() {
	return std::chrono::system_clock::now().time_since_epoch() /
		std::chrono::milliseconds(1);
}