using UnityEngine;
using System.Collections;

public class GoblinBehavior : MonoBehaviour
{
    // Update is called once per frame
    public GameObject Player;
    public int health = 10;
    public float dist;                  //Distance/"offset" from the preferred center of the patrol
    public int speed;
    public float patrolSize;
    private float startPos;
    private Rigidbody2D m_Rigidbody2D;
    private CombatInteraction m_CombatInteraction;


    void Start()
    {
        //Determines the "center" of the patrol based on current position and distance from center
        startPos = GetComponent<Rigidbody2D>().transform.position.x-dist;

        m_CombatInteraction = GetComponent<CombatInteraction>();

        //Set initial velocity
        if (dist < 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(-speed, 0, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (m_CombatInteraction.stunned)
        {
            return;
        }
        GobMove();
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        //print(health);
    }

    void OnTriggerEnter2D(Collider2D item)
    {
        if (item.tag == "Player")
        {
            item.transform.position = new Vector3(-14f, -6.83f, 0);
            //Destroy(player.gameObject);
        }

        else if (item.tag == "Attack")
        {
            health -= 5;
        }
    }

    void GobMove()
    {
        //Patrols the goblin
        dist = GetComponent<Rigidbody2D>().transform.position.x-startPos;
        if (dist>patrolSize/2)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(-speed, 0, 0);
        }
        if (dist < -(patrolSize/2))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
        }
    }
}
