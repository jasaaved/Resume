using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAssBoss : MonoBehaviour {

    private CombatInteraction m_CombatInteraction;
    private EnemyStats m_EnemyStats;
    private Color flashColor;
    private Color m_MainColor;
    private Vector3 targetPosition;
    private Vector3 spawnPosition;

    public GameObject AcidBullet;
    public GameObject Bat;
    public GameObject AcidLob;

    public bool TestAcidShot;
    public bool TestSpawnBats;
    public bool TestAcidLob;

    private float moveTimer;
    private float attackTimer;


    private void Start()
    {
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_EnemyStats = GetComponent<EnemyStats>();
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        flashColor = Color.red;
        targetPosition = transform.position;
        spawnPosition = transform.position;

        moveTimer = 3f;
        attackTimer = 4f;
    }

    private void Update()
    {
        if(m_EnemyStats.health <= 0)
        {
            Destroy(this.gameObject);
        }
        Act();
        TestMoves();
        moveTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        CombatInteractionHandler();
        DisplayDamageTaken();
    }


    ////////////////////////
    /// Bat Moves
    ////////////////////////
    private void Move()
    {
        GameObject target = FindClosestTarget();
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.025f);
    }
    private void FireAcidBullets()
    {
        for(int i = 0; i < 10; i++)
        {
            int angle = Random.Range(0, 360);
            Quaternion rotate = Quaternion.Euler(0, 0, angle);
            GameObject.Instantiate(AcidBullet, transform.position, rotate);
        }
    }
    private void SpawnBats()
    {
        for(int i = 0; i < 5; i++)
        {
            float x = GetRandomX();
            float y = GetRandomY();
            Vector3 spawn = new Vector3(x, y, 0);
            GameObject.Instantiate(Bat, spawn, transform.rotation, transform.parent.transform);
        }
    }
    private void LobAcid()
    {
        Quaternion rotate1 = Quaternion.Euler(0, 0, 60);
        Quaternion rotate2 = Quaternion.Euler(0, 0, 120);
        GameObject.Instantiate(AcidLob, transform.position, rotate1);
        GameObject.Instantiate(AcidLob, transform.position, rotate2);
    }
    private void ChargePlayer()
    {
        if(InRange(targetPosition, 0.25f))
        {
            GameObject target = FindFurthestTarget();
            if(target != null)
            {
                targetPosition = target.transform.position;
            }
            else
            {
                targetPosition = new Vector3(GetRandomX(), GetRandomY(), 0);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.01f);
            if(InRange(targetPosition, 0.25f))
            {
                moveTimer = 2f;
            }
        }
    }

    ////////////////////////
    /// Helper Functions
    ////////////////////////
    private GameObject FindClosestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

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
    private GameObject FindFurthestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            return null;
        }

        GameObject furthest = players[0];
        float furthestDistance = Vector2.Distance(this.gameObject.transform.position, furthest.transform.position);
        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(this.gameObject.transform.position, player.transform.position);
            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthest = player;
            }
        }
        return furthest;
    }
    private bool BatsOnScreen()
    {
        GameObject batTest = GameObject.Find("Bat(Clone)");
        if(batTest != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool InRange(Vector3 newPosition, float minimum)
    {
        if (Vector3.Distance(transform.position, newPosition) <= minimum)
        {
            return true;
        }
        return false;
    }
    private void CombatInteractionHandler()
    {
        if (m_CombatInteraction.damage > 0)
        {
            // Deal the damage
            m_EnemyStats.health -= m_CombatInteraction.damage;
            GameObject blood = Resources.Load<GameObject>("Prefabs/BloodSplatter");
            GameObject.Instantiate(blood, transform.position, transform.rotation);
            // Show damage has been taken
            StartCoroutine(DisplayDamageTaken());
            flashColor = Color.red;
        }

        // ALWAYS RESET VALUES
        m_CombatInteraction.damage = 0;
        m_CombatInteraction.knockback = 0;
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", flashColor);

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.material.SetColor("_Color", m_MainColor);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CombatInteraction>().damage = 3f;
        }
    }

    ////////////////////////
    /// AI Related Functions
    ////////////////////////
    private void Act()
    {
        if (moveTimer <= 0)
        {
            ChargePlayer();
        }
        if(attackTimer <= 0)
        {
            CastRandomAttack();
            attackTimer = 3f;
        }
    }
    private void CastRandomAttack()
    {
        int attack = Random.Range(0, 3);
        if(attack == 0)
        {
            TestAcidShot = true;
        }
        else if(attack == 1)
        {
            TestAcidLob = true;
        }
        else if(attack == 2)
        {
            if(!BatsOnScreen())
            {
                TestSpawnBats = true;
            }
        }
    }
    private float GetRandomX()
    {
        return Random.Range(-17f, 17f) + spawnPosition.x;
    }
    private float GetRandomY()
    {
        return Random.Range(-6f, 7f) + spawnPosition.y;
    }
    private void TestMoves()
    {
        if(TestAcidShot)
        {
            FireAcidBullets();
            TestAcidShot = false;
        }
        if(TestSpawnBats)
        {
            SpawnBats();
            TestSpawnBats = false;
        }
        if(TestAcidLob)
        {
            LobAcid();
            TestAcidLob = false;
        }
    }
}
