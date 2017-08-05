using UnityEngine;
using System.Collections;

public class KnightAggroTrigger : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public GameObject targetObject;
    public bool isAggroed;
    private int layermask = ~(1 << 12);


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start ()
    {
        isAggroed = false;
	}
    void Update ()
    {
        if (targetObject != null && targetObject.tag == "Dead")
        {
            targetObject = null;
            isAggroed = false;
        }
        if (targetObject != null && targetObject.GetComponent<PlayerController>().isDead)
        {
            targetObject = null;
            isAggroed = false;
        }

        this.transform.position = this.transform.parent.position;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (targetObject == null)
            {
                targetObject = other.gameObject;
                isAggroed = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (targetObject == null)
            {
                targetObject = other.gameObject;
                isAggroed = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            targetObject = null;
            isAggroed = false; 
        }
    }
}
