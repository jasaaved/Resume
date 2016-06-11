using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class ServerManager : MonoBehaviour {

	Dictionary< string, Dictionary<string, string> > foundServers;
	public static string KEY_PLAYERS = "players";
	public static string KEY_OPEN_CLOSED = "open_closed";
	string valOpen = "OPEN";
	string valClosed = "CLOSED";
	int lastChangeCounter = -1;
	public int changeCounter = 0;
    public string selected_server;
    private string m_SendString;

    void Start() {
		lastChangeCounter = -1;
	}

	void Update() {
		if(GetChangeCounter() == lastChangeCounter) {
			// No change since last update!
			return;
		}

		if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().received) {
			AddServer ("Server 1", KEY_PLAYERS, GameObject.Find ("LobbyNetwork").GetComponent<LobbyNetwork> ().servers.ServerA.ToString () + "/4");
			if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().servers.startedA)
				AddServer("Server 1", KEY_OPEN_CLOSED, valClosed);
			if (!GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().servers.startedA)
				AddServer("Server 1", KEY_OPEN_CLOSED, valOpen);

			AddServer("Server 2", KEY_PLAYERS, GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().servers.ServerB.ToString() + "/4");
			if (GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().servers.startedB)
				AddServer("Server 2", KEY_OPEN_CLOSED, valClosed);
			if (!GameObject.Find ("LobbyNetwork").GetComponent<LobbyNetwork> ().servers.startedB)
				AddServer ("Server 2", KEY_OPEN_CLOSED, valOpen);

			lastChangeCounter = GetChangeCounter();
        }
    }

	void Init() {
		if (foundServers != null) {
			return;
		}

		// Every server contains the number of players and if it's open/closed
		foundServers = new Dictionary<string, Dictionary<string, string>>();
	}

	void loadDummyData(){
		AddServer("Server 1", KEY_PLAYERS, "0/4");
		AddServer("Server 1", KEY_OPEN_CLOSED, valOpen);
		AddServer("Server 2", KEY_PLAYERS, "2/4");
		AddServer("Server 2", KEY_OPEN_CLOSED, valOpen);
		AddServer("Server 3", KEY_PLAYERS, "4/4");
		AddServer("Server 3", KEY_OPEN_CLOSED, valClosed);
		AddServer("Server 4", KEY_PLAYERS, "1/4");
		AddServer("Server 4", KEY_OPEN_CLOSED, valOpen);
	}

	public void Reset() {
		changeCounter++;
		foundServers = null;
	}

	public string GetServer(string serverName, string key) { // key will be either "players" or "open_closed"
		Init ();

		if(foundServers.ContainsKey(serverName) == false) {
			// We have no score record at all for this server name
			return "";
		}

		if(foundServers[serverName].ContainsKey(key) == false) {
			return "";
		}

		return foundServers[serverName][key];
	}

	public void AddServer(string serverName, string key, string value) {
		Init ();

		changeCounter++;

		if(foundServers.ContainsKey(serverName) == false) {
			foundServers[serverName] = new Dictionary<string, string>();
		}

		foundServers[serverName][key] = value;

		Debug.Log ("SERVER ADDED  = " + serverName);
	}

	public int GetChangeCounter() {
		return changeCounter;
	}

	public string[] GetServerNames() {
		Init ();
		return foundServers.Keys.ToArray();
	}
	
	public string[] GetServerNames(string sortingScoreType) {
		Init ();
		return foundServers.Keys.OrderByDescending( n => GetServer(n, sortingScoreType) ).ToArray();
	}

	public void CONNECT() {
        selected_server = GameObject.Find("Canvas/Server Board Panel/Server List").GetComponent<ServerList>().selected_server;

		if(foundServers[selected_server][KEY_OPEN_CLOSED] == valClosed){
			return;
		}

        if (foundServers[selected_server][KEY_PLAYERS] == "4/4")
        {
            return;
        }

        if (selected_server == "Server 1")
		{
            m_SendString = "A";

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
        }

        if (selected_server == "Server 2")
        {
            m_SendString = "B";

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
        }
        GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().lobby_chosen = selected_server;
        GameObject.Find("UserIPObj").GetComponent<UserIP>().server_choosen = selected_server;
        GameObject gameObj = GameObject.Find("LobbyNetwork");
        DontDestroyOnLoad(gameObj);
        SceneManager.LoadScene ("LobbyScene");
	}

}
