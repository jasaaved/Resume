using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;

public class Servers
{
    public int ServerA = 0;
    public int ServerB = 0;
    public bool startedA = false;
    public bool startedB = false;

    public string ipA = "";
    public string portA = "";

    public string ipB = "";
    public string portB = "";
}

public class LobbyNetwork : MonoBehaviour
{
	private string URL = "http://alpha-wolf-squadron-abagg.c9users.io/Login.php";
	private bool allowQuitting = false;

    private int _messageIdx = 0;
    string ip = "";
	string username = "";
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


    // Use this for initialization
    void Start()
    {
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

        switch (recData)
        {

            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.DataEvent:  //if server will receive echo it will send it back to client, when client will receive echo from serve wit will send other message
                {
                    received = true;
                    char[] chars = new char[dataSize / sizeof(char)];
                    System.Buffer.BlockCopy(recBuffer, 0, chars, 0, dataSize);
                    m_RecString = new string(chars);
                    Debug.Log(m_RecString);
                    if (m_RecString != "full" && m_RecString != "started" && m_RecString != "start8965" && !m_RecString.Contains("chat"))
                    {
                        servers = JsonUtility.FromJson<Servers>(m_RecString);
                        GameObject.Find("_SCRIPTS_").GetComponent<ServerManager>().changeCounter++;
                        GameObject.Find("UserIPObj").GetComponent<UserIP>().AIP = servers.ipA;
                        GameObject.Find("UserIPObj").GetComponent<UserIP>().APort = servers.portA;
                        GameObject.Find("UserIPObj").GetComponent<UserIP>().BIP = servers.ipB;
                        GameObject.Find("UserIPObj").GetComponent<UserIP>().BPort = servers.portB;

                    }

                    else if (m_RecString == "full" || m_RecString == "started")
                    {
                        Destroy(GameObject.Find("LobbyNetwork"));
                        NetworkTransport.Shutdown();
                        SceneManager.LoadScene("FullScene");
                    }

                    else if (m_RecString == "start8965")
                    {
                        GameObject gameObj = GameObject.Find("LobbyNetwork");
                        DontDestroyOnLoad(gameObj);
                        SceneManager.LoadScene("MainScene");
                    }

                    else if (m_RecString.Contains("chat"))
                    {
                        string[] words = m_RecString.Split(';');
                        GameObject.Find("GameObject").GetComponent<ChatMaster>().AddChatEntry(words[1], words[2]);    
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
