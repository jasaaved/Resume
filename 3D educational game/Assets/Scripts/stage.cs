using UnityEngine;
using System.Collections;

public class stage : MonoBehaviour {
	public Texture stagetext;
	public Material stagemat;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material = stagemat;
		GetComponent<Renderer>().material.mainTexture = stagetext;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
