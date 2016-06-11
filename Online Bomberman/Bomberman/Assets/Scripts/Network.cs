using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;


public class Player_Info
{
    public int assignment;
    public Vector3 pos;
    public float horizontal;
    public float vertical;
    public bool bomb;
    public bool dead;
    public int lives = 3;
}

public class Network : MonoBehaviour
{
    public GameObject bombPrefab;
    public float speed = 6;
    private int _messageIdx = 0;
    string ip = "192.168.1.109";
	string username;
	private string URL = "http://alpha-wolf-squadron-abagg.c9users.io/Login.php";
	private bool allowQuitting = false;
    int port = 1234;
    public int m_ConnectionId = 0;
    private int m_WebSocketHostId = 0;
    private int m_GenericHostId = 0;

    private string m_SendString = "";
    private string m_RecString = "";
    private ConnectionConfig m_Config = null;
    public byte m_CommunicationChannel = 0;
 

    private bool player_assigned = false;
    public int player_assignment;
    private Player_Info player_info;

   public int recHostId;
   public int connectionId;
   public int channelId;
   public byte[] recBuffer = new byte[1024];
   public int bufferSize = 1024;
   public int dataSize;
   public byte error;

   private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    public int player1lives = 3;
    public int player2lives = 3;
    public int player3lives = 3;
    public int player4lives = 3;


    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().lobby_chosen == "Server 1")
        {
            ip = GameObject.Find("UserIPObj").GetComponent<UserIP>().AIP;
			username = GameObject.Find("UserIPObj").GetComponent<UserIP>().username;
            port = System.Int32.Parse(GameObject.Find("UserIPObj").GetComponent<UserIP>().APort);
        }

        if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().lobby_chosen == "Server 2")
        {
            ip = GameObject.Find("UserIPObj").GetComponent<UserIP>().BIP;
            port = System.Int32.Parse(GameObject.Find("UserIPObj").GetComponent<UserIP>().BPort);
        }

        Destroy(GameObject.Find("LobbyNetwork"));
        NetworkTransport.Shutdown();
        player_info = new Player_Info();
        m_Config = new ConnectionConfig();
        m_CommunicationChannel = m_Config.AddChannel(QosType.Reliable);
        
        NetworkTransport.Init();

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");
        player4 = GameObject.Find("Player4");

        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);

        HostTopology topology = new HostTopology(m_Config, 12);
        m_GenericHostId = NetworkTransport.AddHost(topology, 0); //any port for udp client, for websocket second parameter is ignored, as webgl based game can be client only
        byte error;
        m_ConnectionId = NetworkTransport.Connect(m_GenericHostId, ip, port, 0, out error);
        Debug.Log(m_ConnectionId);
    }

    void FixedUpdate()
    {
     
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        string m_RecString;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        //move to lobby

        if (player_assigned == false)
        {
            char[] chars = new char[dataSize / sizeof(char)];
            System.Buffer.BlockCopy(recBuffer, 0, chars, 0, dataSize);
            m_RecString = new string(chars);
            player_assignment = System.Int32.Parse(m_RecString);
            player_assigned = true;
            if (player_assignment == 1)
                player1.SetActive(true);
            else if (player_assignment == 2)
            {
                player1.SetActive(true);
                player2.SetActive(true);
            }
            else if (player_assignment == 3)
            {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
            }
            else {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(true);
            }
        }


        switch (recData)
        {

            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.DataEvent:  //if server will receive echo it will send it back to client, when client will receive echo from serve wit will send other message
                {
                  
                    char[] chars = new char[dataSize / sizeof(char)];
                    System.Buffer.BlockCopy(recBuffer, 0, chars, 0, dataSize);
                    m_RecString = new string(chars);

                    if (m_RecString.Contains("TimeStamp"))
                    {
                        string latency_message = "TimeStamp"; // + time_received.ToString();
                        byte[] bytes = new byte[latency_message.Length * sizeof(char)];
                        System.Buffer.BlockCopy(latency_message.ToCharArray(), 0, bytes, 0, bytes.Length);
                        NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                    }

                    else if (m_RecString.Contains("LAG")){
                        NetworkTransport.Shutdown();
                        SceneManager.LoadScene("Lag");
                    }
                    else
                    {
                        player_info = JsonUtility.FromJson<Player_Info>(m_RecString);
                        float h = player_info.horizontal;
                        float v = player_info.vertical;
                        Vector3 pos = player_info.pos;

                        if (player_info.assignment == 1)
                        {
                            if (GameObject.Find("Player1") == null)
                                player1.SetActive(true);
                            if (player_info.dead)
                            {
                                player1.SetActive(false);
                                player1lives = 0;
                                return;
                            }
                         
                            GameObject.Find("Player1").transform.position = player_info.pos;
                            player1lives = player_info.lives;
                            GameObject.Find("Player1").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player1").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player1").GetComponent<Animator>().SetInteger("Y", (int)v);
                        }

                        if (player_info.assignment == 2)
                        {
                            if (GameObject.Find("Player2") == null)
                                player2.SetActive(true);
                            if (player_info.dead)
                            {
                                player2.SetActive(false);
                                player2lives = 0;
                                return;
                            }
                          
                            GameObject.Find("Player2").transform.position = player_info.pos;
                            player2lives = player_info.lives;
                            GameObject.Find("Player2").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player2").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player2").GetComponent<Animator>().SetInteger("Y", (int)v);
                        }

                        if (player_info.assignment == 3)
                        {
                            if (GameObject.Find("Player3") == null)
                                player3.SetActive(true);
                            if (player_info.dead)
                            {
                                player3.SetActive(false);
                                player3lives = 0;
                                return;
                            }
                          
                            GameObject.Find("Player3").transform.position = player_info.pos;
                            player3lives = player_info.lives;
                            GameObject.Find("Player3").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player3").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player3").GetComponent<Animator>().SetInteger("Y", (int)v);
                        }

                        if (player_info.assignment == 4)
                        {
                            if (GameObject.Find("Player4") == null)
                                player4.SetActive(true);
                            if (player_info.dead)
                            {
                                player4.SetActive(false);
                                player4lives = 0;
                                return;
                            }

                            GameObject.Find("Player4").transform.position = player_info.pos;
                            player4lives = player_info.lives;
                            GameObject.Find("Player4").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player4").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player4").GetComponent<Animator>().SetInteger("Y", (int)v);
                        }

                        if (player_info.bomb == true)
                        {
                            pos.x = Mathf.Round(pos.x);
                            pos.y = Mathf.Round(pos.y);
                            Instantiate(bombPrefab, pos, Quaternion.identity);
                        }

                        if (GameObject.Find("ScriptHolder").GetComponent<WinLossChecker>().started == false)
                            GameObject.Find("ScriptHolder").GetComponent<WinLossChecker>().started = true;
                    }

                    break;
                }
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