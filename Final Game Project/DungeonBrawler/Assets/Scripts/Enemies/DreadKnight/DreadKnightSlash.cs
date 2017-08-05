using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadKnightSlash : MonoBehaviour {

    public float damage;

    private void Start()
    {
        Destroy(this.gameObject, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CombatInteraction>().damage = damage;
        }
    }
}