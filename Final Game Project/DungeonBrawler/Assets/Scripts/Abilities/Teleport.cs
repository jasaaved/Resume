using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public string _class = "Wizard";

    private Rigidbody2D rb;
    public GameObject m_Reticle;
    private GameObject player;
    private ReticleMovement m_ReticleMovement;
    private float m_Angle;
    private Vector2 force;
    private WeaponStats m_WeaponStats;
    private bool cscheme1;
    private bool cscheme2;
    private bool cscheme3;
    private float m_Horizontal;
    private float m_Vertical;
    private Vector2 mag_check;
    private int playerNumber;
    private float aim_angle;
    private GameObject health;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        m_WeaponStats = GetComponent<WeaponStats>();

        m_Reticle = GameObject.Find("Wizard(Clone)");
        transform.parent = null;

        if (m_Reticle == null)
        {
            player = GameObject.Find("Wizard");
            m_Reticle = player.transform.FindChild("Reticle").gameObject;
        }

        else
        {
            player = m_Reticle;
            m_Reticle = player.transform.FindChild("Reticle").gameObject; ;
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
        cscheme1 = player.GetComponent<PlayerController>().cscheme1;
        cscheme2 = player.GetComponent<PlayerController>().cscheme2;
        cscheme3 = player.GetComponent<PlayerController>().cscheme3;
        player.GetComponent<PlayerController>().teleporting = true;
        player.GetComponent<PlayerController>().ability1Cooldown = 0;
        player.GetComponent<PlayerController>().maxAbility1Cooldown = 5f;
        health = player.transform.FindChild("HealthBar").gameObject;
        health.SetActive(false);
        HidePlayer();
        Destroy(this.gameObject, 3f);



    }

    void Update()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        playerNumber = player.GetComponent<PlayerController>().playerNumber;
        switch (playerNumber)
        {
            case 1:
                Reticle1Control();
                break;
            case 2:
                Reticle2Control();
                break;
            case 3:
                Reticle3Control();
                break;
        }

        if (m_Horizontal != 0.0f || m_Vertical != 0.0f)
        {
            aim_angle = Mathf.Atan2(m_Vertical, m_Horizontal);
            transform.eulerAngles = new Vector3(0, 0, aim_angle * Mathf.Rad2Deg);
            force.x = Mathf.Cos(aim_angle);
            force.y = Mathf.Sin(aim_angle);
            force.x = force.x * m_WeaponStats.projectileVelocity;
            force.y = force.y * m_WeaponStats.projectileVelocity;
            rb.velocity = new Vector2(force.x, force.y) * m_WeaponStats.projectileVelocity;
        }
            if (player.GetComponent<PlayerController>().playerNumber == 1)
        {
            if (cscheme2 && CrossPlatformInputManager.GetButtonUp("RBFire2"))
            {
                player.transform.position = transform.position;
                AppearPlayer();
                Destroy(this.gameObject);
            }
        }

        if (player.GetComponent<PlayerController>().playerNumber == 2)
        {
            if (cscheme2 && CrossPlatformInputManager.GetButtonUp("RBFire2P2"))
            {
                player.transform.position = transform.position;
                AppearPlayer();
                Destroy(this.gameObject);
            }
        }

        if (player.GetComponent<PlayerController>().playerNumber == 3)
        {
            if (cscheme2 && CrossPlatformInputManager.GetButtonUp("RBFire2P3"))
            {
                player.transform.position = transform.position;
                AppearPlayer();
                Destroy(this.gameObject);
            }
        }



    }

    private void Reticle1Control()
    {

        if (cscheme1 || cscheme3)
        {
            if (cscheme3 && (CrossPlatformInputManager.GetAxisRaw("RightH") != 0 || CrossPlatformInputManager.GetAxisRaw("RightV") != 0))
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("RightH");
                m_Vertical = CrossPlatformInputManager.GetAxis("RightV");
            }
            else
            {
                m_Vertical = CrossPlatformInputManager.GetAxis("Vertical");
                m_Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            }


        }

        else if (cscheme2)
        {

            mag_check = new Vector2(CrossPlatformInputManager.GetAxis("RightH"), CrossPlatformInputManager.GetAxis("RightV"));
            if (mag_check.magnitude >= 0.90)
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("RightH");
                m_Vertical = CrossPlatformInputManager.GetAxis("RightV");
            }



        }
    }
    private void Reticle2Control()
    {
        if (cscheme1 || cscheme3)
        {
            if (cscheme3 && (CrossPlatformInputManager.GetAxisRaw("RightH2") != 0 || CrossPlatformInputManager.GetAxisRaw("RightV2") != 0))
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("RightH2");
                m_Vertical = CrossPlatformInputManager.GetAxis("RightV2");
            }
            else
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("HorizontalP2");
                m_Vertical = CrossPlatformInputManager.GetAxis("VerticalP2");

            }
        }

        else if (cscheme2)
        {
            mag_check = new Vector2(CrossPlatformInputManager.GetAxis("RightH2"), CrossPlatformInputManager.GetAxis("RightV2"));
            if (mag_check.magnitude >= 0.90)
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("RightH2");
                m_Vertical = CrossPlatformInputManager.GetAxis("RightV2");
            }
        }
    }
    private void Reticle3Control()
    {
        if (cscheme1 || cscheme3)
        {
            if (cscheme3 && (CrossPlatformInputManager.GetAxisRaw("RightH3") != 0 || CrossPlatformInputManager.GetAxisRaw("RightV3") != 0))
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("RightH3");
                m_Vertical = CrossPlatformInputManager.GetAxis("RightV3");
            }
            else
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("HorizontalP3");
                m_Vertical = CrossPlatformInputManager.GetAxis("VerticalP3");
            }
        }

        else if (cscheme2)
        {
            mag_check = new Vector2(CrossPlatformInputManager.GetAxis("RightH3"), CrossPlatformInputManager.GetAxis("RightV3"));
            if (mag_check.magnitude >= 0.90)
            {
                m_Horizontal = CrossPlatformInputManager.GetAxis("RightH3");
                m_Vertical = CrossPlatformInputManager.GetAxis("RightV3");
            }
        }
    }

    void HidePlayer()
    {
        player.layer = LayerMask.NameToLayer("Phase");
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.transform.FindChild("Reticle").GetComponent<SpriteRenderer>().enabled = false;
        health.SetActive(false);

    }

    void AppearPlayer()
    {
        if (player == null)
        {
            return;
        }

        player.layer = LayerMask.NameToLayer("Players");
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.transform.FindChild("Reticle").GetComponent<SpriteRenderer>().enabled = true;
        health.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "BossDoors")
        {
            player.transform.position = transform.position;
            AppearPlayer();
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (player != null)
        {
            player.transform.position = transform.position;
            player.GetComponent<PlayerController>().teleporting = false;
        }

        AppearPlayer();
    }
}
