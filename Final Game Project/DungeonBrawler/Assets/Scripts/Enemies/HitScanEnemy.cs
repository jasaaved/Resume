using UnityEngine;
using System.Collections;

public class HitScanEnemy : MonoBehaviour
{

    public GameObject goblinSprite;

    private GameObject target;
    //public GameObject projectile;
    private float attackTimer;
    private float attackDuration;
    private EnemyStats m_EnemyStats;
    private float m_PreviousHealth;
    private Color m_MainColor;
    private GameObject[] players;
    private CombatInteraction m_CombatInteraction;
    private RaycastHit2D hit;
    private bool attacking;

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
        attacking = false;
        attackDuration = 0f;
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
                isDead = true;
                currentRemainTime = fadeTime;
            }

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                Fire();
            }

            if (attackDuration > 0)
            {
                attackDuration -= Time.deltaTime;
            }
            else
            {
                attacking = false;
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

    void Aim()
    {
        if (target == null)
        {
            return;
        }

        hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position);

        float y = target.transform.position.y - transform.position.y;
        float x = target.transform.position.x - transform.position.x;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void Fire()
    {
        //GameObject.Instantiate(projectile, this.transform.position, this.transform.rotation);
        attacking = true;
        attackDuration = 0.3f;
        attackTimer = m_EnemyStats.attackSpeed;
        DrawLine(transform.position, hit.point, Color.red);
        if (hit.transform.tag == "Player" || hit.transform.tag == "Shield")
        {
            hit.transform.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
        }
    }

    private void OnDrawGizmos()
    {
        if(hit.collider != null && attacking)
        {
            LineRenderer lr = GetComponent<LineRenderer>();

            if (lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
            }

            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
            Destroy(lr, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        LineRenderer lr = GetComponent<LineRenderer>();

        if (lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }

        gameObject.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Destroy(lr, duration);
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
