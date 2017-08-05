using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllBossDoors : MonoBehaviour {

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        //Enable enemies in a room as soon as the player enters a room
        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)") //shield throw hotfix
        {
            Destroy(this.gameObject);
        }
    }
}
