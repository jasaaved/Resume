using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
    //Initialize a random number generator
    static System.Random rng = new System.Random();

    //Generally speaking, these two variables should be the same
    public int num_rooms;
    public int Map_Width_and_Height;
    //----------------------------------------------------------

    public float Room_Width;
    public float Room_Height;
    private Room[,] map;

    public GameObject VertPlug;
    public GameObject HorizPlug;
    public GameObject Player;
    public GameObject Player2;
    public GameObject Player3;


    public GameObject[] startRooms;
    public GameObject[] rooms;
    public GameObject[] bossBatRooms;
    public GameObject[] dreadknightRooms;

    private Room bossBatR;
    private Room dreadknightR;



    class Room
    {
        public int X;
        public int Y;

        //public Room up = null;
        public bool up_door = false;
        //public Room down = null;
        public bool down_door = false;
        //public Room left = null;
        public bool left_door = false;
        //public Room right = null;
        public bool right_door = false;

        public Room(int posX, int posY){
            X = posX;
            Y = posY;
        }
    }

    struct coord
    {
        public int X;
        public int Y;

        public coord(int posX, int posY)
        {
            X = posX;
            Y = posY;
        }
    }

    //This is the function that generates the map
    void PGenerate()
    {
        //2D array in C# is of the format: array[row,column]
        int mapsize = (Map_Width_and_Height * 2) + 1;
        int mapcenter = Map_Width_and_Height + 1;
        map = new Room[mapsize, mapsize];
        //Initialize the map to contain all nulls at first 
        //(so we can determine if a room space is empty by checking for null)
        for (int i = 0; i < mapsize; i++)
        {
            for (int j = 0; j < mapsize; j++)
            {
                map[i, j] = null;
            }
        }

        //##################################################################
        //Place the starting room in the center of the map
        Room start_room = new Room(mapcenter, mapcenter);
        map[mapcenter, mapcenter] = start_room;
        num_rooms--;

        //This is the list that will be randomly pulled from to determine which room will be generated next
        List<coord> next_room_list = new List<coord>();

        //##################################################################
        //Add the four possible next rooms to this one.
        //Up
        next_room_list.Add(new coord(mapcenter, mapcenter + 1));
        //Down
        next_room_list.Add(new coord(mapcenter, mapcenter - 1));
        //Right
        next_room_list.Add(new coord(mapcenter + 1, mapcenter));
        //Left
        next_room_list.Add(new coord(mapcenter - 1, mapcenter));

        //Containers for the next part
        coord next_room_coords;
        Room next_room;

        //While there are rooms left to be generated
        while (num_rooms > 0)
        {
            //##############################################################
            //Select the next room to create randomly
            int r = rng.Next(next_room_list.Count);
            next_room_coords = next_room_list[r];
            //Now remove that option from the list
            next_room_list.RemoveAt(r);

            //Create said room
            next_room = new Room(next_room_coords.X, next_room_coords.Y);

            //##############################################################
            //Connect the new room to a room next to it
            //Up
            if (next_room.Y < mapsize && map[next_room.X, next_room.Y + 1] != null) { //Check that the next tile up is a room and within the map
                next_room.up_door = true;
                map[next_room.X, next_room.Y + 1].down_door = true;
            }
            //Down
            else if (next_room.Y > 0 && map[next_room.X, next_room.Y - 1] != null) { //Check that the next tile down is a room and within the map
                next_room.down_door = true;
                map[next_room.X, next_room.Y - 1].up_door = true;
            }
            //Right
            else if (next_room.X < mapsize && map[next_room.X + 1, next_room.Y] != null) { //Check that the next tile to the right is a room and within the map
                next_room.right_door = true;
                map[next_room.X + 1, next_room.Y].left_door = true;
            }
            //Left
            else if (next_room.X > 0 && map[next_room.X - 1, next_room.Y] != null) { //Check that the next tile to the left is a room and within the map
                next_room.left_door = true;
                map[next_room.X - 1, next_room.Y].right_door = true;
            }


            //##############################################################
            //Place the room on the map
            map[next_room.X, next_room.Y] = next_room;


            //##############################################################
            //Now, we add the new possible rooms to the list
            //Up
            if (next_room.Y < mapsize && map[next_room.X, next_room.Y + 1] == null && !next_room_list.Contains(new coord(next_room.X, next_room.Y + 1)))
            { //Check that the next tile up is both empty and within the map and is not already an option
                next_room_list.Add(new coord(next_room.X, next_room.Y + 1));
            }
            //Down
            if (next_room.Y > 0 && map[next_room.X, next_room.Y - 1] == null && !next_room_list.Contains(new coord(next_room.X, next_room.Y - 1)))
            { //Check that the next tile down is both empty and within the map and is not already an option
                next_room_list.Add(new coord(next_room.X, next_room.Y - 1));
            }
            //Right
            if (next_room.X < mapsize && map[next_room.X + 1, next_room.Y] == null && !next_room_list.Contains(new coord(next_room.X + 1, next_room.Y)))
            { //Check that the next tile to the right is both empty and within the map and is not already an option
                next_room_list.Add(new coord(next_room.X + 1, next_room.Y));
            }
            //Left
            if (next_room.X > 0 && map[next_room.X - 1, next_room.Y] == null && !next_room_list.Contains(new coord(next_room.X - 1, next_room.Y)))
            { //Check that the next tile to the left is both empty and within the map and is not already an option
                next_room_list.Add(new coord(next_room.X - 1, next_room.Y));
            }

            //Now that we've generated a room, decrement the number of rooms we need to generate
            num_rooms--;
        }

        //Now, randomly select one of the possible "next room" locations and choose this as the location for the boss bat room
        //We do this last so the boss bat room has a higher likelihood of being placed far from the starting room, but isn't guaranteed.
        int bbr = rng.Next(next_room_list.Count);
        coord bossCoords = next_room_list[bbr];
        //Now remove that option from the list
        next_room_list.RemoveAt(bbr);

        //Create said room
        bossBatR = new Room(bossCoords.X, bossCoords.Y);
        //Now we connect the boss room to whatever room it's next to
        //Up
        if (bossBatR.Y < mapsize && map[bossBatR.X, bossBatR.Y + 1] != null)
        { //Check that the next tile up is a room and within the map
            bossBatR.up_door = true;
            map[bossBatR.X, bossBatR.Y + 1].down_door = true;
        }
        //Down
        else if (bossBatR.Y > 0 && map[bossBatR.X, bossBatR.Y - 1] != null)
        { //Check that the next tile down is a room and within the map
            bossBatR.down_door = true;
            map[bossBatR.X, bossBatR.Y - 1].up_door = true;
        }
        //Right
        else if (bossBatR.X < mapsize && map[bossBatR.X + 1, bossBatR.Y] != null)
        { //Check that the next tile to the right is a room and within the map
            bossBatR.right_door = true;
            map[bossBatR.X + 1, bossBatR.Y].left_door = true;
        }
        //Left
        else if (bossBatR.X > 0 && map[bossBatR.X - 1, bossBatR.Y] != null)
        { //Check that the next tile to the left is a room and within the map
            bossBatR.left_door = true;
            map[bossBatR.X - 1, bossBatR.Y].right_door = true;
        }
        //Now we repeat the process for the dreadknight room
        int dr = rng.Next(next_room_list.Count);
        bossCoords = next_room_list[dr];
        next_room_list.RemoveAt(dr);

        dreadknightR = new Room(bossCoords.X, bossCoords.Y);
        //Up
        if (dreadknightR.Y < mapsize && map[dreadknightR.X, dreadknightR.Y + 1] != null)
        { //Check that the next tile up is a room and within the map
            dreadknightR.up_door = true;
            map[dreadknightR.X, dreadknightR.Y + 1].down_door = true;
        }
        //Down
        else if (dreadknightR.Y > 0 && map[dreadknightR.X, dreadknightR.Y - 1] != null)
        { //Check that the next tile down is a room and within the map
            dreadknightR.down_door = true;
            map[dreadknightR.X, dreadknightR.Y - 1].up_door = true;
        }
        //Right
        else if (dreadknightR.X < mapsize && map[dreadknightR.X + 1, dreadknightR.Y] != null)
        { //Check that the next tile to the right is a room and within the map
            dreadknightR.right_door = true;
            map[dreadknightR.X + 1, dreadknightR.Y].left_door = true;
        }
        //Left
        else if (dreadknightR.X > 0 && map[dreadknightR.X - 1, dreadknightR.Y] != null)
        { //Check that the next tile to the left is a room and within the map
            dreadknightR.left_door = true;
            map[dreadknightR.X - 1, dreadknightR.Y].right_door = true;
        }
    }

    void Construct()
    {
        int mapsize = (Map_Width_and_Height * 2) + 1;
        int mapcenter = Map_Width_and_Height + 1;

        GameObject next_room;
        GameObject generated_room;
        int r = 0;

        //We immediately generate the starting room, since we know exactly where it will go
        r = rng.Next(startRooms.Length);
        next_room = startRooms[r];
        generated_room = Instantiate(next_room, new Vector3(0, 0, 0), Quaternion.identity);

        //Now that we've generated the starting room, we generate its doors using the room object we made earlier
        if (!map[mapcenter, mapcenter].up_door)
        {
            Instantiate(HorizPlug, new Vector3(0, (Room_Height / 2), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/TopDoor").gameObject);
        }
        if (!map[mapcenter, mapcenter].down_door)
        {
            Instantiate(HorizPlug, new Vector3(0, -(Room_Height / 2), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/BottomDoor").gameObject);
        }
        if (!map[mapcenter, mapcenter].right_door)
        {
            Instantiate(VertPlug, new Vector3((Room_Width / 2), 0, 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/RightDoor").gameObject);
        }
        if (!map[mapcenter, mapcenter].left_door)
        {
            Instantiate(VertPlug, new Vector3(-(Room_Width / 2), 0, 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/LeftDoor").gameObject);
        }

        //Now that we've made the starting room, we do a bit of a trick so that our roomgenerator skips over it
        map[mapcenter, mapcenter] = null;
        //This way we save a couple hundred comparisons, but we as programmers always know the center room needs filled
        //The generator sees null and ignores it, but we manually filled it anyway
        

        for (int i = 0; i < mapsize; i++)
        {
            for (int j = 0; j < mapsize; j++)
            {
                if (map[i, j] != null)
                {
                    //Instantiate room
                    r = rng.Next(rooms.Length);
                    next_room = rooms[r];
                    generated_room = Instantiate(next_room, new Vector3(((map[i, j].X - mapcenter) * Room_Width), ((map[i, j].Y - mapcenter) * Room_Height), 0), Quaternion.identity);

                    //Instantiate doors
                    if (!map[i, j].up_door)
                    {
                        Instantiate(HorizPlug, new Vector3(((map[i, j].X - mapcenter) * Room_Width), ((map[i, j].Y - mapcenter) * Room_Height) + (Room_Height / 2), 0), Quaternion.identity);
                        Destroy(generated_room.transform.Find("MinimapBlips/Doors/TopDoor").gameObject);
                    }
                    if (!map[i, j].down_door)
                    {
                        Instantiate(HorizPlug, new Vector3(((map[i, j].X - mapcenter) * Room_Width), ((map[i, j].Y - mapcenter) * Room_Height) - (Room_Height / 2), 0), Quaternion.identity);
                        Destroy(generated_room.transform.Find("MinimapBlips/Doors/BottomDoor").gameObject);
                    }
                    if (!map[i, j].right_door)
                    {
                        Instantiate(VertPlug, new Vector3(((map[i, j].X - mapcenter) * Room_Width) + (Room_Width / 2), ((map[i, j].Y - mapcenter) * Room_Height), 0), Quaternion.identity);
                        Destroy(generated_room.transform.Find("MinimapBlips/Doors/RightDoor").gameObject);
                    }
                    if (!map[i, j].left_door)
                    {
                        Instantiate(VertPlug, new Vector3(((map[i, j].X - mapcenter) * Room_Width) - (Room_Width / 2), ((map[i, j].Y - mapcenter) * Room_Height), 0), Quaternion.identity);
                        Destroy(generated_room.transform.Find("MinimapBlips/Doors/LeftDoor").gameObject);
                    }
                }
            }
        }

        r = rng.Next(bossBatRooms.Length);
        next_room = bossBatRooms[r];
        generated_room = Instantiate(next_room, new Vector3(((bossBatR.X - mapcenter) * Room_Width), ((bossBatR.Y - mapcenter) * Room_Height), 0), Quaternion.identity);

        if (!bossBatR.up_door)
        {
            Instantiate(HorizPlug, new Vector3(((bossBatR.X - mapcenter) * Room_Width), ((bossBatR.Y - mapcenter) * Room_Height) + (Room_Height / 2), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/TopDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Up").gameObject);
        }
        if (!bossBatR.down_door)
        {
            Instantiate(HorizPlug, new Vector3(((bossBatR.X - mapcenter) * Room_Width), ((bossBatR.Y - mapcenter) * Room_Height) - (Room_Height / 2), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/BottomDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Down").gameObject);
        }
        if (!bossBatR.right_door)
        {
            Instantiate(VertPlug, new Vector3(((bossBatR.X - mapcenter) * Room_Width) + (Room_Width / 2), ((bossBatR.Y - mapcenter) * Room_Height), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/RightDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Right").gameObject);
        }
        if (!bossBatR.left_door)
        {
            Instantiate(VertPlug, new Vector3(((bossBatR.X - mapcenter) * Room_Width) - (Room_Width / 2), ((bossBatR.Y - mapcenter) * Room_Height), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/LeftDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Left").gameObject);
        }


        r = rng.Next(dreadknightRooms.Length);
        next_room = dreadknightRooms[r];
        generated_room = Instantiate(next_room, new Vector3(((dreadknightR.X - mapcenter) * Room_Width), ((dreadknightR.Y - mapcenter) * Room_Height), 0), Quaternion.identity);

        if (!dreadknightR.up_door)
        {
            Instantiate(HorizPlug, new Vector3(((dreadknightR.X - mapcenter) * Room_Width), ((dreadknightR.Y - mapcenter) * Room_Height) + (Room_Height / 2), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/TopDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Up").gameObject);
        }
        if (!dreadknightR.down_door)
        {
            Instantiate(HorizPlug, new Vector3(((dreadknightR.X - mapcenter) * Room_Width), ((dreadknightR.Y - mapcenter) * Room_Height) - (Room_Height / 2), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/BottomDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Down").gameObject);
        }
        if (!dreadknightR.right_door)
        {
            Instantiate(VertPlug, new Vector3(((dreadknightR.X - mapcenter) * Room_Width) + (Room_Width / 2), ((dreadknightR.Y - mapcenter) * Room_Height), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/RightDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Right").gameObject);
        }
        if (!dreadknightR.left_door)
        {
            Instantiate(VertPlug, new Vector3(((dreadknightR.X - mapcenter) * Room_Width) - (Room_Width / 2), ((dreadknightR.Y - mapcenter) * Room_Height), 0), Quaternion.identity);
            Destroy(generated_room.transform.Find("MinimapBlips/Doors/LeftDoor").gameObject);
            Destroy(generated_room.transform.Find("AdjacencyChecker/Left").gameObject);
        }
    }

	void Start () {
        //In the future this will draw from an actual list of rooms, from the folder
        //
        PGenerate();
        Construct();
        Instantiate(Player, new Vector3(-3, 0, 0), Quaternion.identity);
        Instantiate(Player2, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(Player3, new Vector3(3, 0, 0), Quaternion.identity);
    }

    
    //Debug.Log(start_room.up);
}
