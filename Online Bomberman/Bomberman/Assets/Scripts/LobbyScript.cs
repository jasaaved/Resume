using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyScript : MonoBehaviour {

	private string URL = "http://alpha-wolf-squadron-abagg.c9users.io/Login.php";
	public GUISkin guiSkin;
	private GameObject player1;
	private GameObject player2;
	private GameObject player3;
	private GameObject player4;
    private int size = 1;
	private string username;
    private string m_SendString;
    private bool pressed = false;
	private bool allowQuitting = false;

    void OnGUI () {
		GUI.skin = guiSkin;

        if (size == 1)
			GUI.Box(new Rect(Screen.width/2 - 200, Screen.height/2 - 150, 400, 60), "Waiting for other players...");
        if (size > 1) {
            if (!pressed)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 130, Screen.height - 75, 248, 46), "Ready?"))
                {
                    m_SendString = "ready8964";
                    byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                    int recHostId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().recHostId;
                    int connectionId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().connectionId;
                    int channelId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().channelId;
                    byte m_CommunicationChannel;
                    m_CommunicationChannel = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().m_CommunicationChannel;
                    int m_ConnectionId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().m_ConnectionId;
                    byte error;
                    NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                    pressed = true;
                }
            }
            if (pressed)
            {
				GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height - 75 , 400, 60), "Ready!");
            }
          }

//		string name = GameObject.Find ("UserIPObj").GetComponent<UserIP>().display_name;
//		var centeredStyle = GUI.skin.GetStyle ("Label");
//		centeredStyle.alignment = TextAnchor.MiddleCenter;
//		GUI.Label (new Rect(player1.transform.position.x + 390, player1.transform.position.y + 100, 150, 35), name, centeredStyle);

    }

	// Use this for initialization
	void Start () {
		username = GameObject.Find ("UserIPObj").GetComponent<UserIP>().username;

		player1 = GameObject.Find("Player1");
		player2 = GameObject.Find("Player2");
		player3 = GameObject.Find("Player3");
		player4 = GameObject.Find("Player4");

		player1.SetActive(false);
		player2.SetActive(false);
		player3.SetActive(false);
		player4.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().lobby_chosen == "Server 1")
        {
            size = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().servers.ServerA;
        }

        if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().lobby_chosen == "Server 2")
        {
            size = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().servers.ServerB;
        }
        if (GameObject.Find("Player1") == null && size >= 1)
            player1.SetActive(true);
        if (GameObject.Find("Player2") == null && size >= 2)
            player2.SetActive(true);
        if (GameObject.Find("Player3") == null && size >= 3)
            player3.SetActive(true);
        if (GameObject.Find("Player4") == null && size >= 4)
            player4.SetActive(true);

        if (GameObject.Find("Player1") != null && size == 1)
        {
            player2.SetActive(false);
            player3.SetActive(false);
            player4.SetActive(false);
        }
        if (GameObject.Find("Player2") != null && size == 2)
        {
            player3.SetActive(false);
            player4.SetActive(false);
        }
        if (GameObject.Find("Player3") != null && size == 3)
        {

            player4.SetActive(false);
        }

    }

	void OnApplicationQuit(){
		StartCoroutine(logoutUser ());

		if (!allowQuitting)
			Application.CancelQuit();
	}

	IEnumerator logoutUser(){
		string logoutURL = URL + "?username=" + username + "&loggedIn=0" + "&Act=" + "Logout";

		WWW w = new WWW(logoutURL); //here we create a var called 'w' and we sync with our URL and the form
		yield return w; //we wait for the form to check the PHP file, so our game dont just hang

		if (w.error != null) {
			print (w.error);
		} else {
			print (w.text);
		}

		allowQuitting = true;
		Application.Quit ();
	}
}
