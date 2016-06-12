using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;


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

public class Coordinate
{
    public float x;
    public float y;
}

public class StWindow : MonoBehaviour {

    public GameObject bombPrefab;

    private bool _isStarted = false;
	private bool _isServer = false;
	string ip  = GetLocalIPAddress() ;
	int   port = 8934;
	private int _messageIdx = 0;

	private int m_ConnectionId = 0;
	private int m_WebSocketHostId = 0;
	private int m_GenericHostId = 0;

	private string m_SendString = "";
	private string m_RecString  = "";
	private ConnectionConfig m_Config = null;
	private byte m_CommunicationChannel = 0;
    private int connections = 0;

    private bool player_assigned = false;
    public int player_assignment;
    private Player_Info player_info;

    private float timer = 0f;
    private float latency_check = 10f;

    private float gamestate_timer = 0f;
    private float gamestate_check = 5f;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private bool waiting = false;
    private double send_time;
    private double latency_estimation1 = 0;
    private double latency_estimation2 = 0;
    private double latency_estimation3 = 0;
    private double latency_estimation4 = 0;
    private int counter = 0;
    private int penalty1 = 0;
    private int penalty2 = 0;
    private int penalty3 = 0;
    private int penalty4 = 0;

    void Start()
	{
		m_Config = new ConnectionConfig();                                         //create configuration containing one reliable channel
        m_CommunicationChannel = m_Config.AddChannel(QosType.Reliable);
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");
        player4 = GameObject.Find("Player4");

        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);

    }

    void OnGUI () {
		GUI.Box(new Rect(5, 5, 450, 450), "window");		
		if( !_isStarted )
		{
			GUI.Label(new Rect(10, 10, 250, 30), ip, "TextField");                                      //initializes IP and port text boxes
			GUI.Label(new Rect(10, 40, 250, 30), port.ToString(), "TextField");

#if !(UNITY_WEBGL && !UNITY_EDITOR)
			if ( GUI.Button( new Rect(10, 70, 250, 30), "start server" ) )
			{
				_isStarted = true;
				_isServer = true;
				NetworkTransport.Init();

				HostTopology topology = new HostTopology(m_Config, 12);
				m_WebSocketHostId = NetworkTransport.AddWebsocketHost(topology, port, null);           //add 2 host one for udp another for websocket, as websocket works via tcp we can do this
				m_GenericHostId = NetworkTransport.AddHost(topology, port, null);
			}
#endif
            //if (GUI.Button(new Rect(10, 100, 250, 30), "start client"))
            //{
            //    _isStarted = true;
            //    _isServer = false;
            //    NetworkTransport.Init();

            //    HostTopology topology = new HostTopology(m_Config, 12);
            //    m_GenericHostId = NetworkTransport.AddHost(topology, 0); //any port for udp client, for websocket second parameter is ignored, as webgl based game can be client only
            //    byte error;
            //    m_ConnectionId = NetworkTransport.Connect(m_GenericHostId, ip, port, 0, out error);
            //}
        }
        else
		{
			GUI.Label(new Rect(10, 20, 250, 500), "Player 1 Latency: " + latency_estimation1 + "ms");
            GUI.Label(new Rect(10, 40, 250, 500), "Player 2 Latency: " + latency_estimation2 + "ms");
            GUI.Label(new Rect(10, 60, 250, 500), "Player 3 Latency: " + latency_estimation3 + "ms");
            GUI.Label(new Rect(10, 80, 250, 500), "Player 4 Latency: " + latency_estimation4 + "ms");
            GUI.Label(new Rect(10, 100, 250, 500), "Player 1 Strikes: " + penalty1);
            GUI.Label(new Rect(10, 120, 250, 500), "Player 2 Strikes: " + penalty2);
            GUI.Label(new Rect(10, 140, 250, 500), "Player 3 Strikes: " + penalty3);
            GUI.Label(new Rect(10, 160, 250, 500), "Player 4 Strikes: " + penalty4);




            if (GUI.Button(new Rect(10, 180, 250, 50), "stop"))
			{
				_isStarted = false;
				NetworkTransport.Shutdown();
			}
		}
	}

	void FixedUpdate()
	{

		if (!_isStarted)
			return;
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        if(!waiting)
            timer += Time.deltaTime;
        /*
        gamestate_timer += Time.deltaTime;
        if (gamestate_timer >= gamestate_check)
        {
            GameObject[] destroyable_blocks = GameObject.FindGameObjectsWithTag("block_destroyable");

            List<Coordinate> Block_Coordinates;
            Block_Coordinates = new List<Coordinate>();

            for (int block = 0; block < destroyable_blocks.Length; block++)
            {
                Coordinate block_coordinate;
                block_coordinate = new Coordinate();
                block_coordinate.x = destroyable_blocks[block].transform.position.x;
                block_coordinate.y = destroyable_blocks[block].transform.position.y;
                Block_Coordinates.Add(block_coordinate);
            }

            string gamestate_message = JsonUtility.ToJson(Block_Coordinates);

            for (int client = 0; client <= connections; client++)
            {
                byte[] bytes = new byte[gamestate_message.Length * sizeof(char)];
                System.Buffer.BlockCopy(gamestate_message.ToCharArray(), 0, bytes, 0, bytes.Length);
                NetworkTransport.Send(recHostId, client, m_CommunicationChannel1, bytes, bytes.Length, out error);
            }
            }
            */





        switch (recData)
		{

			case NetworkEventType.Nothing:
				break;

			case NetworkEventType.ConnectEvent:
				{
                    connections++;
                    m_SendString = connectionId.ToString();
                    byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                    NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                    if (connectionId == 1)
                        player1.SetActive(true);
                    if (connectionId == 2)
                        player2.SetActive(true);
                    if (connectionId == 3)
                        player3.SetActive(true);
                    if (connectionId == 4)
                        player4.SetActive(true);
                    break;
				}

			case NetworkEventType.DataEvent:  //if server will receive echo it will send it back to client, when client will receive echo from serve wit will send other message
                {
                    if (timer >= latency_check && connections >= 1)
                    {
                        timer = 0f;
                        waiting = true;
                        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                        send_time = (double)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;

                        string timestamp_message = "TimeStamp";

                        byte[] bytes = new byte[timestamp_message.Length * sizeof(char)];
                        System.Buffer.BlockCopy(timestamp_message.ToCharArray(), 0, bytes, 0, bytes.Length);
                        
                        for (int client = 1; client <= connections; client++)
                        {
                            NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                        }

                    }

                    char[] chars = new char[dataSize / sizeof(char)];
                    System.Buffer.BlockCopy(recBuffer, 0, chars, 0, dataSize);
                    m_RecString = new string(chars);

                    if (m_RecString.Contains("TimeStamp"))
                    {
                        
                        counter++;
                        if (counter == connections)
                        {
                            waiting = false;
                            counter = 0;
                        }
                        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                        double time_received = (double)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
                        string timestamp_message = m_RecString.Substring(9);
                  
                        if (connectionId == 1)
                            latency_estimation1 = (time_received - send_time);
                        if (connectionId == 2)
                            latency_estimation2 = (time_received - send_time);
                        if (connectionId == 3)
                            latency_estimation3 = (time_received - send_time);
                        if (connectionId == 4)
                            latency_estimation4 = (time_received - send_time);
                        if (latency_estimation1 > 500)
                            penalty1++;
                        if (latency_estimation2 > 500)
                            penalty2++;
                        if (latency_estimation3 > 500)
                            penalty3++;
                        if (latency_estimation4 > 500)
                            penalty4++;
                        if (latency_estimation1 < 500 && penalty1 != 0)
                            penalty1--;
                        if (latency_estimation2 < 500 && penalty2 != 0)
                            penalty2--;
                        if (latency_estimation3 < 500 && penalty3 != 0)
                            penalty3--;
                        if (latency_estimation4 < 500 && penalty4 != 0)
                            penalty4--;

                        if (penalty1 >= 3)
                        {
                            m_SendString = "LAG";
                            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            NetworkTransport.Send(recHostId, 1, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                        if (penalty2 >= 3)
                        {
                            m_SendString = "LAG";
                            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            NetworkTransport.Send(recHostId, 2, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                        if (penalty3 >= 3)
                        {
                            m_SendString = "LAG";
                            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            NetworkTransport.Send(recHostId, 3, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                        if (penalty4 >= 3)
                        {
                            m_SendString = "LAG";
                            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            NetworkTransport.Send(recHostId, 4, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                    }

                    else {
                        m_SendString = m_RecString;
                        player_info = JsonUtility.FromJson<Player_Info>(m_RecString);

                        byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                        System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                        for (int client = 1; client <= connections; client++)
                        {
                            if (client != connectionId) {
                                if (player_info.dead || player_info.bomb)
                                    NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                                else
                                    NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                            }
                         }

                        
                        float h = player_info.horizontal;
                        float v = player_info.vertical;
                        Vector3 pos = player_info.pos;

                        if (player_info.assignment == 1)
                        {
                            if (player_info.dead)
                            {
                                player1.SetActive(false);
                                return;
                            }
                            GameObject.Find("Player1").transform.position = player_info.pos;
                            GameObject.Find("Player1").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player1").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player1").GetComponent<Animator>().SetInteger("Y", (int)v);


                        }

                        if (player_info.assignment == 2)
                        {
                            if (player_info.dead)
                            {
                                player2.SetActive(false);
                                return;
                            }
                            GameObject.Find("Player2").transform.position = player_info.pos;
                            GameObject.Find("Player2").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player2").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player2").GetComponent<Animator>().SetInteger("Y", (int)v);
                        }

                        if (player_info.assignment == 3)
                        {
                            if (player_info.dead)
                            {
                                player3.SetActive(false);
                                return;
                            }
                            GameObject.Find("Player3").transform.position = player_info.pos;
                            GameObject.Find("Player3").GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;
                            // Set Animation Parameters
                            GameObject.Find("Player3").GetComponent<Animator>().SetInteger("X", (int)h);
                            GameObject.Find("Player3").GetComponent<Animator>().SetInteger("Y", (int)v);
                        }
                        if (player_info.assignment == 4)
                        {
                            if (player_info.dead)
                            {
                                player4.SetActive(false);
                                return;
                            }
                            GameObject.Find("Player4").transform.position = player_info.pos;
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
                    }
                    break;
                }
               
			case NetworkEventType.DisconnectEvent:
		    {
                    player_info.assignment = connectionId;
                    player_info.dead = true;
                    if (connectionId == 1 && GameObject.Find("Player1") != null)
                    {
                        player1.SetActive(false);
                        
                    }

                    if (connectionId == 2  && GameObject.Find("Player2") != null)
                    {
                        player2.SetActive(false);

                    }

                    if (connectionId == 3 && GameObject.Find("Player3") != null)
                    {
                        player3.SetActive(false);

                    }

                    if (connectionId == 4 && GameObject.Find("Player4") != null)
                    {
                        player4.SetActive(false);

                    }

                    m_SendString = JsonUtility.ToJson(player_info);

                    byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);

                    for (int client = 1; client <= connections; client++)
                    {
                        if (client != connectionId)
                            NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                    }
                    //Debug.Log(String.Format("DisConnect from host {0} connection {1}", recHostId, connectionId));
                    connections--;
                    if (connections == 0)
                    {
                        penalty1 = 0;
                        penalty2 = 0;
                        penalty3 = 0;
                        penalty4 = 0;
                    }
                    player_info.dead = false;
                    break;
				}
		}
	}

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("Local IP Address Not Found!");
    }
}
