using UnityEngine;
using System.Collections;

public class BombDrop : MonoBehaviour {

	///////////////////////////////////////////
	// MEMBERS
	///////////////////////////////////////////
	public GameObject bombPrefab;


	///////////////////////////////////////////
	// LIFE CYCLE
	///////////////////////////////////////////
	void Update () {
		getInput ();
	}


	///////////////////////////////////////////
	// METHODS
	///////////////////////////////////////////
	private void getInput(){
        Vector2 pos;
		if(Input.GetKeyDown(KeyCode.Space)){

            if (GameObject.Find("Main Camera").GetComponent<StWindow>().player_assignment == 1)
            {
                pos = GameObject.Find("Player1").transform.position;
                GameObject.Find("Player1").GetComponent<Player1Move>().player_info.bomb = true;
            }

            else if (GameObject.Find("Main Camera").GetComponent<StWindow>().player_assignment == 2)
            {
                pos = GameObject.Find("Player2").transform.position;
                GameObject.Find("Player2").GetComponent<Player2Move>().player_info.bomb = true;

            }

            else if (GameObject.Find("Main Camera").GetComponent<StWindow>().player_assignment == 3)
            {
                pos = GameObject.Find("Player3").transform.position;
                GameObject.Find("Player3").GetComponent<Player3Move>().player_info.bomb = true;

            }
            else {
                pos = GameObject.Find("Player4").transform.position;
                GameObject.Find("Player4").GetComponent<Player4Move>().player_info.bomb = true;

            }

            pos.x = Mathf.Round (pos.x);
			pos.y = Mathf.Round (pos.y);

            Instantiate (bombPrefab, pos, Quaternion.identity);
		}

	}

}
