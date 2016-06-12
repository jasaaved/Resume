Task: Create an online version of a game (Frogger, Space Invaders, Bomberman, or Tetris). User needs to be able to create an account to login.
There needs to be latency mitigation. Server is able to run two game instances at the same time with two players each.

Accomplishments: Created an online version of Bomberman. A database was used using mySQL and was run using C9.io. We created a middleman server
that the user will connect to initially. Middleman server tells client what game servers are available to join (server a or server b). Client
can join any server (if it is not full or closed because a game was started). Once more than one player has joined a server, players can chat and 
then start a game of bomberman. Two games can run simultaneously with 4 players each.

My tasks: I was charge of the communication between the server and clients. I coded what is being sent and received and how the client or server
should respond once it gets a message. I was also in charge of latency mitigation. I used extrapolation (player movement is predicted based
on their current trajectory). This helps mask the latency that might be happening and makes it somewhat invisible to the player. If the prediction
was incorrect, the game corrects the position once it gets the latest message from the server. However, extrapolation is only used to predict
the movement of opponent bombermans, each client has full control of their own Bomberman. I also implemented the chat system and the strike system. Every 10
seconds the server tests the latency of each player. If the server detects a client has latency higher than 500ms, that client will receive a strike.
Three strikes and that client is kicked out of the game.

To find files with code, find the assets folder within bomberman, servera, serverb, middlemanserver folders. There is a script folder there that has all the code.