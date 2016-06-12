using UnityEngine;
using System.Collections;

public class LobbyScript : MonoBehaviour {

	public GUISkin guiSkin;

    void OnGUI ()
    {
		GUI.skin = guiSkin;
		GUI.Box(new Rect(Screen.width/2 - 200, Screen.height/2 - 30, 400, 60), "Waiting for other players...");
    }

	// Use this for initialization
	void Start () {
		string userIp = GameObject.Find ("UserIPObj").GetComponent<UserIP>().IP;
		print ("UserIP " + userIp + " waiting for server...");

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
