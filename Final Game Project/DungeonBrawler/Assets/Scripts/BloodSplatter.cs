using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour {

    private GameObject[] players;

    private void Start()
    {
        Destroy(this.gameObject, 1.5f);
        if (FindClosestTarget() != null)
        {
            if (transform.position.x < FindClosestTarget().transform.position.x)
            {
                transform.rotation = Quaternion.Euler(-135, 90, -90);
            }
            else if (transform.position.x > FindClosestTarget().transform.position.x)
            {
                transform.rotation = Quaternion.Euler(-45, 90, -90);
            }
        }
    }

    GameObject FindClosestTarget()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            return null;
        }

        GameObject closest = players[0];
        float shortestDistance = Vector2.Distance(this.gameObject.transform.position, closest.transform.position);
        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(this.gameObject.transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = player;
            }
        }
        return closest;
    }
}
