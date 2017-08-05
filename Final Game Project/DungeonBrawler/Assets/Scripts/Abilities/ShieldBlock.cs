using UnityEngine;
using UnityStandardAssets._2D;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ShieldBlock : MonoBehaviour
{

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public float range = 25;
    public float knockback = 20;
    public float cooldown = 1f;

    private bool cscheme1;
    private bool cscheme2;
    private bool cscheme3;
    private CombatInteraction m_CombatInteraction;
    private PlayerStats m_PlayerStats;

    private GameObject parent;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        parent = transform.parent.gameObject;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.position = parent.transform.position;
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_PlayerStats = GetComponent<PlayerStats>();

        // Only alter cooldown for the warrior
        if (parent.name == "Warrior" || parent.name == "Warrior(Clone)")
        {
            parent.GetComponent<PlayerController>().ability1Cooldown = 0;
            parent.GetComponent<PlayerController>().maxAbility1Cooldown = cooldown;
        }

        parent.GetComponent<PlayerController>().invincible = true;

        cscheme1 = transform.parent.GetComponent<PlayerController>().cscheme1;
        cscheme2 = transform.parent.GetComponent<PlayerController>().cscheme2;
        cscheme3 = transform.parent.GetComponent<PlayerController>().cscheme3;
        Destroy(this.gameObject, 3f);
    }
    void Update()
    {
        if (transform.parent.tag == "Dead")
        {
            Destroy(this.gameObject);
            return;
        }
        if (m_PlayerStats.health <= 0)
        {
            Destroy(this.gameObject);
        }

        if(transform.localScale.x < 1.1)
        {
            transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 10;
        }
        else
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }
    void OnDestroy()
    {
        parent.GetComponent<PlayerController>().invincible = false;
    }

}
