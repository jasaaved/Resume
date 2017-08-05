using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireConeFloor : MonoBehaviour {

    private string itemClass;


    void Start()
    {
        itemClass = "Wizard";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.name == itemClass)
        {
            collision.GetComponent<PlayerController>().ability1 = Resources.Load<GameObject>("Prefabs/Items/Wizard/BasicFireCone");
            Destroy(this.gameObject);
        }
    }
}
