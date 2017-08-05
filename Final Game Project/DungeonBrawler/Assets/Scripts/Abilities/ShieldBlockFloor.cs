using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlockFloor : MonoBehaviour {

    private string itemClass;

    void Start()
    {
        itemClass = "Warrior";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.name == itemClass)
        {
            collision.GetComponent<PlayerController>().ability1 = Resources.Load<GameObject>("Prefabs/Items/Warrior/BasicShield");
            Destroy(this.gameObject);
        }
    }
}
