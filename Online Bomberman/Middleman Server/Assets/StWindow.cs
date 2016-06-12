using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;


public class Servers
{
    public int ServerA = 0;
    public int ServerB = 0;
    public bool startedA = false;
    public bool startedB = false;

    public string ipA = "";
    public string portA = "8934";

    public string ipB = "";
    public string portB = "6373";
}

public class Players
{
    public int player1 = 0;
    public int player2 = 0;
    public int player3 = 0;
    public int player4 = 0;
    public int player5 = 0;
    public int player6 = 0;
    public int player7 = 0;
    public int player8 = 0;

}



public class StWindow : MonoBehaviour
{

    public GameObject bombPrefab;

    private bool _isStarted = false;
    private bool _isServer = false;
    string ip = GetLocalIPAddress();
    int port = 5897;
    private int _messageIdx = 0;

    private int m_ConnectionId = 0;
    private int m_WebSocketHostId = 0;
    private int m_GenericHostId = 0;

    private string m_SendString = "";
    private string m_RecString = "";
    private ConnectionConfig m_Config = null;
    private byte m_CommunicationChannel = 0;
    private int connections = 0;

    private Servers servers = new Servers();
    private Players players = new Players();
    private int[] numbers = new int[] { 0, 0, 0, 0, 0, 0, 0, 0};
    private int[] readiness = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
    string ids = "";
    byte[] bytes;
    public int serverA_ready = 0;
    public int serverB_ready = 0;


    void Start()
    {
        m_Config = new ConnectionConfig();                                         //create configuration containing one reliable channel
        m_CommunicationChannel = m_Config.AddChannel(QosType.Reliable);
        

    }

    void OnGUI()
    {
        GUI.Box(new Rect(5, 5, 450, 450), "Server Info");
        if (!_isStarted)
        {
            GUI.Label(new Rect(10, 35, 250, 30), "Middleman Server IP: "+ ip);                                      //initializes IP and port text boxes
            GUI.Label(new Rect(10, 65, 250, 30), "Middleman Server Port: " + port.ToString());

            GUI.Label(new Rect(10, 95, 250, 500), "Server A IP: ");
            servers.ipA = GUI.TextField(new Rect(90, 95, 200, 20), servers.ipA, 25);
            GUI.Label(new Rect(10, 125, 250, 500), "Server A Port: " + servers.portA);
            

            GUI.Label(new Rect(10, 155, 250, 500), "Server B IP: ");
            servers.ipB = GUI.TextField(new Rect(90, 155, 200, 20), servers.ipB, 25);
            GUI.Label(new Rect(10, 185, 250, 500), "Server B Port: " + servers.portB);
            


           


#if !(UNITY_WEBGL && !UNITY_EDITOR)

            if (GUI.Button(new Rect(10, 215, 250, 30), "start server"))
            {
                _isStarted = true;
                _isServer = true;
                NetworkTransport.Init();

                HostTopology topology = new HostTopology(m_Config, 8);
                m_WebSocketHostId = NetworkTransport.AddWebsocketHost(topology, port, null);           //add 2 host one for udp another for websocket, as websocket works via tcp we can do this
                m_GenericHostId = NetworkTransport.AddHost(topology, port, null);
            }
            
            
#endif

        }
        else
        {
            GUI.Label(new Rect(10, 20, 250, 30), "Middleman Server IP: " + ip);                                      //initializes IP and port text boxes
            GUI.Label(new Rect(10, 50, 250, 30), "Middleman Server Port: " + port.ToString());

            GUI.Label(new Rect(10, 80, 250, 500), "Server 1 IP: " + servers.ipA);
            GUI.Label(new Rect(10, 110, 250, 500), "Server 1 Port: " + servers.portA);

            GUI.Label(new Rect(10, 140, 250, 500), "Server 2 IP: " + servers.ipB);
            GUI.Label(new Rect(10, 170, 250, 500), "Server 2 Port: " + servers.portB);


            GUI.Label(new Rect(10, 200, 250, 500), "Players Connected: " + connections);
            GUI.Label(new Rect(10, 230, 250, 500), "Server 1 Capacity: " + servers.ServerA);
            GUI.Label(new Rect(10, 260, 250, 500), "Server 1 Status: " + servers.startedA);
            GUI.Label(new Rect(10, 290, 250, 500), "Server 2 Capacity: " + servers.ServerB);
            GUI.Label(new Rect(10, 320, 250, 500), "Server 2 Status: " + servers.startedB);
            ids = string.Join(" ", new List<int>(numbers).ConvertAll(i => i.ToString()).ToArray());
            GUI.Label(new Rect(10, 350, 250, 500), "Player Vector: " + ids);
            GUI.Label(new Rect(10, 380, 250, 500), "ReadyA: " + serverA_ready);
            GUI.Label(new Rect(10, 410, 250, 500), "ReadyB: " + serverB_ready);






            if (GUI.Button(new Rect(10, 440, 250, 50), "stop"))
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

        switch (recData)
        {

            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                {
                    connections++;
                    m_SendString = JsonUtility.ToJson(servers);
                    byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                    NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                    break;
                }

            case NetworkEventType.DataEvent:  //if server will receive echo it will send it back to client, when client will receive echo from serve wit will send other message
                {
                    char[] chars = new char[dataSize / sizeof(char)];
                    System.Buffer.BlockCopy(recBuffer, 0, chars, 0, dataSize);
                    m_RecString = new string(chars);

                    if (numbers[connectionId - 1] != 0)
                    {
                        if (m_RecString == "quit894")
                        {

                            if (numbers[connectionId - 1] == 1)
                                servers.ServerA--;
                            if (numbers[connectionId - 1] == 2)
                                servers.ServerB--;
                            numbers[connectionId - 1] = 0;
                            m_SendString = JsonUtility.ToJson(servers);
                            bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                        }

                        if (m_RecString == "ready8964")
                        {
                            readiness[connectionId - 1] = 1;
                            if (numbers[connectionId - 1] == 1)
                            {
                                serverA_ready++;
                                if (serverA_ready == servers.ServerA)
                                {
                                    servers.startedA = true;
                                    m_SendString = "start8965";
                                    bytes = new byte[m_SendString.Length * sizeof(char)];
                                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                    for (int clients = 1; clients <= connections; clients++)
                                    {

                                        if (numbers[connectionId - 1] == numbers[clients - 1])
                                        {
                                            NetworkTransport.Send(recHostId, clients, m_CommunicationChannel, bytes, bytes.Length, out error);
                                            readiness[clients - 1] = 0;
                                        }
                                    }
                                    
                                    serverA_ready = 0;
                                    m_SendString = JsonUtility.ToJson(servers);
                                    bytes = new byte[m_SendString.Length * sizeof(char)];
                                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                    for (int client = 1; client <= connections; client++)
                                        NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                                }
                            }

                            if (numbers[connectionId - 1] == 2)
                            {
                                serverB_ready++;
                                if (serverB_ready == servers.ServerB)
                                {
                                    servers.startedB = true;
                                    m_SendString = "start8965";
                                    bytes = new byte[m_SendString.Length * sizeof(char)];
                                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                    for (int clients = 1; clients <= connections; clients++)
                                    {
                                        if (numbers[connectionId - 1] == numbers[clients - 1])
                                        {
                                            NetworkTransport.Send(recHostId, clients, m_CommunicationChannel, bytes, bytes.Length, out error);
                                            readiness[clients - 1] = 0;
                                        }
                                    }
                                    
                                    serverB_ready = 0;
                                    m_SendString = JsonUtility.ToJson(servers);
                                    bytes = new byte[m_SendString.Length * sizeof(char)];
                                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                    for (int client = 1; client <= connections; client++)
                                        NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                                }
                            }
                        }

                        else
                        {
                            m_SendString = m_RecString;
                            bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);

                            for (int clients = 1; clients <= connections; clients++)
                            {
                                if (clients != connectionId && numbers[connectionId - 1] == numbers[clients - 1])
                                    NetworkTransport.Send(recHostId, clients, m_CommunicationChannel, bytes, bytes.Length, out error);
                            }

                        }

                    }

                    else {


                        if (m_RecString == "A")
                        {
                            if (servers.ServerA >= 4)
                            {
                                m_SendString = "full";
                                bytes = new byte[m_SendString.Length * sizeof(char)];
                                System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                            }

                            else if (servers.startedA == true)
                            {
                                m_SendString = "started";
                                bytes = new byte[m_SendString.Length * sizeof(char)];
                                System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                            }

                            servers.ServerA++;
                            numbers[connectionId - 1] = 1;
                            m_SendString = JsonUtility.ToJson(servers);
                            bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            for (int client = 1; client <= connections; client++)
                                NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                        if (m_RecString == "B")
                        {
                            if (servers.ServerB >= 4)
                            {
                                m_SendString = "full";
                                bytes = new byte[m_SendString.Length * sizeof(char)];
                                System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                            }

                            else if (servers.startedB == true)
                            {
                                m_SendString = "started";
                                bytes = new byte[m_SendString.Length * sizeof(char)];
                                System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                                NetworkTransport.Send(recHostId, connectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                            }
                            servers.ServerB++;
                            numbers[connectionId - 1] = 2;
                            m_SendString = JsonUtility.ToJson(servers);
                            bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            for (int client = 1; client <= connections; client++)
                                NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
                        }

                        if (m_RecString == "SERVER29865")
                        {
                            servers.ServerA = 0;
                            servers.startedA = false;
                            m_SendString = JsonUtility.ToJson(servers);
                            bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            for (int client = 1; client <= connections; client++)
                                NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                        if (m_RecString == "SERVER19865")
                        {
                            servers.ServerB = 0;
                            servers.startedB = false;
                            m_SendString = JsonUtility.ToJson(servers);
                            bytes = new byte[m_SendString.Length * sizeof(char)];
                            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                            for (int client = 1; client <= connections; client++)
                                NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);

                        }

                    }
                    break;
                }

            case NetworkEventType.DisconnectEvent:
                {
                    if (numbers[connectionId - 1] == 1 && servers.startedA == false)
                        servers.ServerA--;
                    if (numbers[connectionId - 1] == 2 && servers.startedB == false)
                        servers.ServerB--;
                    connections--;
                    if (readiness[connectionId - 1] == 1 && numbers[connectionId - 1] == 1 && servers.startedA != true)
                        serverA_ready--;
                    if (readiness[connectionId - 1] == 1 && numbers[connectionId - 1] == 2 && servers.startedB != true)
                        serverB_ready--;
                    numbers[connectionId - 1] = 0;
                    readiness[connectionId - 1] = 0;

                    if (serverA_ready < 0)
                        serverA_ready = 0;
                    if (serverB_ready < 0)
                        serverB_ready = 0;

                    m_SendString = JsonUtility.ToJson(servers);
                    bytes = new byte[m_SendString.Length * sizeof(char)];
                    System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                    for (int client = 1; client <= connections; client++)
                        NetworkTransport.Send(recHostId, client, m_CommunicationChannel, bytes, bytes.Length, out error);
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

