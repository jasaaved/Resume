using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckWinCondition : MonoBehaviour {

	///////////////////////////////////////////
	// MEMBERS
	///////////////////////////////////////////
	public static int numberOfWorms = 4;


	///////////////////////////////////////////
	// LIFE CYCLE
	///////////////////////////////////////////
	void Update () {
		if(numberOfWorms == 0){
			SceneManager.LoadScene ("WinScreen");
		}
	
	}

}
