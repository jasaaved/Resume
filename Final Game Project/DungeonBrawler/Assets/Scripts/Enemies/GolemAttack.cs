using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : MonoBehaviour {

    private float knockBack = 10;
    private GolemBehavior m_GolemBehavior;
    private CombatInteraction combatInteraction;
    private EnemyStats enemyStats;

    void Awake()
    {
        m_GolemBehavior = transform.parent.GetComponent<GolemBehavior>();
        combatInteraction = transform.parent.GetComponent<CombatInteraction>();
        enemyStats = transform.parent.GetComponent<EnemyStats>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!m_GolemBehavior.isDead)
        {
            if (combatInteraction.stunned)
            {
                return;
            }
            if (other.tag == "Player")
            {
                other.GetComponent<CombatInteraction>().damage = enemyStats.damage;
                other.GetComponent<CombatInteraction>().knockback = knockBack;
            }
        }
    }

}
