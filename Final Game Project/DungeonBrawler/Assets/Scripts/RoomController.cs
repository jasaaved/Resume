using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour {

    private static int ROOM_POSITION_X = 0;
    private static int ROOM_POSITION_Y = 0;
    private Vector3 cameraTransform;
    private GameObject mainCamera;
    private Vector3 playerPosition;
    private GameObject player;

    public int xBound = 26;
    public int yBound = 15;
    public int cameraTranslateX = 54;
    public int cameraTranslateY = 30;
    public float playerTranslateX = 1.5f;
    public float playerTranslateY = 4f;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("RobotBoy(Clone)");
        mainCamera = GameObject.Find("Main Camera");

        // This will be implemented once level generation is a thing.
        // It's not yet complete, so it doesn't yet handle "plugging"
        // based on level generation. It's more a place holder for now.
        //PlugHandler();
    }

    // Update is called once per frame
    void Update () {
        if (player != null)
        {
            playerPosition = player.transform.position;
        }
 
        CameraHandler();
    }

    void CameraHandler()
    {
        // Player moving through right side entry, switch camera to right-adjacent room
        if (playerPosition.x >= ROOM_POSITION_X + xBound)
        {
            cameraTransform = new Vector3(
                mainCamera.transform.position.x + cameraTranslateX,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = cameraTransform;

            player.transform.position = new Vector3(
                player.transform.position.x + playerTranslateX,
                player.transform.position.y,
                player.transform.position.z);

            ROOM_POSITION_X += cameraTranslateX;
        }
        // Player moving through left side entry, switch camera to left-adjacent room
        else if (playerPosition.x <= ROOM_POSITION_X - xBound)
        {
            cameraTransform = new Vector3(
                mainCamera.transform.position.x - cameraTranslateX,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = cameraTransform;

            player.transform.position = new Vector3(
                player.transform.position.x - playerTranslateX,
                player.transform.position.y,
                player.transform.position.z);

            ROOM_POSITION_X -= cameraTranslateX;
        }
        // Player moving through top entry, switch camera to top-adjacent room
        if (playerPosition.y >= ROOM_POSITION_Y + yBound)
        {
            cameraTransform = new Vector3(
                mainCamera.transform.position.x,
                mainCamera.transform.position.y + cameraTranslateY,
                mainCamera.transform.position.z);
            mainCamera.transform.position = cameraTransform;

            player.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + playerTranslateY,
                player.transform.position.z);

            ROOM_POSITION_Y += cameraTranslateY;
        }
        // Player moving through bottom entry, switch camera to bottom-adjacent room
        else if (playerPosition.y <= ROOM_POSITION_Y - yBound)
        {
            cameraTransform = new Vector3(
                mainCamera.transform.position.x,
                mainCamera.transform.position.y - cameraTranslateY,
                mainCamera.transform.position.z);
            mainCamera.transform.position = cameraTransform;

            player.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y - playerTranslateY,
                player.transform.position.z);

            ROOM_POSITION_Y -= cameraTranslateY;
        }
    }

    void PlugHandler()
    {
        GameObject plugLeft = GameObject.Find("PlugLeft");
        GameObject plugRight = GameObject.Find("PlugRight");
        GameObject plugTop = GameObject.Find("PlugTop");
        GameObject plugBottom = GameObject.Find("PlugBottom");
 
        float randValue = Random.value;
        if (randValue < .5f) // 50% of the time
        {
            plugLeft.SetActive(true);
        }
        else
        {
            plugLeft.SetActive(false);
        }

        randValue = Random.value;
        if (randValue < .5f) // 50% of the time
        {
            plugRight.SetActive(true);
        }
        else
        {
            plugRight.SetActive(false);
        }

        randValue = Random.value;
        if (randValue < .5f) // 50% of the time
        {
            plugTop.SetActive(true);
        }
        else
        {
            plugTop.SetActive(false);
        }

        randValue = Random.value;
        if (randValue < .5f) // 50% of the time
        {
            plugBottom.SetActive(true);
        }
        else
        {
            plugBottom.SetActive(false);
        }

        
    }
}
