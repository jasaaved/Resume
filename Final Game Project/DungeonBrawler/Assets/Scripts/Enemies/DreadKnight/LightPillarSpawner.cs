using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillarSpawner : MonoBehaviour {

    public GameObject lightPillar;
    private float spawnRate;
    private Vector3 DreadKnightSpawn;

    private void Start()
    {
        DreadKnightSpawn = GameObject.Find("DreadKnight").GetComponent<DreadKnightBehaviour>().spawnPosition;
        spawnRate = 2f;
    }

    private void Update()
    {
        if(spawnRate <= 0f)
        {
            GameObject.Instantiate(lightPillar, GetSpawnPoint(), transform.rotation);
            spawnRate = 1f;
        }
        spawnRate -= Time.deltaTime;
    }

    private Vector3 GetSpawnPoint()
    {
        float x = Random.Range(-23f, 23f);
        return new Vector3(x + DreadKnightSpawn.x, 0 + DreadKnightSpawn.y, 0);
    }

}
