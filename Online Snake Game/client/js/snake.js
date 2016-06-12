/*
Joshua Jacoby
Steven Ratcliff
Jonathan Saavedra
*/

var
/**
 * Constats
 */
COLS = 26,
ROWS = 26,
EMPTY = 0,
SNAKE1 = 1,
FRUIT = 2,
SNAKE2 = 3,
LEFT  = 0,
UP    = 1,
RIGHT = 2,
DOWN  = 3,
KEY_LEFT  = 37,
KEY_UP    = 38,
KEY_RIGHT = 39,
KEY_DOWN = 40,
KEY_W = 87,
KEY_S = 83,
KEY_A = 65,
KEY_D = 68,
/**
 * Game objects
 */
canvas,	  /* HTMLCanvas */
ctx,	  /* CanvasRenderingContext2d */
keystate, /* Object, used for keyboard inputs */
temp,

myName,
player1, 
player2, /* string, player names */

score1, /* int: player scores */
score2,

playerNumber, /* int : Player number (1 or 2) assigned from server */
localSnake,  /* reference to the local player's snake object */
remoteSnake,  /* reference to the remote player's snake object */

newServerUpdate, /* message object: a server game update ready to be processed */ 
applePosition, /* (x, y) coordinate pair representing the apple location */

running, /* boolean, flags if game is running or not */
animationFrame, /* the current Window.animationframe (in case we need to kill it) */
updateCycleLength, /* in ms, dictated by server */
network_latency, /* the most recent latency estimate */

frame = 0, /* The current frame number for server synchronization */
lastUpdateTime, /* the time, in ms, when we last advanced the frame */

got_queue,/* a bool to check if the cleint has gotten a queue from the server */
collision, /* a bool. If client hits a wall, this will be true */
got_apple, /* a bool. If client has gotten an apple, this will be true */

player1Correction,
player2Correction,

network; /* type: GameNetwork */

/**
 * Grid datastructor, usefull in games where the game world is
 * confined in absolute sized chunks of data or information.
 *
 * @type {Object}
 */
grid = {
	width: null,  /* number, the number of columns */
	height: null, /* number, the number of rows */
	_grid: null,  /* Array<any>, data representation */
	/**
	 * Initiate and fill a c x r grid with the value of d
	 * @param  {any}    d default value to fill with
	 * @param  {number} c number of columns
	 * @param  {number} r number of rows
	 */
	init: function(d, c, r) {
		this.width = c;
		this.height = r;
		this._grid = [];
		for (var x=0; x < c; x++) {
			this._grid.push([]);
			for (var y=0; y < r; y++) {
				this._grid[x].push(d);
			}
		}
	},
	/**
	 * Set the value of the grid cell at (x, y)
	 *
	 * @param {any}    val what to set
	 * @param {number} x   the x-coordinate
	 * @param {number} y   the y-coordinate
	 */
	set: function(val, x, y) {
		if (x >= 0 && x < this._grid.length && y >= 0 && y < this._grid[x].length) {
		    this._grid[x][y] = val;
		}
	},
	/**
	 * Get the value of the cell at (x, y)
	 *
	 * @param  {number} x the x-coordinate
	 * @param  {number} y the y-coordinate
	 * @return {any}   the value at the cell
	 */
	get: function(x, y) {
		return this._grid[x][y];
	}
}

/**
 * The snake, works as a queue (FIFO, first in first out) of data
 * with all the current positions in the grid with the snake id
 *
 *  Snake 1 is the snake corresponding to Player 1 
 *  (which may or may not be the local player -- depends on the assignment from server) 
 */
snake1 = {
	direction: null, /* number, the direction */
	last: null,		 /* Object, pointer to the last element in
						the queue */
	_queue: null,	 /* Array<number>, data representation*/
	history: {},   /* List of the direction for each frame. E.g., history[2] = direction @ frame 2 */
	/**
	 * Clears the queue and sets the start position and direction
	 *
	 * @param  {number} d start direction
	 * @param  {number} x start x-coordinate
	 * @param  {number} y start y-coordinate
	 */
	init: function(d, x, y) {
		this.direction = d;
		this._queue = [];
		this.insert(x, y);
	},
	/**
	 * Adds an element to the queue
	 *
	 * @param  {number} x x-coordinate
	 * @param  {number} y y-coordinate
	 */
	insert: function(x, y) {
		// unshift prepends an element to an array
		this._queue.unshift({x:x, y:y});
		this.last = this._queue[0];
	},
	/**
	 * Removes and returns the first element in the queue.
	 *
	 * @return {Object} the first element
	 */
	remove: function() {
		// pop returns the last element of an array
		return this._queue.pop();
	}
}

/*  Snake 2 is the snake corresponding to Player 2 
 *  (which may very well be this client - depends on server assignment) 
 */
    snake2 = {
    direction: null, /* number, the direction */
    last: null,		 /* Object, pointer to the last element in
						the queue */
    _queue: null,	 /* Array<number>, data representation*/
    history: {},   /* List of the direction for each frame. E.g., history[2] = direction @ frame 2 */
    /**
	 * Clears the queue and sets the start position and direction
	 *
	 * @param  {number} d start direction
	 * @param  {number} x start x-coordinate
	 * @param  {number} y start y-coordinate
	 */
    init: function (d, x, y) {
        this.direction = d;
        this._queue = [];
        this.insert(x, y);
    },
    /**
	 * Adds an element to the queue
	 *
	 * @param  {number} x x-coordinate
	 * @param  {number} y y-coordinate
	 */
    insert: function (x, y) {
        // unshift prepends an element to an array
        this._queue.unshift({ x: x, y: y });
        this.last = this._queue[0];
    },
    /**
	 * Removes and returns the first element in the queue.
	 *
	 * @return {Object} the first element
	 */
    remove: function () {
        // pop returns the last element of an array
        return this._queue.pop();
    }
};


/**
 * Starts the game
 */
function main() {
    // create and initiate the canvas element
    got_queue = false;
    collision = false;
    got_apple = false;
	canvas = document.createElement("canvas");
	canvas.width = COLS*20;
	canvas.height = ROWS*20;
	ctx = canvas.getContext("2d");
	// add the canvas element to the body of the document
	document.body.appendChild(canvas);
	// sets an base font for bigger score display
	ctx.font = "12px Helvetica";
	ui.showConnectionStatus("active");
    ui.setGameBorder(playerNumber); // Make border match user color
	frame = 0;
	keystate = {};
	// keeps track of the keybourd input
	document.addEventListener("keydown", function(evt) {
		keystate[evt.keyCode] = true;
	});
	document.addEventListener("keyup", function(evt) {
		delete keystate[evt.keyCode];
	});
	// intatiate game objects and starts the game loop
	init();
	running = true;
	loop();
}

/**
 * Resets and inits game objects
 */
function init() {
    score1 = 0;
    score2 = 0;
    temp = ' ';
	grid.init(EMPTY, COLS, ROWS);
}

/**
 * The game loop function, used for game updates and rendering
 */
function loop() {
    // Only run this loop if game is running
    if (!running) {
        return;
    }
	
	checkKeyState();
	update();
	writeGrid();
	draw();
	
	// When ready to redraw the canvas call the loop function
	// first. Runs about 60 frames a second
	animationFrame = window.requestAnimationFrame(loop, canvas);
}

/**
 * Checks the key state and updates the local player's direction.
 */
function checkKeyState() {
	
    // CAUTION: This section is a little tricky, becasue we need to know
    // which snake we are "driving." Are we snake1, or snake2?
    // That depends on the assignment we received from the server.

    // First, let's determine our current direction, since it affects
    // where the snake is allowed to turn
    var direction;
    
    if (playerNumber == 1) {
        direction = snake1.direction;     
    }
    else {
        direction = snake2.direction;
    }

    var newDirection = direction;

    // Read the keystate and choose a direction    
	if (keystate[KEY_LEFT] && direction !== RIGHT) {
		newDirection = LEFT;
	}
	if (keystate[KEY_UP] && direction !== DOWN) {
		newDirection = UP;
	}
	if (keystate[KEY_RIGHT] && direction !== LEFT) {
		newDirection = RIGHT;
	}
	if (keystate[KEY_DOWN] && direction !== UP) {
		newDirection = DOWN;
	}
	
	// If the direction has changed, we need to update our 
	// status and then tell the server.
	if (direction != newDirection) {
    	    // Update status
    	    if (playerNumber == 1) {
        	    snake1.direction = newDirection;
    	    }
    	    else {
        	    snake2.direction = newDirection;
    	    }
    	    
    	    // Build a player status object and send it to the server
    	    
  	}	
}

/** Updates the game state, if an update is available from the server.
 *
 *  Status object contains at least the following key-value pairs 
 *  (more can be added later, as client functionality grows):
 *
 *  "MESSAGE_TYPE" = "SERVER_UPDATE",
 *  "CURRENT_FRAME" = the current frame number
 *  "GAME_STATUS" = true/false whether the game is still active
 *  "RESYNC" = true/false whether we must override local sim with server data
 *  "PLAYER_1_NAME" = player 1's name
 *  "PLAYER_2_NAME" = player 2's name
 *  "PLAYER_1_SCORE" = player 1's score
 *  "PLAYER_2_SCORE" = player 2's score
 *  "PLAYER_1_QUEUE" = a JSON object containing P1's queue
 *  "PLAYER_2_QUEUE" = a JSON object containing P2's queue
 *  "PLAYER_1_DIRECTION" = player 1's direction
 *  "PLAYER_2_DIRECTION" = player 2's direction
 */
function update() {
    // If it's too soon for the next frame, do nothing
    if (new Date().getTime() - lastUpdateTime < updateCycleLength) {
        return;
    }
    
    frame++;
    console.log("Frame: " + frame);
    
    // Record the current player's direction in the history
    localSnake.history[frame] = localSnake.direction;
   
    // Advance both snakes forward as a prediction
    if (got_queue) {
        if (!collision) {
            advanceSnake(localSnake);
        }
        advanceSnake(remoteSnake);
        boundary_check(localSnake);
    }

    network.sendUpdate(playerStatus());
    got_apple = false;
        
    // Use the most recent server update, if one is available.
    if (newServerUpdate != null && newServerUpdate != undefined) {
        
        if (newServerUpdate["GAME_STATUS"] == false) {
            ui.endGame(player1, player2, score1, score2);
            newServerUpdate = null;
            localSnake = null;
            remoteSnake = null;
            running = false;
            return;
        }
        
        player1 = newServerUpdate["PLAYER_1_NAME"];
        player2 = newServerUpdate["PLAYER_2_NAME"];
        score1 = newServerUpdate["PLAYER_1_SCORE"];
        score2 = newServerUpdate["PLAYER_2_SCORE"];
        applePosition = newServerUpdate["APPLE_POSITION"];
        player1Correction = newServerUpdate["PLAYER_1_CORRECTION"];
        player2Correction = newServerUpdate["PLAYER_2_CORRECTION"];

        if (player1Correction == true && playerNumber == 1)
            localSnake.remove();

        if (player2Correction == true && playerNumber == 2)
            localSnake.remove();
        //score_check = newServerUpdate["SCORE_CORRECTION];
        //if (!score_check){
        //  localSnake.remove();
        //}
        
        var frame_lag;

        // Update the remote player's data, and compensate for lag
        if (playerNumber == 1) {
            remoteSnake._queue = newServerUpdate["PLAYER_2_QUEUE"];
            remoteSnake.direction = newServerUpdate["PLAYER_2_DIRECTION"];
            frame_lag = Math.max(0, frame - newServerUpdate["PLAYER_2_FRAME"]);
        } else {
            remoteSnake._queue = newServerUpdate["PLAYER_1_QUEUE"];
            remoteSnake.direction = newServerUpdate["PLAYER_1_DIRECTION"];
            frame_lag = Math.max(0, frame - newServerUpdate["PLAYER_1_FRAME"]);
        }
        compensateLag(remoteSnake, frame_lag);
                
        // If a RESYNC signal is received, or if this is the first frame, 
        // also replace the local player's queue, and compensate for lag
        if (!got_queue) {
            if (playerNumber == 1) {
                localSnake._queue = newServerUpdate["PLAYER_1_QUEUE"];
            } else {
                localSnake._queue = newServerUpdate["PLAYER_2_QUEUE"];
            }
            got_queue = true;
        }
   
        // Delete the server update; we don't need it anymore
        newServerUpdate = null;        
    }

    // Update the game clock counter
    lastUpdateTime = new Date().getTime();
}


/* Writes data to grid object in preparation for draw() call. */
function writeGrid() {
    
    // Clear the grid
    	grid.init(EMPTY, COLS, ROWS);

    	// Write snake 1 onto the grid
    	for (var i = 0; snake1._queue != null && i < snake1._queue.length; i++) {
        	grid.set(SNAKE1, snake1._queue[i]["x"], snake1._queue[i]["y"]);
    	}

    	// Write snake 2 onto the grid
    	for (var i = 0; snake2._queue != null && i < snake2._queue.length; i++) {
        	grid.set(SNAKE2, snake2._queue[i]["x"], snake2._queue[i]["y"]);
    	}
                    
    // Write the apple onto the grid
    grid.set(FRUIT, applePosition["x"], applePosition["y"]);    
}

/** Advances the snake object one unit for client-side
 *  prediction purposes.
 *  @param snakeObject - the snake object to advance
 */
function advanceSnake(snake) {
    // Grab the current head location
	var nx = snake._queue[0].x;
	var ny = snake._queue[0].y;

	// Determine new head position (nx, ny)
	switch (snake.direction) {
		case LEFT:
			nx--;
			break;
		case UP:
			ny--;
			break;
		case RIGHT:
			nx++;
			break;
		case DOWN:
			ny++;
			break;
	}
         
	snake.insert(nx, ny);
	check_apple(localSnake);
	if (!got_apple)
	    snake.remove();
}

function boundary_check(snake) {
    var nx = snake._queue[0].x;
    var ny = snake._queue[0].y;

    if (0 >= nx || nx >= grid.width - 1 || 0 >= ny || ny >= grid.height - 1 ||
        grid.get(nx, ny) === SNAKE1 ) {
        collision = true;
    }


}

function check_apple(snake) {
    var nx = snake._queue[0].x;
    var ny = snake._queue[0].y;
    if (grid.get(nx, ny) === FRUIT) {
        got_apple = true;
    }
}

/** Compensates for lag in server update by fast-forwarding. 
 *  For the opponent, we simply advance the snake by the 
 *  number of lag frames, as a prediction.
 *  
 *  For the player, we don't need any compensation because the
 *  client is always deemed authoritative over its own 
 *  snake object (i.e., it's treated as a locked object).
 */
function compensateLag(snake, lag) {
    
    if (snake == remoteSnake) {

        for (var i = 0; i <= lag; i++) {
            advanceSnake(snake);
        }

    }
}


/**
 * Render the grid to the canvas.
 */
function draw() {
	// calculate tile-width and -height
	var tw = canvas.width/grid.width;
	var th = canvas.height/grid.height;
	// iterate through the grid and draw all cells
	for (var x=0; x < grid.width; x++) {
		for (var y=0; y < grid.height; y++) {
			// sets the fillstyle depending on the id of
			// each cell
			switch (grid.get(x, y)) {
				case EMPTY:
					ctx.fillStyle = "#fff";
					break;
				case SNAKE1:
					ctx.fillStyle = "#00f";
					break;
				case FRUIT:
					ctx.fillStyle = "#f83";
					break;
			    case SNAKE2:
			        ctx.fillStyle = "#f00";
			        break;
			}
			ctx.fillRect(x*tw, y*th, tw, th);
		}
	}
	// changes the fillstyle once more and draws the score
	// message to the canvas
	ctx.fillStyle = "#000";
	ctx.fillText(temp, 10, canvas.height - 10);
	ctx.fillText(player1 + " score: " + score1, 10, canvas.height - 10);
	ctx.fillText(player2 + " score: " + score2, 180, canvas.height - 10);
    ctx.fillText(" Network latency: " + network_latency + "ms", 300, canvas.height - 10);

}

/** Initializes new player 
    @param playernum - the player number assigned from the server (1 or 2)
*/
function initializePlayer(playerNum) {
    // Set the player number
    playerNumber = playerNum;

    // Set the initial direction on the appropriate snake
    // The local player could be either snake1 or snake2, depending.
    if (playerNumber == 1) {        
        snake1.direction = UP;
    }
    else if (playerNumber == 2) {
        snake2.direction = DOWN;
    }
}


/** Returns an object containing a Client Update message bundle.
  * This has at least the following fields (more can be added later):
  * "MESSAGE_TYPE" = "CLIENT_UPDATE"
  * "PLAYER_NUMBER" = 1 or 2
  * "PLAYER_NAME" = (player name)
  * "CLIENT_DIRECTION" = int (direction)
  * "CLIENT_QUEUE" = the local snake's queue
*/
function playerStatus() {
    var msg = {};
    msg["MESSAGE_TYPE"] = "CLIENT_UPDATE";
    msg["CURRENT_FRAME"] = frame;
    msg["PLAYER_NUMBER"] = playerNumber;
    msg["PLAYER_NAME"] = myName;
    msg["TIME_STAMP"] = new Date().getTime();
    msg["FRAME"] = frame;
    msg["COLLISION"] = collision;
    msg["GOT_APPLE"] = got_apple;
    msg["GOT_QUEUE"] = got_queue;

    if (playerNumber == 1) {
        msg["CLIENT_DIRECTION"] = snake1.direction;
        msg["CLIENT_QUEUE"] = snake1._queue;
    }    
    
    else if (playerNumber == 2) {
        msg["CLIENT_DIRECTION"] = snake2.direction;
        msg["CLIENT_QUEUE"] = snake2._queue;
    }
    return msg;
}