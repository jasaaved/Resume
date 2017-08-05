using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightShield : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public float damageThreshold;

    private CombatInteraction m_ShieldCombatInteraction;
    private CombatInteraction m_ParentCombatInteraction;
    private KnightBehavior m_KnightBehavior;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        m_ShieldCombatInteraction = GetComponent<CombatInteraction>();
        m_ParentCombatInteraction = transform.parent.GetComponent<CombatInteraction>();
        m_KnightBehavior = transform.parent.GetComponent<KnightBehavior>();

        m_ShieldCombatInteraction.immuneToCC = true;
    }
    void Update()
    {
        BlockDamage();
    }


    /////////////////////////////////////////////////////////////////
    // METHODS
    /////////////////////////////////////////////////////////////////
    void BlockDamage()
    {
        if(m_ShieldCombatInteraction.damage > 0)
        {
            m_ParentCombatInteraction.damage = 0;
        }
        if (m_ShieldCombatInteraction.knockback > 0)
        {
            m_ParentCombatInteraction.knockback = m_ShieldCombatInteraction.knockback / 2;
            m_ParentCombatInteraction.direction = m_ShieldCombatInteraction.direction;
        }

        // ALWAYS RESET VALUES
        m_ShieldCombatInteraction.damage = 0;
        m_ShieldCombatInteraction.knockback = 0;
    }

}
