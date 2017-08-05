using UnityEngine;
using System.Collections;

public class SlimeBehavior : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public float patrolSize;
    public float jumpForce = 350f;
    public float minJumpFrequency = 1f;
    public float maxJumpFrequency = 2f;
    [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character

    private Transform m_GroundCheck; // A position marking where to check if the player is grounded.
    private const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded; // Whether or not the player is grounded.
    private float m_StartPosition;
    private Rigidbody2D m_Rigidbody2D;
    private EnemyStats m_EnemyStats;
    private Vector3 m_PlayerPosition;
    private float m_Direction = 100f;
    private float m_JumpTimer;
    private float m_AttackTime = 0;
    private Color m_MainColor;
    private CombatInteraction m_CombatInteraction;
    private GameObject m_Player;
    private Animator m_animator;
    private float alpha;
    private Color color;
    private float currentRemainTime;
    public float fadeTime = 3f;
    private SpriteRenderer m_Renderer;
    private bool isDead;
    private Vector2 acceleration = new Vector2(0, -9.81f);
    private float sizeX;
    private float sizeY;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    private void Awake()
    {
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_EnemyStats = GetComponent<EnemyStats>();
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_JumpTimer = Random.Range(minJumpFrequency, maxJumpFrequency);
        m_animator = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
        isDead = false;
        sizeX = this.transform.localScale.x;
        sizeY = this.transform.localScale.y;
    }

    private void Start()
    {
        // Needs to be set in Start, otherwise is null
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isDead)
        {
            if (m_CombatInteraction.stunned)
            {
                return;
            }

            CombatInteractionHandler();

            if (m_EnemyStats.health <= 0)
            {
                isDead = true;
                currentRemainTime = fadeTime;
                m_Rigidbody2D.velocity = Vector2.zero;
                m_Rigidbody2D.gravityScale = 3.0f;
            }
        }
        else
        {
            currentRemainTime -= Time.deltaTime;
            if (currentRemainTime <= 0f)
            {
                Destroy(this.gameObject);
                return;
            }
            GetComponent<SpawnHealthDrop>().SpawnDrop();
            fadingOut();
            getSmaller();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (m_CombatInteraction.stunned)
            {
                m_animator.SetBool("Ground", m_Grounded);
                m_animator.SetFloat("vSpeed", 0);
                return;
            }
            m_animator.SetBool("Ground", m_Grounded);
            m_animator.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            OrientSlime();
            if (m_CombatInteraction.stunned)
            {
                return;
            }

            OnGround();
            Move();
        }
        else
        {
            m_Rigidbody2D.AddForce(acceleration);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead)
        {
            if (m_CombatInteraction.stunned)
            {
                return;
            }
            if (other.tag == "Player" || other.tag == "Shield")
            {
                other.GetComponent<PlayerStats>().isPoisoned = true;

                if (m_AttackTime == 0)
                {
                    other.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
                }
            }
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (!isDead)
        {
            if (m_CombatInteraction.stunned)
            {
                return;
            }
            if (other.tag == "Player" || other.tag == "Shield")
            {
                m_AttackTime += Time.deltaTime;

                other.GetComponent<PlayerStats>().poisonTimer = 2.0f;


                if (m_AttackTime >= GetComponent<EnemyStats>().attackSpeed)
                {
                    other.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
                    m_AttackTime = 0;
                }
            }
        }
    }


    /////////////////////////////////////////////////////////////////
    // METHODS
    /////////////////////////////////////////////////////////////////
    void Move()
    {
        if(!m_Grounded)
        {
            return;
        }

        m_PlayerPosition = m_Player.transform.position;

        if (m_PlayerPosition.x < transform.position.x)
        {
            m_Direction = -100f;
        }
        else
        {
            m_Direction = 100f;
        }

        m_JumpTimer -= Time.deltaTime;

        if (m_JumpTimer < 0)
        {
            m_Rigidbody2D.AddForce(new Vector2(m_Direction, jumpForce));
            m_JumpTimer = Random.Range(minJumpFrequency, maxJumpFrequency);
        }
        
    }
    private void OnGround()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Vector2 left = new Vector2(m_GroundCheck.position.x - transform.localScale.x / 2, m_GroundCheck.position.y);
        Vector2 right = new Vector2(m_GroundCheck.position.x + transform.localScale.x / 2, m_GroundCheck.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Ground" || colliders[i].gameObject.tag == "Platform")
            {
                m_Grounded = true;
            }
        }
        //m_Anim.SetBool("Ground", m_Grounded);
    }
    private void CombatInteractionHandler()
    {
        if (m_CombatInteraction.damage > 0)
        {
            // Deal the damage
            m_EnemyStats.health -= m_CombatInteraction.damage;
            GameObject blood = Resources.Load<GameObject>("Prefabs/SlimeSplatter");
            //ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            //var col = ps.colorOverLifetime;
            //Gradient grad = new Gradient();
            //grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            //col.color = grad;
            GameObject.Instantiate(blood, transform.position, transform.rotation);
            // Show damage has been taken
            StartCoroutine(DisplayDamageTaken());       
        }
        if (m_CombatInteraction.knockback > 0)
        {
            ApplyKnockback();
        }

        // ALWAYS RESET VALUES
        m_CombatInteraction.damage = 0;
        m_CombatInteraction.knockback = 0;
    }
    private void ApplyKnockback()
    {
        Vector2 dir = m_CombatInteraction.direction.normalized;
        Vector2 vel = m_Rigidbody2D.velocity;

        // Knockback in negative or positive x direction a fixed amount
        if(dir.x < 0)
        {
            vel.x = -m_CombatInteraction.knockback;
        }
        else
        {
            vel.x = m_CombatInteraction.knockback;
        }

        // Knockback in positive y direction a fixed amount
        vel.y = m_CombatInteraction.knockback;
        
        m_Rigidbody2D.velocity = vel;
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

    private void OrientSlime()
    {
        if(m_Rigidbody2D.velocity.x <= 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -3;
            transform.localScale = theScale;
        }
        else
        {
            Vector3 theScale = transform.localScale;
            theScale.x = 3;
            transform.localScale = theScale;
        }
    }

    private void fadingOut()
    {
        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;
    }

    private void getSmaller()
    {
        sizeX = currentRemainTime / fadeTime;
        sizeY = currentRemainTime / fadeTime;
        this.transform.localScale = new Vector3(sizeX, sizeY, this.transform.localScale.z);
    }
}
