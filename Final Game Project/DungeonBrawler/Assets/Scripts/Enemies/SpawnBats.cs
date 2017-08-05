using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBats : MonoBehaviour {
    public float spawnTimer = 7f;
    private GameObject warrior;
    private GameObject rogue;
    private GameObject wizard;
    private GameObject doorLeft;
    private float SpawnTime;
    public bool spawning;
    private GameObject _parent;
    public GameObject bat_child;
    private float m_SpawnX;
    private float m_SpawnY;
    private float m_SpawnZ;
    private Vector3 m_PatrolCycle;
    public int amount_Spawning_bat = 5;

    void Start()
    {
        spawning = false;
        warrior = GameObject.Find("Warrior");
        rogue = GameObject.Find("Rogue");
        wizard = GameObject.Find("Wizard");
        doorLeft = GameObject.Find("DoorLeft");
        SpawnTime = Time.time;
        _parent = transform.parent.gameObject;
        m_SpawnX = transform.position.x;
        m_SpawnY = transform.position.y;
        m_SpawnZ = transform.position.z;
        m_PatrolCycle = new Vector3(m_SpawnX, m_SpawnY, m_SpawnZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer > 0 && !spawning)
        {
            spawnTimer -= Time.deltaTime;
        }
        if (spawnTimer <= 0)
        {
            if (!spawning)
            {
                SpawnTime = 1f;
            }
            spawning = true;

            if (SpawnTime >= 1f && this.transform.parent.transform.parent.transform.childCount < amount_Spawning_bat && Vector2.Distance(this.transform.position, m_PatrolCycle) <= 2)
            {
                //change this if enemy folder is not used with boss bat
                GameObject bat = Instantiate(bat_child, this.transform.parent.transform.parent.transform);

                float rand = Random.value;

                if (rand <= 0.25)
                {
                    bat.transform.position = new Vector3(transform.position.x + 5, transform.position.y + 5, transform.position.z);
                }
                else if (rand > 0.25 && rand <= 0.5)
                {
                    bat.transform.position = new Vector3(transform.position.x + 5, transform.position.y - 5, transform.position.z);
                }
                else if (rand > 0.5 && rand <= 0.75)
                {
                    bat.transform.position = new Vector3(transform.position.x - 5, transform.position.y + 5, transform.position.z);
                }
                else
                {
                    bat.transform.position = new Vector3(transform.position.x - 5, transform.position.y - 5, transform.position.z);
                }

                SpawnTime = 0f;

            }

            SpawnTime += Time.deltaTime;

            if (this.transform.parent.transform.parent.transform.childCount >= amount_Spawning_bat)
            {
                spawnTimer = 7f;
                spawning = false;
               
            }
        }


    

        //gameObject.GetComponent<BossBatBehavior>().stop = true;
    }

    void OnDestroy()
    {
        if (warrior != null) warrior.GetComponent<PlayerController>().ResetPlayer();
        if (rogue != null) rogue.GetComponent<PlayerController>().ResetPlayer();
        if (wizard != null) wizard.GetComponent<PlayerController>().ResetPlayer();
    }
}
