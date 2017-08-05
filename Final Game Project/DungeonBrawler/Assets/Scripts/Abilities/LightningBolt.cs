using UnityEngine;
using UnityStandardAssets._2D;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LightningBolt : MonoBehaviour
{

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    [HideInInspector] public GameObject m_Player;
    public GameObject lightningblast;
    private ReticleMovement m_ReticleMovement;
    private float m_Angle;
    private WeaponStats m_WeaponStats;
    private Vector2 s;
    private Vector2 e;
    BoxCollider2D bc;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////

    void Start()
    {
        m_WeaponStats = GetComponent<WeaponStats>();

        m_Player = GameObject.Find("Wizard");

        if (m_Player == null)
        {
            m_Player = GameObject.Find("Wizard(Clone)");
        }

        if (m_Player != null && m_Player.GetComponent<PlayerController>() != null)
        {
            m_Player.GetComponent<PlayerController>().ability2Cooldown = 0;
            m_Player.GetComponent<PlayerController>().maxAbility2Cooldown = m_WeaponStats.cooldown;
        }

        bc = GetComponent<BoxCollider2D>();

        //Destroy(transform.parent.gameObject, 1f);
    }
    void Update()
    {
        s = transform.parent.GetComponent<LightningBoltScript>().start;
        e = transform.parent.GetComponent<LightningBoltScript>().end;
        float lineLength = Vector3.Distance(s, e); // length of line

        if (bc != null)
        {
            bc.size = new Vector2(lineLength, 1.0f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
            Vector2 midPoint = (s + e) / 2;
            bc.transform.position = midPoint; // setting position of collider object
        }
        // Following lines calculate the angle between startPos and endPos

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
        if (other.tag == "Dead")
        {
            if (other.GetComponent<PlayerController>().isDead == true)
            {
                other.GetComponent<PlayerController>().LightningRevivePlayer();
            }
        }
        if (other.tag == "Ground")
        {
            transform.parent.GetComponent<LightningBoltScript>().grow = false;
            lightningblast = Instantiate(lightningblast);
            Vector3 end = transform.parent.GetComponent<LightningBoltScript>().end;
            lightningblast.transform.position = new Vector3(end.x, end.y + 0.75f, end.z);
            Destroy(transform.parent.gameObject);

        }
    }
    /*
    void OnDestroy()
    {
        lightningblast = Instantiate(lightningblast);
        Vector3 end = transform.parent.GetComponent<LightningBoltScript>().end;
        lightningblast.transform.position = new Vector3(end.x, end.y + 0.75f, end.z);
    }
    */
}
