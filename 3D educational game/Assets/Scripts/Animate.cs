using UnityEngine;
using System.Collections;

public class Animate : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Start () {
	anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Animation>().Play("idle");
	}
	
}
