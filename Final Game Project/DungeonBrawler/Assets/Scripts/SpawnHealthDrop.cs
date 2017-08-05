using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHealthDrop : MonoBehaviour {

    private GameObject healthDrop;
    private bool Spawned;

    private void Start()
    {
        Spawned = false;
        healthDrop = Resources.Load<GameObject>("Prefabs/Heart");
    }

    public void SpawnDrop()
    {
        if (!Spawned)
        {
            Spawned = true;
            float spawnChance = Random.Range(0, 100);
            Quaternion straightRotation = Quaternion.Euler(0, 0, 0);
            if (spawnChance <= 29f)
            {
                if (healthDrop != null)
                {
                    GameObject.Instantiate(healthDrop, transform.position, straightRotation);
                }
            }
        }
    }
/*
    private void OnDestroy()
    {
        float spawnChance = Random.Range(0, 100);
        Quaternion straightRotation = Quaternion.Euler(0, 0, 0);
        if (spawnChance <= 29f)
        {
            if (healthDrop != null)
            {
                GameObject.Instantiate(healthDrop, transform.position, straightRotation);
            }
        }
    }
*/
}
