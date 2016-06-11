using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkShutdown : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetworkTransport.Shutdown();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
