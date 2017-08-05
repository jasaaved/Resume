using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public GameObject enemyType;
    private float enemyTimer;
    private float enemyCounter;
    public float maxEnemyCounter;
    public float SpawnRange;
    private CombatInteraction m_CombatInteraction;
    private EnemyStats m_EnemyStats;
    private Color m_MainColor;

    private void Start()
    {
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_EnemyStats = GetComponent<EnemyStats>();
        m_MainColor = GetComponent<SpriteRenderer>().color;
        enemyTimer = 5f;
        enemyCounter = 0f;
    }

    private void Update()
    {
        enemyCounter = transform.childCount;
        if(m_EnemyStats.health <= 0)
        {
            if (transform.childCount > 0)
            {
                for (int i = transform.childCount; i > 0; i--)
                {
                    Transform child = transform.GetChild(i-1);
                    child.parent = null;
                }
            }
            Destroy(this.gameObject);
        }


        if(enemyTimer > 0)
        {
            enemyTimer -= Time.deltaTime;
        }
        else if(enemyCounter < maxEnemyCounter)
        {
            SpawnEnemy();
        }
        CombatInteractionHandler();
    }

    void SpawnEnemy()
    {
        GameObject.Instantiate(enemyType, EnemyPlacement(), this.transform.rotation, this.transform);
        enemyTimer = 3f;
    }

    Vector3 EnemyPlacement()
    {
        float radian = Random.Range(-Mathf.PI, Mathf.PI);
        float rand = Random.value;
        float xdirection = Mathf.Cos(radian);
        float ydirection = Mathf.Sin(radian);
        float radius = rand * SpawnRange;

        float x = this.transform.position.x + (xdirection * radius);
        float y = this.transform.position.y + (ydirection * radius);

        Vector3 result = new Vector3(x, y, 0);
        return result;
    }

    private void CombatInteractionHandler()
    {
        if (m_CombatInteraction.damage > 0)
        {
            // Deal the damage
            m_EnemyStats.health -= m_CombatInteraction.damage;

            // Show damage has been taken
            StartCoroutine(DisplayDamageTaken());

            // ALWAYS RESET VALUES
            m_CombatInteraction.damage = 0;
            m_CombatInteraction.knockback = 0;
        }
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", Color.red);

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.material.SetColor("_Color", m_MainColor);
        }
    }
}
