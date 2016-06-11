using UnityEngine;
using System.Collections;

public class PlayerLives : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("Player4") != null || GameObject.Find("Network").GetComponent<Network>().player4lives != 3)
        {
            GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "White Player: " + GameObject.Find("Network").GetComponent<Network>().player1lives;
            GameObject.Find("Player2Lives").GetComponent<UnityEngine.UI.Text>().text = "Red Player:    " + GameObject.Find("Network").GetComponent<Network>().player2lives;
            GameObject.Find("Player3Lives").GetComponent<UnityEngine.UI.Text>().text = "Blue Player:    " + GameObject.Find("Network").GetComponent<Network>().player3lives;
            GameObject.Find("Player4Lives").GetComponent<UnityEngine.UI.Text>().text = "Yellow Player: " + GameObject.Find("Network").GetComponent<Network>().player4lives;
        }
        else if (GameObject.Find("Player3") != null || GameObject.Find("Network").GetComponent<Network>().player3lives != 3) {
            GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "White Player: " + GameObject.Find("Network").GetComponent<Network>().player1lives;
            GameObject.Find("Player2Lives").GetComponent<UnityEngine.UI.Text>().text = "Red Player:    " + GameObject.Find("Network").GetComponent<Network>().player2lives;
            GameObject.Find("Player3Lives").GetComponent<UnityEngine.UI.Text>().text = "Blue Player:    " + GameObject.Find("Network").GetComponent<Network>().player3lives;
        } else if (GameObject.Find("Player2") != null || GameObject.Find("Network").GetComponent<Network>().player2lives != 3) {
            GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "White Player: " + GameObject.Find("Network").GetComponent<Network>().player1lives;
            GameObject.Find("Player2Lives").GetComponent<UnityEngine.UI.Text>().text = "Red Player:    " + GameObject.Find("Network").GetComponent<Network>().player2lives;
        } else {
            GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "White Player: " + GameObject.Find("Network").GetComponent<Network>().player1lives;
        }

    }
}
