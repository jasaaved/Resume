using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BombermanCollision : MonoBehaviour {

	///////////////////////////////////////////
	// MEMBERS
	///////////////////////////////////////////
	public static bool bomberManDied = false;


	///////////////////////////////////////////
	// COLLISION
	///////////////////////////////////////////
	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.name.Contains("Worm")){
			CheckWinCondition.numberOfWorms = 4;
			bomberManDied = true;
			SceneManager.LoadScene ("MainScene");
		}

	}

}
