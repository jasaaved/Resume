using UnityEngine;
using System.Collections;

public class BombermanMovement : MonoBehaviour {

	///////////////////////////////////////////
	// MEMBERS
	///////////////////////////////////////////
	public float speed = 6;


	///////////////////////////////////////////
	// LIFE CYCLE
	///////////////////////////////////////////
	void FixedUpdate () {
		getInput ();
	}


	///////////////////////////////////////////
	// METHODS
	///////////////////////////////////////////
	private void getInput(){
		// Check input axes
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		// Implement velocity
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (h, v) * speed;

		// Set animation parameters
		GetComponent<Animator>().SetInteger("X", (int)h);
		GetComponent<Animator>().SetInteger("Y", (int)v);

	}

}