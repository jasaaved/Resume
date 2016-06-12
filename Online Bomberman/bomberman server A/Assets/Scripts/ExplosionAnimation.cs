using UnityEngine;
using System.Collections;

public class ExplosionAnimation : MonoBehaviour {

	///////////////////////////////////////////
	// MEMBERS
	///////////////////////////////////////////
	public GameObject explosionPrefab;


	///////////////////////////////////////////
	// LIFE CYCLE
	///////////////////////////////////////////
	void OnDestroy () {
		Instantiate (explosionPrefab, transform.position, Quaternion.identity);

	}

}
