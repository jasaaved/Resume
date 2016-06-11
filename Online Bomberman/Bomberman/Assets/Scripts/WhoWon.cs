using UnityEngine;
using System.Collections;

public class WhoWon : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("Canvas/Text").GetComponent<UnityEngine.UI.Text>().text = "You blew up!\n" + GameObject.Find("Winner").GetComponent<WinnerHolder>().winner;

    }
}
