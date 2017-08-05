using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDreadKnightKey : MonoBehaviour {
    private GameObject key;
    private GameObject enemies_folder;
    private bool did_already;

    // Use this for initialization
    void Start () {

        enemies_folder = transform.FindChild("Enemies").gameObject;

        Transform childKey = transform.FindChild("Key");

        if (childKey != null)
        {
            key = childKey.gameObject;
            key.SetActive(false);
        }

        did_already = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (enemies_folder.transform.childCount <= 0 && !did_already)
        {
            did_already = true;
            key.SetActive(true);
        }

    }
}
