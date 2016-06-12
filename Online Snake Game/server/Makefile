CC = g++
CFLAGS = -c -std=c++11 -ggdb
LDFLAGS =
SOURCES = main.cpp websocket.cpp sha1.cpp base64.cpp snakegame.cpp player.cpp position.cpp MessageDelayer.cpp
OBJECTS = $(SOURCES:.cpp=.o)
EXECUTABLE = snake-server

all: $(SOURCES) $(EXECUTABLE)
	
$(EXECUTABLE): $(OBJECTS) 
	$(CC) $(LDFLAGS) $(OBJECTS) -o $@

.cpp.o:
	$(CC) $(CFLAGS) $< -o $@

clean:
	rm -rf *o $(EXECUTABLE)
