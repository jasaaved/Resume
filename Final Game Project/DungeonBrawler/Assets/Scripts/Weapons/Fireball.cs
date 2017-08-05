using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public string _class = "Wizard";

    private Rigidbody2D rb;
    public GameObject m_Reticle;
    private ReticleMovement m_ReticleMovement;
    private float m_Angle;
    private Vector2 force;
    public GameObject fireblast;
    private WeaponStats m_WeaponStats;
    public AudioClip[] sounds;
    private Vector2 direction;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
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

        transform.position = new Vector3(Mathf.Cos(m_Angle) * m_WeaponStats.range + posx, Mathf.Sin(m_Angle) * m_WeaponStats.range + posy, 0);
        transform.eulerAngles = new Vector3(0, 0, m_Angle * Mathf.Rad2Deg);

        rb = GetComponent<Rigidbody2D>();
        force.x = Mathf.Cos(m_Angle);
        force.y = Mathf.Sin(m_Angle);
        force.x = force.x * m_WeaponStats.projectileVelocity;
        force.y = force.y * m_WeaponStats.projectileVelocity;
        rb.velocity = new Vector2(force.x, force.y) * m_WeaponStats.projectileVelocity;
        PlayAudio();
    }

    void FixedUpdate()
    {
        if (m_WeaponStats.projectileLifeSpan <= 0f)
        {
            Destroy(this.gameObject);
        }
        m_WeaponStats.projectileLifeSpan -= Time.deltaTime;

        rb.velocity = new Vector2(rb.velocity.x * (1f+Time.deltaTime), rb.velocity.y * (1f+Time.deltaTime));

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            CombatInteraction combatInteraction = other.GetComponent<CombatInteraction>();
            direction = gameObject.transform.right;

            if (combatInteraction != null)
            {
                //combatInteraction.damage += m_WeaponStats.damage;
                combatInteraction.knockback += m_WeaponStats.knockback;
                combatInteraction.direction = direction;
            }
            Destroy(this.gameObject);
        }
        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        fireblast = Instantiate(fireblast);
        fireblast.transform.position = transform.position;
        fireblast.GetComponent<AOEAttack>().direction = direction;
    }

    void PlayAudio()
    {
        int sound = Random.Range(0, sounds.Length);
        this.GetComponent<AudioSource>().clip = sounds[sound];
        this.GetComponent<AudioSource>().Play();
    }
}
