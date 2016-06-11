using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;

public class WinNetwork : MonoBehaviour
{

    private int _messageIdx = 0;
    string ip = "";
    int port = 5897;
    public int m_ConnectionId = 0;
    private int m_WebSocketHostId = 0;
    private int m_GenericHostId = 0;

    private string m_SendString = "";
    private string m_RecString = "";
    private ConnectionConfig m_Config = null;
    public byte m_CommunicationChannel = 0;

    public int recHostId;
    public int connectionId;
    public int channelId;
    public byte[] recBuffer = new byte[1024];
    public int bufferSize = 1024;
    public int dataSize;
    public byte error;
    public Servers servers = new Servers();
    public bool received = false;
    public string lobby_chosen;
    private bool sent;

	private string URL = "http://alpha-wolf-squadron-abagg.c9users.io/Login.php";
	private string username;
	private bool allowQuitting = false;

    // Use this for initialization
    void Start()
    {
        NetworkTransport.Shutdown();
        ip = GameObject.Find("UserIPObj").GetComponent<UserIP>().IP;
		username = GameObject.Find("UserIPObj").GetComponent<UserIP>().username;
        m_Config = new ConnectionConfig();
        m_CommunicationChannel = m_Config.AddChannel(QosType.Reliable);
        NetworkTransport.Init();

        HostTopology topology = new HostTopology(m_Config, 12);
        m_GenericHostId = NetworkTransport.AddHost(topology, 0); //any port for udp client, for websocket second parameter is ignored, as webgl based game can be client only
        byte error;
        m_ConnectionId = NetworkTransport.Connect(m_GenericHostId, ip, port, 0, out error);

    }

    void FixedUpdate()
    {
        if (sent)
            return;
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;



        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        //move to lobby

        switch (recData)
        {

            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.DataEvent:  //if server will receive echo it will send it back to client, when client will receive echo from serve wit will send other message
                {
                    if (sent)
                        return;
                    if (GameObject.Find("UserIPObj").GetComponent<UserIP>().server_choosen == "Server 1")
                    {
                        string m_SendString = "SERVER29865";
                        byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                        System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                        NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                    }

                    if (GameObject.Find("UserIPObj").GetComponent<UserIP>().server_choosen == "Server 2")
                    {
                        string m_SendString = "SERVER19865";
                        byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                        System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                        NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                    }

                    sent = true;
                    NetworkTransport.Shutdown();


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
