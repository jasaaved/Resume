using UnityEngine;
using System.Collections;

public class FireCone : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    private GameObject m_Player;
    private GameObject m_Reticle;
    private ReticleMovement m_ReticleMovement;
    private float m_Angle;
    private WeaponStats m_WeaponStats;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        Destroy(this.gameObject, 0.1f);

        m_WeaponStats = GetComponent<WeaponStats>();
        
        m_Reticle = GameObject.Find("Wizard(Clone)");

        if (m_Reticle == null)
        {
            m_Reticle = GameObject.Find("Wizard").transform.FindChild("Reticle").gameObject;
        }
        else
        {
            m_Reticle = GameObject.Find("Wizard(Clone)").transform.FindChild("Reticle").gameObject; ;
        }

        m_ReticleMovement = m_Reticle.GetComponent<ReticleMovement>();

        m_Angle = m_ReticleMovement.angle;
        float posx = m_Reticle.transform.position.x;
        float posy = m_Reticle.transform.position.y;

        if(m_Angle * Mathf.Rad2Deg > 90 && m_Angle * Mathf.Rad2Deg < 270)
        {
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        }

        transform.position = new Vector3(Mathf.Cos(m_Angle) * m_WeaponStats.range + posx, Mathf.Sin(m_Angle) * m_WeaponStats.range + posy, 0);
        transform.eulerAngles = new Vector3(0, 0, m_Angle * Mathf.Rad2Deg);
        transform.parent = gameObject.transform;

        m_Player = GameObject.Find("Wizard");
        if (m_Player == null)
        {
            m_Player = GameObject.Find("Wizard(Clone)");
        }

        m_Player.GetComponent<PlayerController>().ability1Cooldown = 0;
        m_Player.GetComponent<PlayerController>().maxAbility1Cooldown = m_WeaponStats.cooldown;
    }
    void FixedUpdate()
    {
        m_Angle = m_ReticleMovement.angle;
        float posx = m_Reticle.transform.position.x;
        float posy = m_Reticle.transform.position.y;

        transform.position = new Vector3(Mathf.Cos(m_Angle) * m_WeaponStats.range + posx, Mathf.Sin(m_Angle) * m_WeaponStats.range + posy, 0);
        transform.eulerAngles = new Vector3(0, 0, m_Angle * Mathf.Rad2Deg);
        transform.parent = gameObject.transform;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            CombatInteraction combatInteraction = other.GetComponent<CombatInteraction>();

            if (combatInteraction != null)
            {
                combatInteraction.damage += m_WeaponStats.damage;
                combatInteraction.knockback += m_WeaponStats.knockback;
                combatInteraction.direction = gameObject.transform.right;
            }
        }
    }

}
