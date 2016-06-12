TASK: Create an online 2-player version of snake that has latency mitigation.

Accomplishments: Created a 2 player version of snake. Snake game was written in javascript and the server was written in C++.
Extrapolation was used to predict the location of the opponent snake.

My tasks: Took existing code of a single player version of snake and made it multiplayer. Also took code from a chat room made 
in c++ and made changes to allow it to work with our game which created the base for our server. I was also in charge of latency
mitigation. The server has a copy of the game state running, but we felt the server should only correct a game state if a player did something
wrong (latency causes inconsistency between game states). Therefore, clients have full control of their snakes.  If a player's snake dies,
they report it to the server. A client is never wrong in reporting their own death. The only times a client could possibly be wrong: 
client takes a fruit that has already been taken (latency may cause the delay for the fruit to disappear) and if the two snake collides.
The server is in charge of handling these issues by using timestamps. If a client was wrong, the server sends a message and client changes the 
game state accordingly.  