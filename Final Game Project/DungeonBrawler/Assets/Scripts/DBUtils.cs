using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBUtils : MonoBehaviour {

    // Since rigidbodies can sometimes be nested in the child of an object,
    // this makes it possible to get the rigidbody that may be nested in a child
    public static Rigidbody2D GetRigidbodyOfObject(GameObject obj)
    {
        Rigidbody2D rigidbody = null;

        // Check in object
        if (obj.GetComponent<Rigidbody2D>() != null)
        {
            rigidbody = obj.GetComponent<Rigidbody2D>();
        }
        // Check in children of object
        else if (obj.GetComponentInChildren<Rigidbody2D>() != null)
        {
            rigidbody = obj.GetComponentInChildren<Rigidbody2D>();
        }
        // Check in parent of object
        else
        {
            if (obj.transform.parent != null)
            {
                rigidbody = obj.transform.parent.GetComponent<Rigidbody2D>();
            }
        }

        return rigidbody;
    }
    // Since non-trigger colliders can sometimes be nested in the child of an object,
    // this makes it possible to get the non-trigger collider that may be nested in a child
    public static Collider2D GetNonTriggerColliderOfObject(GameObject obj)
    {
        if (!obj.GetComponent<Collider2D>().isTrigger)
        {
            return obj.GetComponent<Collider2D>();
        }
        else
        {
            foreach (Collider2D coll in obj.GetComponentsInChildren<Collider2D>())
            {
                if (!coll.isTrigger)
                {
                    return coll;
                }
            }
        }

        return null;
    }
    public static CombatInteraction GetCombatInteractionOfObject(GameObject obj)
    {
        CombatInteraction combatInteraction = null;

        // Check in object
        if (obj.GetComponent<Rigidbody2D>() != null)
        {
            combatInteraction = obj.GetComponent<CombatInteraction>();
        }
        // Check in children of object
        else if (obj.GetComponentInChildren<CombatInteraction>() != null)
        {
            combatInteraction = obj.GetComponentInChildren<CombatInteraction>();
        }
        // Check in parent of object
        else
        {
            if (obj.transform.parent != null)
            {
                combatInteraction = obj.transform.parent.GetComponent<CombatInteraction>();
            }
        }

        return combatInteraction;
    }

}
