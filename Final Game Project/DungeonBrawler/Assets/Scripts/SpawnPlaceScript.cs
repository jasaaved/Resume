using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaceScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Body")
        {
            other.GetComponent<SpawnBats>().spawning = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Body")
        {
            other.GetComponent<SpawnBats>().spawning = false;
        }
    }
}
