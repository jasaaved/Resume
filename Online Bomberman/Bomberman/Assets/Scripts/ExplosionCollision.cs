using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExplosionCollision : MonoBehaviour {

	///////////////////////////////////////////
	// COLLISION
	///////////////////////////////////////////
	void OnTriggerEnter2D(Collider2D collider) {
		if(!collider.gameObject.isStatic){
			if(collider.gameObject.name.Contains("Explosion")){
				return;
			}

            else if (collider.gameObject.name == "Player1" || collider.gameObject.name == "Player2" ||
                collider.gameObject.name == "Player3" || collider.gameObject.name == "Player4"){
                LifeManager lm = (LifeManager)GameObject.Find(collider.gameObject.name).GetComponent(typeof(LifeManager));
                lm.Respawn();
            }

            else if (collider.gameObject.name.Contains("block_undestroyable")){
                return;
            }

            else{
                Destroy(collider.gameObject);
            }
        }
	}
}
