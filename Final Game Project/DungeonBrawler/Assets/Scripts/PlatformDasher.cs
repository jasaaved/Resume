using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDasher : MonoBehaviour {

    private Rigidbody2D body;
    private RaycastHit2D scan;
    private float idleVelocity;
    private float berserkVelocity;
    private bool movingRight;
    private bool berserk;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        idleVelocity = 2f;
        berserkVelocity = 10f;
        movingRight = true;
        berserk = false;
    }

    private void Update()
    {
        if(!Scan4Floor() && movingRight)
        {
            movingRight = false;
        }
        else if(!Scan4Floor() && !movingRight)
        {
            movingRight = true;
        }
        berserk = Berserk();
        Move();
    }

    bool Scan4Floor()
    {
        if(movingRight)
        {
            Vector3 inFront = new Vector3(0.5f, 0, 0);
            scan = Physics2D.Raycast(transform.position + inFront, Vector2.down, 2f);
        }
        else
        {
            Vector3 inFront = new Vector3(-0.5f, 0, 0);
            scan = Physics2D.Raycast(transform.position + inFront, Vector2.down, 2f);
        }
        
        if(scan)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move()
    {
        if (movingRight && !berserk)
        {
            body.velocity = Vector2.right * idleVelocity;
        }
        else if (movingRight && berserk)
        {
            body.velocity = Vector2.right * berserkVelocity;
        }
        else if (!movingRight && !berserk)
        {
            body.velocity = Vector2.left * idleVelocity;
        }
        else if (!movingRight && berserk)
        {
            body.velocity = Vector2.left * berserkVelocity;
        }
    }

    bool Berserk()
    {
        float lowerBound = transform.position.y;
        float upperBound = transform.position.y + 1f;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.name == "Warrior" || player.name == "Wizard" || player.name == "Rogue")
            {
                float playerY = player.transform.position.y;
                if(playerY <= upperBound && playerY >= lowerBound)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
