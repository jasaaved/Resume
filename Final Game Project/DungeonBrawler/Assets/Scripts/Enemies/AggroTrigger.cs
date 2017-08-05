using UnityEngine;
using System.Collections;

public class AggroTrigger : MonoBehaviour {

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

        this.transform.position = this.transform.parent.position;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, other.transform.position, layermask);
        if (other.tag == "Player")
        {
            if (targetObject == null && hit.transform.tag == "Player")
            {
                targetObject = other.gameObject;
                isAggroed = true;
            }

            if (hit.transform.tag != "Player")
            {
                targetObject = null;
                isAggroed = false;
            } 

        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        

        if (other.tag == "Player")
        {
            if (targetObject == null)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, other.transform.position, layermask);
                if (hit.transform == null || hit.transform.tag != "Player")
                {
                    targetObject = null;
                    isAggroed = false;
                }

                if (hit.transform != null && hit.transform.tag == "Player")
                {
                    targetObject = other.gameObject;
                    isAggroed = true;
                }
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
