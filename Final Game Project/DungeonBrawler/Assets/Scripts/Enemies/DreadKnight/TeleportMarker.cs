using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMarker : MonoBehaviour {

    private GameObject dreadKnight;
    private Vector3 targetPosition;
    private DreadKnightBehaviour m_dreadKnightBehavior;

    private void Start()
    {
        dreadKnight = GameObject.Find("DreadKnight");
        targetPosition = GameObject.Find("DreadKnight").GetComponent<DreadKnightBehaviour>().newPosition;
        m_dreadKnightBehavior = GameObject.Find("DreadKnight").GetComponent<DreadKnightBehaviour>();
    }

    private void Update()
    {
        if (!m_dreadKnightBehavior.isDead)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
            if (InRange())
            {
                dreadKnight.GetComponent<DreadKnightBehaviour>().repositioning = true;
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private bool InRange()
    {
        if(Vector3.Distance(transform.position, targetPosition) <= 0.25f)
        {
            return true;
        }
        return false;
    }
}
