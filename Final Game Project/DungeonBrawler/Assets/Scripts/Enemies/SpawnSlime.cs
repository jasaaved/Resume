using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlime : MonoBehaviour {
    public float spawnTimer = 5f;
    private float timer;
    private GameObject _parent;
    private float m_SpawnX;
    private float m_SpawnY;
    private float m_SpawnZ;
    public GameObject wimp_child;


    void Start()
    {
        _parent = transform.parent.gameObject;
        m_SpawnX = transform.position.x;
        m_SpawnY = transform.position.y;
        m_SpawnZ = transform.position.z;
        timer = spawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            GameObject wimp = Instantiate(wimp_child, this.transform);
            wimp.transform.position = new Vector3(m_SpawnX, m_SpawnY, m_SpawnZ);
            wimp.transform.parent = transform.parent.FindChild("Enemies").gameObject.transform;

            timer = spawnTimer;
        }

        //gameObject.GetComponent<BossBatBehavior>().stop = true;
    }

}
