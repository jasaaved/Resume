using UnityEngine;
using System.Collections;

public class behavior : MonoBehaviour {
	public int hi = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hi==3){
			GetComponent<Animation>().Play ("hit");
	
	}
}
}
