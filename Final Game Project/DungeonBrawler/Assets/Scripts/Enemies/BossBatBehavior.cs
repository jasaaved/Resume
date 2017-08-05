using UnityEngine;
using System.Collections;

public class BossBatBehavior : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public bool clockwise = true;

    private float m_MoveTime = 0;
    private float m_AttackTime = 0;
    private float m_SpawnX;
    private float m_SpawnY;
    private float m_SpawnZ;
    private float m_PatrolRadius;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_PatrolCycle;
    private GameObject m_TargetObject;
    private EnemyStats m_EnemyStats;
    private Color m_MainColor;
    private GameObject m_Parent;
    private GameObject m_Zone;
    private AggroTrigger m_AggroTrigger;
    private CombatInteraction m_CombatInteraction;
    private bool m_ApplyKnockback;
    private bool spawning;
    private float m_KnockbackTime = 0;
    private float m_Knockback = 0;
    private float m_CurrentSpeed = 0;
    public bool stop;
    private GameObject anyobject;

    private float alpha;
    private Color color;
    private float currentRemainTime;
    public float fadeTime = 3f;
    private SpriteRenderer m_Renderer;
    private bool isDead;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start ()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Parent = gameObject.transform.parent.gameObject;
        m_Zone = gameObject.transform.parent.transform.GetChild(0).gameObject;
        m_AggroTrigger = gameObject.transform.parent.gameObject.GetComponentInChildren<AggroTrigger>();
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_CombatInteraction.immuneToCC = true;
        m_EnemyStats = GetComponent<EnemyStats>();
        m_SpawnX = transform.position.x;
        m_SpawnY = transform.position.y;
        m_SpawnZ = transform.position.z;
        m_PatrolCycle = new Vector3(m_SpawnX, m_SpawnY, m_SpawnZ);
        m_PatrolRadius = 1;
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        m_CurrentSpeed = m_EnemyStats.speed;
        m_Renderer = GetComponent<SpriteRenderer>();
        isDead = false;
    }
	void FixedUpdate ()
    {
        if (!isDead)
        {
            m_TargetObject = m_AggroTrigger.targetObject;

            CombatInteractionHandler();

            if (gameObject.GetComponent<EnemyStats>().health <= 0)
            {
                isDead = true;
                currentRemainTime = fadeTime;
            }

            if (m_CombatInteraction.stunned)
            {
                return;
            }

            m_Zone.transform.position = this.transform.position;
        }
        else
        {
            fadingOut();
        }
	}
    
    void Update()
    {
        if (!isDead)
        {
            spawning = this.gameObject.GetComponent<SpawnBats>().spawning;
            if (m_AggroTrigger.isAggroed && !spawning)
            {
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.material.SetColor("_Color", m_MainColor);
                Attack();
            }
            else if ((!m_AggroTrigger.isAggroed && (m_PatrolCycle != this.transform.position)) || spawning)
            {
                Return();
            }
            else
            {
                Move();
            }
            // Disable if off screen
            //someone removed this for some reason, I put it back -Jonathan (remove if unnecessary
            if (!GetComponent<Renderer>().isVisible || stop)
            {
                Return();
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            if (m_AttackTime == 0)
            {
                other.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            m_AttackTime += Time.deltaTime;

            if (m_AttackTime >= GetComponent<EnemyStats>().attackSpeed)
            {
                other.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
                m_AttackTime = 0;
            }
        }
    }


    /////////////////////////////////////////////////////////////////
    // METHODS
    /////////////////////////////////////////////////////////////////
    private void Move()
    {
        if(clockwise)
        {
            m_MoveTime -= Time.deltaTime * m_EnemyStats.speed;
        }
        else
        {
            m_MoveTime += Time.deltaTime * m_EnemyStats.speed;
        }

        float x = m_SpawnX + (Mathf.Cos(m_MoveTime) * m_PatrolRadius);
        float y = m_SpawnY + (Mathf.Sin(m_MoveTime) * m_PatrolRadius);

        float z = m_SpawnZ;
        m_PatrolCycle = new Vector3(x, y, z);
        this.transform.position = new Vector3(x, y, z);
    }
    private void Attack()
    {
        if(m_CurrentSpeed < m_EnemyStats.speed)
        {
            m_CurrentSpeed += 0.1f;
        }

        if (m_TargetObject != null)
        {
            if (this.transform.position.x > m_TargetObject.transform.position.x)
            {
                Vector2 target = new Vector2(m_TargetObject.transform.position.x + 0.5f, m_TargetObject.transform.position.y);
                this.transform.position = Vector2.MoveTowards(this.transform.position, target, m_CurrentSpeed * Time.deltaTime);
            }
            else
            {
                Vector2 target = new Vector2(m_TargetObject.transform.position.x - 0.5f, m_TargetObject.transform.position.y);
                this.transform.position = Vector2.MoveTowards(this.transform.position, target, m_CurrentSpeed * Time.deltaTime);
            }
        }
    }
    private void Return()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, m_PatrolCycle, 20 * Time.deltaTime);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (spawning == true)
        {
            renderer.material.SetColor("_Color", Color.yellow);
        }
        
    }
    private void CombatInteractionHandler()
    {
        if(m_CombatInteraction.damage > 0)
        {
            // Deal the damage
            m_EnemyStats.health -= m_CombatInteraction.damage;
            GameObject blood = Resources.Load<GameObject>("Prefabs/BloodSplatter");
            ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            var col = ps.colorOverLifetime;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            col.color = grad;
            GameObject.Instantiate(blood, transform.position, transform.rotation);
            // Show damage has been taken
            StartCoroutine(DisplayDamageTaken());
        }
        if(m_CombatInteraction.knockback > 0)
        {
            m_KnockbackTime = 0;
            m_Knockback = m_CombatInteraction.knockback;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_CurrentSpeed = 0;
            m_ApplyKnockback = true;
        }

        // ALWAYS RESET VALUES
        m_CombatInteraction.damage = 0;
        m_CombatInteraction.knockback = 0;
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

    private void fadingOut()
    {
        m_Rigidbody2D.velocity = Vector2.zero;

        currentRemainTime -= Time.deltaTime;

        if (currentRemainTime <= 0f)
        {
            Destroy(this.transform.parent.gameObject);
            return;
        }

        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;
    }
}
