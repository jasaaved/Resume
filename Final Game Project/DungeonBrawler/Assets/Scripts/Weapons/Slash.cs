using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////


    private GameObject m_Reticle;
    private ReticleMovement m_ReticleMovement;
    private float m_Angle;
    private WeaponStats m_WeaponStats;
    private Animator m_animator;
    public AudioClip[] sounds;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////

    void Start()
    {
        Destroy(this.gameObject, 0.2f);
        m_animator = GetComponent<Animator>();
        m_WeaponStats = GetComponent<WeaponStats>();

        m_Reticle = GameObject.Find("Warrior(Clone)");

        if (m_Reticle == null)
        {
            m_Reticle = GameObject.Find("Warrior").transform.FindChild("Reticle").gameObject;
        }
        else
        {
            m_Reticle = GameObject.Find("Warrior(Clone)").transform.FindChild("Reticle").gameObject; ;
        }

        m_ReticleMovement = m_Reticle.GetComponent<ReticleMovement>();

        m_Angle = m_ReticleMovement.angle;
        float posx = m_Reticle.transform.position.x;
        float posy = m_Reticle.transform.position.y;

        transform.position = new Vector3(Mathf.Cos(m_Angle) * m_WeaponStats.range + posx, Mathf.Sin(m_Angle) * m_WeaponStats.range + posy, 0);
        transform.eulerAngles = new Vector3(0, 0, m_Angle * Mathf.Rad2Deg);
        transform.parent = gameObject.transform;
        PlayAudio();
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

    void PlayAudio()
    {
        int sound = Random.Range(0, sounds.Length);
        this.GetComponent<AudioSource>().clip = sounds[sound];
        this.GetComponent<AudioSource>().Play();
    }

}
