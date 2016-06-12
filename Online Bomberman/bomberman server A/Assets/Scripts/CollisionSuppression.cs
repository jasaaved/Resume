using UnityEngine;
using System.Collections;

public class CollisionSuppression : MonoBehaviour {
	
	// Update is called once per frame
	void OnTriggerExit2D (Collider2D Player) {
        Collider2D Bomb = (Collider2D)gameObject.GetComponent("BoxCollider2D");
        Bomb.isTrigger = false;
	}
}
