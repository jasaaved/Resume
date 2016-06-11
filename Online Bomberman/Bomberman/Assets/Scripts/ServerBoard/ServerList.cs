using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ServerList : MonoBehaviour {

	public GameObject serverEntryPrefab;
	public GameObject selectedServer;
	string PREFAB_ITEM_SERVER_NAME = "Server Name";
	string PREFAB_ITEM_PLAYERS = "Players";
	string PREFAB_ITEM_OPEN_CLOSED = "Open_Closed";
	ServerManager serverManager;
	int lastChangeCounter;
    public string selected_server = "";

	// Use this for initialization
	void Start () {
		serverManager = GameObject.FindObjectOfType<ServerManager>();
		lastChangeCounter = serverManager.GetChangeCounter();
	}
	
	// Update is called once per frame
	void Update () {
		if(serverManager == null) {
			Debug.LogError("You forgot to add the server manager component to a game object!");
			return;
		}

		if(serverManager.GetChangeCounter() == lastChangeCounter) {
			// No change since last update!
			return;
		}

		lastChangeCounter = serverManager.GetChangeCounter();

		while(this.transform.childCount > 0) {
			Transform c = this.transform.GetChild(0);
			c.SetParent(null);  // Become Batman
			Destroy (c.gameObject);
		}

		string[] names = serverManager.GetServerNames();
		
		foreach(string name in names) {
			GameObject server = (GameObject)Instantiate(serverEntryPrefab);
			server.transform.SetParent(this.transform);
			Toggle serverToggle = server.GetComponent<Toggle> ();
			serverToggle.onValueChanged.AddListener ( (value) => {
					OnServerToggled(value);
				} 
			);
			server.transform.Find (PREFAB_ITEM_SERVER_NAME).GetComponent<Text>().text = name;
			server.transform.Find (PREFAB_ITEM_PLAYERS).GetComponent<Text>().text = serverManager.GetServer(name, ServerManager.KEY_PLAYERS).ToString();
			server.transform.Find (PREFAB_ITEM_OPEN_CLOSED).GetComponent<Text>().text = serverManager.GetServer(name, ServerManager.KEY_OPEN_CLOSED).ToString();
		}
	}

	public void OnServerToggled(bool value){
		selectedServer = EventSystem.current.currentSelectedGameObject;
		Debug.Log ("SELECTED " + selectedServer.transform.Find(PREFAB_ITEM_SERVER_NAME).GetComponent<Text>().text);
        selected_server = selectedServer.transform.Find(PREFAB_ITEM_SERVER_NAME).GetComponent<Text>().text;
    }
}
