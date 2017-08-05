using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    private float m_AttackTime = 0;
    private KnightBehavior m_KnightBehavior;
    private CombatInteraction m_CombatInteraction;
    private EnemyStats m_EnemyStats;
    private Animator m_Animator;
    public GameObject AttackTarget;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Awake()
    {
        m_KnightBehavior = transform.parent.GetComponent<KnightBehavior>();
        m_CombatInteraction = transform.parent.GetComponent<CombatInteraction>();
        m_EnemyStats = transform.parent.GetComponent<EnemyStats>();
        m_Animator = transform.parent.GetComponent<Animator>();
    }

    void Update()
    {
        if (m_KnightBehavior.GetComponent<KnightBehavior>().m_TargetObject == null)
        {
            m_Animator.SetBool("attack", false);
            m_KnightBehavior.moving = true;
            m_KnightBehavior.attacking = false;
        }

        if (AttackTarget != null && AttackTarget != m_KnightBehavior.GetComponent<KnightBehavior>().m_TargetObject)
        {
            m_Animator.SetBool("attack", false);
            m_KnightBehavior.moving = true;
            m_KnightBehavior.attacking = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!m_KnightBehavior.isDead)
        {
            if (m_CombatInteraction.stunned)
            {
                return;
            }
            if (other.tag == "Player")
            {
                m_KnightBehavior.moving = false;
                m_KnightBehavior.attacking = true;
                AttackTarget = other.gameObject;

                if (m_AttackTime == 0)
                {
                    m_Animator.SetBool("attack", true);
                    other.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!m_KnightBehavior.isDead)
        {
            if (m_CombatInteraction.stunned)
            {
                return;
            }
            if (other.tag == "Player" || other.tag == "Shield")
            {
                m_KnightBehavior.moving = false;
                m_KnightBehavior.attacking = true;
                AttackTarget = other.gameObject;
                m_AttackTime += Time.deltaTime;

                if (m_AttackTime >= m_EnemyStats.attackSpeed)
                {
                    m_Animator.SetBool("attack", true);
                    other.GetComponent<CombatInteraction>().damage = m_EnemyStats.damage;
                    m_AttackTime = 0;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!m_KnightBehavior.isDead)
        {
            if (other.tag == "Player")
            {
                m_Animator.SetBool("attack", false);
                m_KnightBehavior.moving = true;
                m_KnightBehavior.attacking = false;
                AttackTarget = null;
            }
        }
    }
}
