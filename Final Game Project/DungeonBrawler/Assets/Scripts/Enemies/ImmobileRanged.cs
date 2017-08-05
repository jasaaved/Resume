using UnityEngine;
using System.Collections;

public class ImmobileRanged : MonoBehaviour {

    public GameObject goblinSprite;

    private GameObject target;
    public GameObject projectile;
    private float attackTimer;
    private EnemyStats m_EnemyStats;
    private float m_PreviousHealth;
    private Color m_MainColor;
    private GameObject[] players;
    private CombatInteraction m_CombatInteraction;

    private float alpha;
    private Color color;
    private float currentRemainTime;
    public float fadeTime = 2f;
    private SpriteRenderer m_Renderer;
    private bool isDead;

    void Start()
    {
        target = FindClosestTarget();
        attackTimer = 1f;
        m_EnemyStats = this.GetComponent<EnemyStats>();
        m_PreviousHealth = m_EnemyStats.health;
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        players = GameObject.FindGameObjectsWithTag("Player");
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_CombatInteraction.immuneToCC = true;
        m_Renderer = GetComponent<SpriteRenderer>();
        isDead = false;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            target = FindClosestTarget();
        }
    }

    void Update()
    {
        if (!isDead)
        {
            CombatInteractionHandler();
            Aim();
            if (m_EnemyStats.health <= 0)
            {
                Destroy(this.gameObject);
            }

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                Fire();
            }
        }
        else
        {
            GetComponent<SpawnHealthDrop>().SpawnDrop();
            fadingOut();
        }

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

        SpriteRenderer goblinSpriteRenderer = GetComponent<SpriteRenderer>();
        goblinSpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.material.SetColor("_Color", m_MainColor);
            goblinSpriteRenderer.color = goblinSprite.GetComponent<GoblinSprite>().mainColor;
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
        foreach(GameObject player in players)
        {
            float distance = Vector2.Distance(this.gameObject.transform.position, player.transform.position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = player;
            }
        }
        return closest;
    }

    void Aim()
    {
        if(target == null)
        {
            return;
        }

        float y = target.transform.position.y - transform.position.y;
        float x = target.transform.position.x - transform.position.x;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void Fire()
    {
        GameObject.Instantiate(projectile, this.transform.position, this.transform.rotation);
        attackTimer = m_EnemyStats.attackSpeed;
    }

    private void fadingOut()
    {
        //m_Rigidbody2D.velocity = Vector2.zero;

        currentRemainTime -= Time.deltaTime;

        if (currentRemainTime <= 0f)
        {
            Destroy(goblinSprite);
            Destroy(this.gameObject);
            return;
        }

        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;

        Color goblinColor = gameObject.GetComponent<SpriteRenderer>().color;
        goblinColor.a = alpha;
        gameObject.GetComponent<SpriteRenderer>().color = goblinColor;
    }

}
