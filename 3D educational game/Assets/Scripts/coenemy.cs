using UnityEngine;
using System.Collections;

public class coenemy : MonoBehaviour {
		static public bool saltidle= true;
		static public bool saltattack = false;
		static public bool salthit = false;
		static public bool dead = false;

		// Use this for initialization
		void Start () {
			saltidle= true;
			saltattack = false;
			salthit = false;
			dead = false;
		}
		
		// Update is called once per frame
		void Update () {
			if (saltidle == true){
				GetComponent<Animation>().Play("idle");
			}
			if (saltattack == true){
				GetComponent<Animation>()["attack"].wrapMode = WrapMode.Once;
				GetComponent<Animation>().Play("attack");
				saltattack = false;
			}
			if (salthit == true){
				if (Playerscript2.playeridle3 == true){ //change
					GetComponent<Animation>()["hit"].wrapMode = WrapMode.Once;
					GetComponent<Animation>().Play("hit");
					BattleMenuScript2.monsterHP3 = BattleMenuScript2.monsterHP3 - 1000;
					salthit = false;
				}
			}
			
			if (GetComponent<Animation>().IsPlaying("attack") == false){
				if (GetComponent<Animation>().IsPlaying("hit") == false){
					if (salthit == false){
						BattleMenuScript2.inmotion = false;
						if (BattleMenuScript2.monsterHP3 <= 0){
							Destroy(this.gameObject);
							dead = true;
						}
						if (BattleMenuScript2.monsterHP3 > 0){
							saltidle = true;
						}
					}
				}
			}
			
		}
	}