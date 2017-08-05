using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyColor : MonoBehaviour {
    private Image key;
    private GameObject gamemanager;

	// Use this for initialization
	void Start () {
        gamemanager = GameObject.Find("GameManager");
        key = this.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.name == "GreenKey" && gamemanager.GetComponent<GameManager>().key_found)
        {
            Color c = key.color;
            c.a = 255;
            key.color = c;
        }

        if (transform.name == "RedKey" && gamemanager.GetComponent<GameManager>().DK_key_found)
        {
            Color c = key.color;
            c.a = 255;
            key.color = c;
        }

    }
}
