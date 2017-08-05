using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHazard : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CombatInteraction>().damage = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Slime(Clone)")
        {
            collision.GetComponent<CombatInteraction>().damage = 2f;
        }
    }

}
