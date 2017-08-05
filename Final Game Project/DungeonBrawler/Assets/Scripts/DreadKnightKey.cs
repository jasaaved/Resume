using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadKnightKey : MonoBehaviour
{
    private GameObject gamemanager;

    void Start()
    {
        gamemanager = GameObject.Find("GameManager").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Enable enemies in a room as soon as the player enters a room
        if (other.gameObject.tag == "Player") //shield throw hotfix
        {
            gamemanager.GetComponent<GameManager>().DK_key_found = true;
            Destroy(this.gameObject);
        }
    }
}
