using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadKnightHammer : MonoBehaviour {

    private Vector3 startPosition;
    private float angle;
    private float aliveTimer;
    public float damage;
    private GameObject dreadKnight;
    private Vector3[] dreadKnightTeleport = new Vector3[4];
    private Vector3 dreadKnightSpawn;

    private void Start()
    {
        startPosition = GameObject.Find("DreadKnight").GetComponent<DreadKnightBehaviour>().newPosition;
        dreadKnight = GameObject.Find("DreadKnight");
        dreadKnightSpawn = dreadKnight.GetComponent<DreadKnightBehaviour>().spawnPosition;
        PopulateTeleportPositions();
        transform.position = startPosition;
        angle = GetStartAngle();
        aliveTimer = 0;     
        //Destroy(this.gameObject, 3.2f);
    }

    private void Update()
    {
        Travel();
        Rotate();
        angle += Time.deltaTime * 2;
        aliveTimer += Time.deltaTime;
        if (InRange() && aliveTimer >= 1f)
        {
            Destroy(this.gameObject);
        }

    }

    private void Travel()
    {
        float newX = Mathf.Cos(angle) * 13;
        float newY = Mathf.Sin(angle) * 6;
        transform.position = new Vector3(newX, newY + 1, 0) + dreadKnightSpawn;
    }
    private float GetStartAngle()
    {
        if (startPosition == dreadKnightTeleport[0])
        {
            return 0f * Mathf.Deg2Rad;
        }
        else if(startPosition == dreadKnightTeleport[1])
        {
            return 180f * Mathf.Deg2Rad;
        }
        else if(startPosition == dreadKnightTeleport[2])
        {
            return 270f * Mathf.Deg2Rad;
        }
        else if(startPosition == dreadKnightTeleport[3])
        {
            return 90f * Mathf.Deg2Rad;
        }
        print("Finished Getting Start Angle");
        return 0.5f; //Just a random value.
    }
    private void Rotate()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.z += 50f;
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CombatInteraction>().damage = damage;
        }
    }
    private bool InRange()
    {
        if(Vector3.Distance(transform.position, dreadKnight.transform.position) <= 2f)
        {
            return true;
        }
        return false;
    }
    private void PopulateTeleportPositions()
    {
        dreadKnightTeleport[0] = dreadKnight.GetComponent<DreadKnightBehaviour>().teleportPositions[0];
        dreadKnightTeleport[1] = dreadKnight.GetComponent<DreadKnightBehaviour>().teleportPositions[1];
        dreadKnightTeleport[2] = dreadKnight.GetComponent<DreadKnightBehaviour>().teleportPositions[2];
        dreadKnightTeleport[3] = dreadKnight.GetComponent<DreadKnightBehaviour>().teleportPositions[3];
    }
}
