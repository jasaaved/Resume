using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {
    private GameObject gamemanger;

	// Use this for initialization
	void Start () {
        gamemanger = GameObject.Find("GameManager").gameObject;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gamemanger.GetComponent<GameManager>().key_found = true;
            Destroy(this.gameObject);
        }
    }
}
