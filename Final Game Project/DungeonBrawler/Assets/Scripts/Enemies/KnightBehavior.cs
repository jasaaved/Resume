using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehavior : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public bool moving;
    public bool attacking;
    public Color damageTakenColor = Color.red;
    public Color blockDamageColor = Color.gray;

    private float m_MoveTime = 0;
    private float m_AttackTime = 0;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_StartPos;
    public GameObject m_TargetObject;
    private EnemyStats m_EnemyStats;
    private GameObject m_Parent;
    private GameObject m_Zone;
    private KnightAggroTrigger m_AggroTrigger;
    private CombatInteraction m_CombatInteraction;
    private bool m_ApplyKnockback;
    private float m_KnockbackTime = 0;
    private float m_Knockback = 0;
    private float m_CurrentSpeed = 0;
    private float m_xScale;
    private Animator m_Animator;
    private Color m_MainColor;

    private float alpha;
    private Color color;
    private float currentRemainTime;
    public float fadeTime = 2f;
    private SpriteRenderer m_Renderer;
    public bool isDead;
    private GameObject blockParticle;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Zone = transform.GetChild(0).gameObject;
        m_AggroTrigger = gameObject.GetComponentInChildren<KnightAggroTrigger>();
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_EnemyStats = GetComponent<EnemyStats>();
        m_StartPos = transform.position;
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        m_CurrentSpeed = m_EnemyStats.speed;
        m_xScale = transform.localScale.x;
        m_Renderer = GetComponent<SpriteRenderer>();
        blockParticle = transform.FindChild("BlockParticle").gameObject;
        isDead = false;
        moving = false;
        attacking = false;
    }
    void Update()
    {
        if (!isDead)
        {
            // Disable if off screen
            if (!GetComponent<Renderer>().isVisible)
            {
                return;
            }

            if (m_CombatInteraction.stunned)
            {
                return;
            }

            if (m_AggroTrigger != null)
            {
                m_TargetObject = m_AggroTrigger.targetObject;
            }

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

            if (m_AggroTrigger.isAggroed)
            {
                moving = true;
                Movement();
            }
            else if (!m_AggroTrigger.isAggroed && (m_StartPos.x != transform.position.x))
            {
                Return();
            }
            else
            {
                moving = false;
            }

            if (moving)
            {
                m_Animator.SetFloat("speed", 1.0f);
            }
            else
            {
                m_Animator.SetFloat("speed", 0.0f);
            }

            m_Zone.transform.position = transform.position;
        }
        else
        {
            GetComponent<SpawnHealthDrop>().SpawnDrop();
            fadingOut();
            DisplayDeath();
        }
    }


    /////////////////////////////////////////////////////////////////
    // METHODS
    /////////////////////////////////////////////////////////////////
    private void Movement()
    {
        if(attacking)
        {
            moving = false;
            return;
        }

        if (m_CurrentSpeed < m_EnemyStats.speed)
        {
            m_CurrentSpeed += 0.1f;
        }

        if (m_TargetObject != null)
        {
            // Flip the gameObject accordingly
            if (m_TargetObject.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(m_xScale, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-m_xScale, transform.localScale.y, transform.localScale.z);
            }

            // Move towards player
            if (transform.position.x > m_TargetObject.transform.position.x)
            {
                Vector2 target = new Vector2(m_TargetObject.transform.position.x + 0.5f, transform.position.y);
                Vector2 moveTowards = Vector2.MoveTowards(transform.position, target, m_CurrentSpeed * Time.deltaTime);
                transform.position = new Vector2(moveTowards.x, transform.position.y);
            }
            else
            {
                Vector2 target = new Vector2(m_TargetObject.transform.position.x - 0.5f, m_TargetObject.transform.position.y);
                Vector2 moveTowards = Vector2.MoveTowards(transform.position, target, m_CurrentSpeed * Time.deltaTime);
                transform.position = new Vector2(moveTowards.x, transform.position.y);
            }
        }
    }
    private void Return()
    {
        if (transform.position.y > m_StartPos.y + 1f || transform.position.y < m_StartPos.y - 1f)
        {
            moving = false;
            return;
        }

        // Flip the gameObject accordingly
        if (m_StartPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(m_xScale, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-m_xScale, transform.localScale.y, transform.localScale.z);
        }

        transform.position = Vector2.MoveTowards(transform.position, m_StartPos, m_CurrentSpeed * Time.deltaTime);
    }
    private void CombatInteractionHandler()
    {
        if (m_CombatInteraction.damage > 0)
        {
            bool knightFacingRight = transform.localScale.x > 0;
            bool attackFromRight = m_CombatInteraction.direction.x > 0;
            if (knightFacingRight && !attackFromRight)
            {
                // Block damage from the right
                m_CombatInteraction.damage = 0;
                StartCoroutine(DisplayBlockDamage());
                blockParticle.transform.localRotation = Quaternion.Euler(0, 0, 0);
                blockParticle.GetComponent<ParticleSystem>().Emit(5);
                return;
            }
            else if (!knightFacingRight && attackFromRight)
            {
                // Block damage from the left
                m_CombatInteraction.damage = 0;
                StartCoroutine(DisplayBlockDamage());
                blockParticle.transform.localRotation = Quaternion.Euler(0, 0, -135);
                blockParticle.GetComponent<ParticleSystem>().Emit(5);
                return;
            }


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
            damageTakenColor = Color.red;
        }

        // ALWAYS RESET VALUES
        m_CombatInteraction.damage = 0;
        m_CombatInteraction.knockback = 0;
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", damageTakenColor);

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.material.SetColor("_Color", m_MainColor);
        }
    }
    private IEnumerator DisplayBlockDamage()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", blockDamageColor);

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
            Destroy(this.gameObject);
            return;
        }

        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;
    }

    private void DisplayDeath()
    {
        if (transform.rotation.eulerAngles.z < 90)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 10f);
        }
    }
}
