﻿using UnityEngine;
using System.Collections;

public class playerscript : MonoBehaviour {
	static public bool playeridle3= true;
	static public bool playerattack3 = false;
	static public bool playerhit3 = false;
	static public bool dead = false;

	// Use this for initialization
	void Start () {
		playeridle3= true;
		playerattack3 = false;
		playerhit3 = false;
		dead = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (playeridle3 == true){
				GetComponent<Animation>().Play("Idle");
			}
		if (playerattack3 == true){
			GetComponent<Animation>()["attack"].wrapMode = WrapMode.Once;
			GetComponent<Animation>().Play("attack");
			playerattack3 = false;
		}
		if (playerhit3 == true){
			if (saltenemy.saltidle == true){
				GetComponent<Animation>()["hit"].wrapMode = WrapMode.Once;
				GetComponent<Animation>().Play("hit");
				BattleMenuScript.playerHP3 = BattleMenuScript.playerHP3 - 1000;
				playerhit3 = false;
			}
		}
		if (GetComponent<Animation>().IsPlaying("attack") == false){
			if (GetComponent<Animation>().IsPlaying("hit") == false){
				if (playerhit3 == false){
					BattleMenuScript.inmotion = false;
					if (BattleMenuScript.playerHP3 <= 0){
						Destroy(this.gameObject);
						dead = true;
					}
					if (BattleMenuScript.playerHP3 > 0){
					playeridle3 = true;
					}
				
			}
		}
	}

}
}


