using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Rewired;
using UnityStandardAssets.CrossPlatformInput;



public class PlayerController : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    [System.NonSerialized] public int playerNumber;
    public GameObject weapon;
    public GameObject ability1;
    public GameObject ability2;
    public GameObject reviveEffect;
    public GameObject poisonEffect;
    public GameObject feet;
    public bool invincibileOnHit = true; // Used to determine if the players have invincibility after being hit.
    public bool invincible = false;
    public float maxInvincibilityTime = 0.5f;
    public Color flashDamage = Color.red;
    public Color flashBlock = Color.blue;
    public bool movement; // Used to determine if the players is able to move.
    public bool isDead = false;
    public bool isRevived = false;
    public bool cscheme1 = false;
    public bool cscheme2 = false;
    public bool cscheme3 = false;
    public string location;
    public int enemies_left;
    public UnityEvent OnAbility2ConsecutiveEvent;
    [HideInInspector] public float weaponCooldown = 0f;
    [HideInInspector] public float ability1Cooldown = 0f;
    [HideInInspector] public float ability2Cooldown = 0f;
    [HideInInspector] public float maxWeaponCooldown = 0f;
    [HideInInspector] public float maxAbility1Cooldown = 0f;
    [HideInInspector] public float maxAbility2Cooldown = 0f;
    [HideInInspector] public float maxPhaseTime = 0.25f;
    [HideInInspector] public bool teleporting = false;

    private PlayerMovement m_Character;
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    private const float k_GroundedRadius = 0.1f; // Radius of the overlap circle to determine if grounded
    private bool m_OnPlatform;           
    private bool m_Jump;
    private bool m_DoAttack;
    private bool m_DoAbility1;
    private bool m_DoAbility2;
    private bool m_DownPressed;
    private float m_Horizontal;
    private float m_Vertical;
    private PlayerStats m_PlayerStats;
    private Color m_MainColor;
    private float m_PreviousHealth;
    private CombatInteraction m_CombatInteraction;
    private float m_InvincibilityTimer;
    private float m_CurrentPhaseTime;
    private GameObject reticle;
    private bool LTinuse = false;
    private bool RTinuse = false;
    private Animator m_anim;
    private GameObject healthbar;
    private bool reviving;
    private float reviveTimer;
    private GameObject reviveEffectCopy;
    private float maxSpeed;
    private float maxJumpForce;
    private bool alreadyPoisoned = false;
    private GameObject poisonEffectCopy;
    public AudioClip[] SelectedLines;
    public AudioClip[] AttackLines;
    public AudioClip[] Ability1Lines;
    public AudioClip[] Ability2Lines;
    public AudioClip[] DamagedSounds;
    public AudioClip[] DeathLines;
    public AudioClip[] DeselectedLines;
    private AudioSource audioPlayer;
    private GameObject gameManager;

    private Player p1;
    private Player p2;
    private Player p3;



    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Awake()
    {
        // Player Number
        if (transform.name == "Warrior" || transform.name == "Warrior(Clone)")
        {
            playerNumber = PlayerStats.warriorNum;
        }
        else if (transform.name == "Rogue" || transform.name == "Rogue(Clone)")
        {
            playerNumber = PlayerStats.rogueNum;
        }
        else if (transform.name == "Wizard" || transform.name == "Wizard(Clone)")
        {
            playerNumber = PlayerStats.wizardNum;
        }

        // Objects
        m_GroundCheck = transform.FindChild("GroundCheck");
        healthbar = transform.FindChild("HealthBar").gameObject;
        reticle = transform.FindChild("Reticle").gameObject;
        gameManager = GameObject.Find("GameManager");

        // Components
        m_Character = GetComponent<PlayerMovement>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_PlayerStats = GetComponent<PlayerStats>();
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_anim = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();

        // Values
        maxWeaponCooldown = weapon.GetComponent<WeaponStats>().cooldown;
        weaponCooldown = maxWeaponCooldown;
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        m_PreviousHealth = m_PlayerStats.health;
        m_InvincibilityTimer = maxInvincibilityTime;
        m_CurrentPhaseTime = 0f;
        movement = true;
        maxSpeed = m_PlayerStats.maxSpeed;
        maxJumpForce = m_PlayerStats.maxJumpForce;

        p1 = ReInput.players.GetPlayer(0);
        p2 = ReInput.players.GetPlayer(1);
        p3 = ReInput.players.GetPlayer(2);
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            PlaySelectedLine();
        }
    }
    void Update()
    {
        if (gameManager != null && gameManager.GetComponent<GameManager>().winState)
        {
            GetRecovered();
            return;
        }

        if (isDead)
        {
            if (m_PlayerStats.isPoisoned)
            {
                GetRecovered();
            }
            return;
        }

        if (m_PlayerStats.isPoisoned && !alreadyPoisoned)
        {
            GetPoisoned();
        }

        m_PlayerStats.poisonTimer -= Time.deltaTime;

        if (m_PlayerStats.isPoisoned && m_PlayerStats.poisonTimer <= 0)
        {
            GetRecovered();
        }

        if (m_CurrentPhaseTime > 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Phase");
            feet.layer = LayerMask.NameToLayer("Phase");
            m_CurrentPhaseTime -= Time.deltaTime;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Players");
            feet.layer = LayerMask.NameToLayer("Players");
            m_CurrentPhaseTime = 0f;
        }

        CombatInteractionHandler();

        if (!teleporting)
        {
            switch (playerNumber)
            {
                case 1:
                    Player1Control();
                    break;
                case 2:
                    Player2Control();
                    break;
                case 3:
                    Player3Control();
                    break;
            }

            OnWeapon();
            OnAbility1();
            OnAbility2();
        }
        OnPlatform();
        ResetUpdateValues();
    }
    void FixedUpdate()
    {
        if (isRevived || isDead)
        {
            // Cancel x-axis velocity
            Vector3 v = m_Rigidbody2D.velocity;
            v.x = 0;

            if (v.y > 0)
            {
                v.y -= Time.deltaTime;
            }

            m_Rigidbody2D.velocity = v;
        }

        if (isRevived)
        {
            DisplayRevive();
            return;
        }
        if (isDead)
        {
            DisplayDeath();
            return;
        }
        if(m_Rigidbody2D.gravityScale < 5f)
        {
            m_Rigidbody2D.gravityScale += Time.deltaTime*4f;
        }
        else if(m_Rigidbody2D.gravityScale > 5f)
        {
            m_Rigidbody2D.gravityScale = 5f;
        }

        OnMove();
        ResetFixedUpdateValues();
    }


    /////////////////////////////////////////////////////////////////
    // LISTENERS
    /////////////////////////////////////////////////////////////////
    private void OnPlatform()
    {
        m_OnPlatform = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Platform")
            {
                m_OnPlatform = true;
            }
        }

        // Make player fall through platforms if they hit 'down' + 'jump' while on a platform
        if (m_Vertical < -0.60)
        {
            if (m_OnPlatform && m_Jump)
            {
                m_CurrentPhaseTime = maxPhaseTime;
                // Disable jumping for this action
                m_Jump = false;
            }
        }
    }
    private void OnWeapon()
    {
        if (m_DoAttack && weaponCooldown >= maxWeaponCooldown)
        {
            GameObject.Instantiate(weapon);
            PlayAttackLine();
            weaponCooldown = 0;
        }
        else if (weaponCooldown < maxWeaponCooldown)
        {
            weaponCooldown += Time.deltaTime;
        }
    }
    private void OnAbility1()
    {
        if (m_DoAbility1 && ability1Cooldown >= maxAbility1Cooldown)
        {
            GameObject.Instantiate(ability1, transform);
            PlayAbility1Line();
        }
        else if (ability1Cooldown < maxAbility1Cooldown)
        {
            ability1Cooldown += Time.deltaTime;
        }
    }
    private void OnAbility2()
    {
        if (m_DoAbility2 && ability2Cooldown >= maxAbility2Cooldown)
        {
            GameObject.Instantiate(ability2, transform);
            PlayAbility2Line();
        }
        else if (ability2Cooldown < maxAbility2Cooldown)
        {
            ability2Cooldown += Time.deltaTime;

            if (m_DoAbility2 && OnAbility2ConsecutiveEvent != null)
            {
                OnAbility2ConsecutiveEvent.Invoke();
            }
        }
    }
    private void OnMove()
    {
        // Pass all parameters to the character control script.
        if (!movement)
        {
            m_Horizontal = 0;
            m_Vertical = 0;
            m_Jump = false;
        }

        m_Character.MovePlayer(m_Horizontal, m_Vertical, m_Jump);
    }


    /////////////////////////////////////////////////////////////////
    // PUBLIC METHODS
    /////////////////////////////////////////////////////////////////
    public void ResetPlayer()
    {
        isRevived = true;
        m_PlayerStats.health = m_PlayerStats.maxHealth;
        m_PreviousHealth = m_PlayerStats.maxHealth;
        gameObject.tag = "Player";
        gameObject.layer = LayerMask.NameToLayer("Players");
    }
    public void RevivePlayer()
    {
        isDead = false;
        isRevived = true;
        m_PlayerStats.health = m_PlayerStats.maxHealth / 2;
        reticle.SetActive(true);
        gameObject.tag = "Player";
        gameObject.layer = LayerMask.NameToLayer("Players");
    }
    public void LightningRevivePlayer()
    {
        isDead = false;
        isRevived = true;
        m_PlayerStats.health = m_PlayerStats.maxHealth / 3;
        reticle.SetActive(true);
        gameObject.tag = "Player";
        gameObject.layer = LayerMask.NameToLayer("Players");
    }


    /////////////////////////////////////////////////////////////////
    // PRIVATE METHODS
    /////////////////////////////////////////////////////////////////
    private void Player1Control()
    {
        m_Horizontal = p1.GetAxis("LS Axis X"); //// m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = p1.GetAxis("LS Axis Y"); //// m_Vertical = Input.GetAxis("Vertical");
        

        if (!m_Jump)
        {
            if (cscheme1 || cscheme3)
            {// Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            else if (cscheme2)
            {
                if (p1.GetButtonDown("LT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("LTJump") != 0)
                {
                    if (!LTinuse)
                    {
                        LTinuse = true;
                        m_Jump = true;
                    }
                }

                if (!p1.GetButtonDown("LT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("LTJump") == 0)
                {
                    LTinuse = false;

                }

            }

        }

        if (!m_DoAttack)
        {
            if (cscheme1)
            {
                m_DoAttack = CrossPlatformInputManager.GetButtonDown("Fire1");
            }

            else if (cscheme2)
            {
                if (p1.GetButtonDown("RT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("RTFire1") != 0)
                {
                    m_DoAttack = true;
                    //if (!RTinuse)
                    //{
                    //    RTinuse = true;
                    //    m_DoAttack = true;
                    //}
                }
                else
                {
                    m_DoAttack = false;
                }

                if (!p1.GetButtonDown("RT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("RTFire1") == 0)
                {
                    RTinuse = false;
                }
            }

            else if (cscheme3)
            {
                if (CrossPlatformInputManager.GetAxisRaw("RightH") != 0 || CrossPlatformInputManager.GetAxisRaw("RightV") != 0)
                {
                    m_DoAttack = true;
                }
            }
        }

        if (!m_DoAbility1)
        {
            if (cscheme1 || cscheme3)
            {
                m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("Fire2"); //// m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("Fire2");
            }

            else if (cscheme2)
            {
                m_DoAbility1 = p1.GetButtonDown("RB"); //// m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("RBFire2");
            }
        }

        if (!m_DoAbility2)
        {
            if (cscheme1 || cscheme3)
            {
                m_DoAbility2 = CrossPlatformInputManager.GetButtonDown("Fire3P1");
            }

            else if (cscheme2)
            {
                m_DoAbility2 = p1.GetButtonDown("LB"); //// m_DoAbility2 = CrossPlatformInputManager.GetButtonDown("LBFire3P1");
            }
        }

        if (m_PlayerStats.health <= 0)
        {
            KillPlayer();
        }

        if (m_Rigidbody2D.gravityScale < 3f && CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            m_Rigidbody2D.gravityScale = 3f;
        }

        if (!reviving)
        {
            if (p1.GetButtonDown("Y") && IsThereDeadAround()) //// if (Input.GetButtonDown("ReviveP1") && IsThereDeadAround())
            {
                reviving = true;
                movement = false;
                reviveTimer += Time.deltaTime;
            }
        }
        if (reviving)
        {
            if (reviveEffectCopy == null)
            {
                reviveEffectCopy = GameObject.Instantiate(reviveEffect);
                reviveEffectCopy.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }
          
            reviveTimer += Time.deltaTime;
            if (reviveTimer >= 3f)
            {
                Destroy(reviveEffectCopy);
                GameObject toRevive = GetNearestDeadPlayer();
                toRevive.GetComponent<PlayerController>().RevivePlayer();
                reviving = false;
            }
        }
        if (!p1.GetButton("Y")) //// if (!Input.GetButton("ReviveP1"))
        {
            Destroy(reviveEffectCopy);
            reviveTimer = 0;
            movement = true;
            reviving = false;
        }
    }
    private void Player2Control()
    {
        m_Horizontal = p2.GetAxis("LS Axis X"); //// m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = p2.GetAxis("LS Axis Y"); //// m_Vertical = Input.GetAxis("Vertical");


        if (!m_Jump)
        {
            if (cscheme1 || cscheme3)
            {// Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            else if (cscheme2)
            {
                if (p2.GetButtonDown("LT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("LTJump") != 0)
                {
                    if (!LTinuse)
                    {
                        LTinuse = true;
                        m_Jump = true;
                    }
                }

                if (!p2.GetButtonDown("LT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("LTJump") == 0)
                {
                    LTinuse = false;

                }

            }

        }

        if (!m_DoAttack)
        {
            if (cscheme1)
            {
                m_DoAttack = CrossPlatformInputManager.GetButtonDown("Fire1");
            }

            else if (cscheme2)
            {
                if (p2.GetButtonDown("RT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("RTFire1") != 0)
                {
                    m_DoAttack = true;
                    //if (!RTinuse)
                    //{
                    //    RTinuse = true;
                    //    m_DoAttack = true;
                    //}
                }
                else
                {
                    m_DoAttack = false;
                }

                if (!p2.GetButtonDown("RT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("RTFire1") == 0)
                {
                    RTinuse = false;
                }
            }

            else if (cscheme3)
            {
                if (CrossPlatformInputManager.GetAxisRaw("RightH") != 0 || CrossPlatformInputManager.GetAxisRaw("RightV") != 0)
                {
                    m_DoAttack = true;
                }
            }
        }

        if (!m_DoAbility1)
        {
            if (cscheme1 || cscheme3)
            {
                m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("Fire2"); //// m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("Fire2");
            }

            else if (cscheme2)
            {
                m_DoAbility1 = p2.GetButtonDown("RB"); //// m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("RBFire2");
            }
        }

        if (!m_DoAbility2)
        {
            if (cscheme1 || cscheme3)
            {
                m_DoAbility2 = CrossPlatformInputManager.GetButtonDown("Fire3P1");
            }

            else if (cscheme2)
            {
                m_DoAbility2 = p2.GetButtonDown("LB"); //// m_DoAbility2 = CrossPlatformInputManager.GetButtonDown("LBFire3P1");
            }
        }

        if (m_PlayerStats.health <= 0)
        {
            KillPlayer();
        }

        if (m_Rigidbody2D.gravityScale < 3f && CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            m_Rigidbody2D.gravityScale = 3f;
        }

        if (!reviving)
        {
            if (p2.GetButtonDown("Y") && IsThereDeadAround()) //// if (Input.GetButtonDown("ReviveP1") && IsThereDeadAround())
            {
                reviving = true;
                movement = false;
                reviveTimer += Time.deltaTime;
            }
        }
        if (reviving)
        {
            if (reviveEffectCopy == null)
            {
                reviveEffectCopy = GameObject.Instantiate(reviveEffect);
                reviveEffectCopy.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }

            reviveTimer += Time.deltaTime;
            if (reviveTimer >= 3f)
            {
                Destroy(reviveEffectCopy);
                GameObject toRevive = GetNearestDeadPlayer();
                toRevive.GetComponent<PlayerController>().RevivePlayer();
                reviving = false;
            }
        }
        if (!p2.GetButton("Y")) //// if (!Input.GetButton("ReviveP1"))
        {
            Destroy(reviveEffectCopy);
            reviveTimer = 0;
            movement = true;
            reviving = false;
        }
    }
    private void Player3Control()
    {
        m_Horizontal = p3.GetAxis("LS Axis X"); //// m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = p3.GetAxis("LS Axis Y"); //// m_Vertical = Input.GetAxis("Vertical");


        if (!m_Jump)
        {
            if (cscheme1 || cscheme3)
            {// Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            else if (cscheme2)
            {
                if (p3.GetButtonDown("LT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("LTJump") != 0)
                {
                    if (!LTinuse)
                    {
                        LTinuse = true;
                        m_Jump = true;
                    }
                }

                if (!p3.GetButtonDown("LT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("LTJump") == 0)
                {
                    LTinuse = false;

                }

            }

        }

        if (!m_DoAttack)
        {
            if (cscheme1)
            {
                m_DoAttack = CrossPlatformInputManager.GetButtonDown("Fire1");
            }

            else if (cscheme2)
            {
                if (p3.GetButtonDown("RT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("RTFire1") != 0)
                {
                    m_DoAttack = true;
                    //if (!RTinuse)
                    //{
                    //    RTinuse = true;
                    //    m_DoAttack = true;
                    //}
                }
                else
                {
                    m_DoAttack = false;
                }

                if (!p3.GetButtonDown("RT"))  //////// if (CrossPlatformInputManager.GetAxisRaw("RTFire1") == 0)
                {
                    RTinuse = false;
                }
            }

            else if (cscheme3)
            {
                if (CrossPlatformInputManager.GetAxisRaw("RightH") != 0 || CrossPlatformInputManager.GetAxisRaw("RightV") != 0)
                {
                    m_DoAttack = true;
                }
            }
        }

        if (!m_DoAbility1)
        {
            if (cscheme1 || cscheme3)
            {
                m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("Fire2"); //// m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("Fire2");
            }

            else if (cscheme2)
            {
                m_DoAbility1 = p3.GetButtonDown("RB"); //// m_DoAbility1 = CrossPlatformInputManager.GetButtonDown("RBFire2");
            }
        }

        if (!m_DoAbility2)
        {
            if (cscheme1 || cscheme3)
            {
                m_DoAbility2 = CrossPlatformInputManager.GetButtonDown("Fire3P1");
            }

            else if (cscheme2)
            {
                m_DoAbility2 = p3.GetButtonDown("LB"); //// m_DoAbility2 = CrossPlatformInputManager.GetButtonDown("LBFire3P1");
            }
        }

        if (m_PlayerStats.health <= 0)
        {
            KillPlayer();
        }

        if (m_Rigidbody2D.gravityScale < 3f && CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            m_Rigidbody2D.gravityScale = 3f;
        }

        if (!reviving)
        {
            if (p3.GetButtonDown("Y") && IsThereDeadAround()) //// if (Input.GetButtonDown("ReviveP1") && IsThereDeadAround())
            {
                reviving = true;
                movement = false;
                reviveTimer += Time.deltaTime;
            }
        }
        if (reviving)
        {
            if (reviveEffectCopy == null)
            {
                reviveEffectCopy = GameObject.Instantiate(reviveEffect);
                reviveEffectCopy.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }

            reviveTimer += Time.deltaTime;
            if (reviveTimer >= 3f)
            {
                Destroy(reviveEffectCopy);
                GameObject toRevive = GetNearestDeadPlayer();
                toRevive.GetComponent<PlayerController>().RevivePlayer();
                reviving = false;
            }
        }
        if (!p3.GetButton("Y")) //// if (!Input.GetButton("ReviveP1"))
        {
            Destroy(reviveEffectCopy);
            reviveTimer = 0;
            movement = true;
            reviving = false;
        }
    }
    private void ResetFixedUpdateValues()
    {
        m_Jump = false;
        m_DoAttack = false;
    }
    private void ResetUpdateValues()
    {
        m_DoAbility1 = false;
        m_DoAbility2 = false;
    }
    private void CombatInteractionHandler()
    {
        // If invincibility on hit is enabled
        if (invincibileOnHit)
        {
            // Continue invincibility timer and return
            if (m_InvincibilityTimer < maxInvincibilityTime)
            {
                m_InvincibilityTimer += Time.deltaTime;
                return;
            }
            // Done being invincible, reset layer back to normal
            else
            {
                m_InvincibilityTimer = maxInvincibilityTime;
            }
        }

        if (m_CombatInteraction.damage > 0)
        {
            if (invincible)
            {
                StartCoroutine(DisplayInvincibility(flashBlock));
                m_CombatInteraction.damage = 0;
                m_CombatInteraction.knockback = 0;
                return;
            }

            ApplyKnockback();

            // Deal the damage
            m_PlayerStats.health -= m_CombatInteraction.damage;
            if(m_PlayerStats.health <= 0)
            {
                PlayDeathLine();
            }
            else
            {
                PlayDamagedSound();
            }
            reviveTimer = 0;

            // Show damage has been taken if player invincibility on hit is disabled
            if (!invincibileOnHit)
            {
                StartCoroutine(DisplayDamageTaken());
            }

            m_InvincibilityTimer = 0;
            StartCoroutine(DisplayInvincibility(flashDamage));

            // ALWAYS RESET VALUES
            m_CombatInteraction.damage = 0;
            m_CombatInteraction.knockback = 0;
        }
    }
    private void ApplyKnockback()
    {
        if (m_CombatInteraction.knockback <= 0)
        {
            return;
        }

        Vector2 dir = m_CombatInteraction.direction.normalized;
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;

        // Knockback in negative or positive x direction a fixed amount
        if (dir.x < 0)
        {
            vel.x = -m_CombatInteraction.knockback;
        }
        else
        {
            vel.x = m_CombatInteraction.knockback;
        }

        // Knockback in positive y direction a fixed amount
        vel.y = m_CombatInteraction.knockback;

        GetComponent<Rigidbody2D>().velocity = vel;

        // ALWAYS RESET VALUES
        m_CombatInteraction.knockback = 0;
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", flashDamage);

        yield return new WaitForSeconds(0.05f);
        if (renderer != null)
        {
            renderer.material.SetColor("_Color", m_MainColor);
        }
    }
    private IEnumerator DisplayInvincibility(Color color)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", color);

        yield return new WaitForSeconds(0.05f);

        if(m_InvincibilityTimer == maxInvincibilityTime)
        {
            if (renderer != null)
            {
                renderer.material.SetColor("_Color", m_MainColor);
            }
            yield break;
        }

        if (color == flashDamage)
        {
            StartCoroutine(DisplayInvincibility(m_MainColor));
        }
        else if (color == m_MainColor)
        {
            StartCoroutine(DisplayInvincibility(flashDamage));
        }
        else
        {
            renderer.material.SetColor("_Color", m_MainColor);
        }
    }
    private void KillPlayer()
    {
        isDead = true;
        m_anim.enabled = false;
        gameObject.tag = "Dead";
        gameObject.layer = LayerMask.NameToLayer("Dead");
        reticle.SetActive(false);

        StopAllCoroutines();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", m_MainColor);
    }
    private void DisplayDeath()
    {
        m_Rigidbody2D.gravityScale = 5f;
        if (transform.rotation.eulerAngles.z < 90)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 10f);
        }
    }
    private void DisplayRevive()
    {
        m_anim.enabled = true;
        isRevived = false;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        transform.transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
        healthbar.SetActive(true);
    }
    private bool IsThereDeadAround()
    {
        bool deadAround = false;
        GameObject[] deadPlayers = GameObject.FindGameObjectsWithTag("Dead");
        if (deadPlayers.Length == 0)
        {
            return false;
        }
        foreach (GameObject dead in deadPlayers)
        {
            if (Vector3.Distance(transform.position, dead.transform.position) <= 1)
            {
                deadAround = true;
            }
        }
        return deadAround;
    }
    private GameObject GetNearestDeadPlayer()
    {
        GameObject[] deadPlayers = GameObject.FindGameObjectsWithTag("Dead");
        if (deadPlayers.Length == 0)
        {
            return null;
        }
        else
        {
            GameObject closestDead = deadPlayers[0];
            float shortestDistance = Vector3.Distance(transform.position, closestDead.transform.position);
            foreach (GameObject dead in deadPlayers)
            {
                if (Vector3.Distance(transform.position, dead.transform.position) <= shortestDistance)
                {
                    closestDead = dead;
                }
            }
            return closestDead;
        }
    }
    private void GetPoisoned()
    {
        m_PlayerStats.maxSpeed /= 2;
        m_PlayerStats.maxJumpForce /= 2;
        alreadyPoisoned = true;
        poisonEffectCopy = GameObject.Instantiate(poisonEffect);
        poisonEffectCopy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        poisonEffectCopy.transform.parent = transform;
    }
    private void GetRecovered()
    {
        m_PlayerStats.maxSpeed = maxSpeed;
        m_PlayerStats.maxJumpForce = maxJumpForce;
        m_PlayerStats.isPoisoned = false;
        alreadyPoisoned = false;
        Destroy(poisonEffectCopy);
    }
    private void PlaySelectedLine()
    {
        if(SelectedLines.Length == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, SelectedLines.Length);
        audioPlayer.clip = SelectedLines[index];
        audioPlayer.Play();
    }
    private void PlayAttackLine()
    {
        if (SelectedLines.Length == 0)
        {
            return;
        }
        int chance = UnityEngine.Random.Range(0, 100);
        if(chance < 10)
        {
            int index = UnityEngine.Random.Range(0, AttackLines.Length);
            audioPlayer.clip = AttackLines[index];
            audioPlayer.Play();
        }
        
    }
    private void PlayDamagedSound()
    {
        if (DamagedSounds.Length == 0)
        {
            return;
        }
        int chance = UnityEngine.Random.Range(0, 100);
        if (chance < 10)
        {
            int index = UnityEngine.Random.Range(0, DamagedSounds.Length);
            audioPlayer.clip = DamagedSounds[index];
            audioPlayer.Play();
        }
    }
    private void PlayDeathLine()
    {
        if (DeathLines.Length == 0)
        {
            return;
        }
        int chance = UnityEngine.Random.Range(0, 100);
        if (chance < 10)
        {
            int index = UnityEngine.Random.Range(0, DeathLines.Length);
            audioPlayer.clip = DeathLines[index];
            audioPlayer.Play();
        }
    }
    private void PlayDeselectedLine()
    {
        if (DeselectedLines.Length == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, DeselectedLines.Length);
        audioPlayer.clip = DeselectedLines[index];
        audioPlayer.Play();
    }
    private void PlayAbility1Line()
    {
        if (Ability1Lines.Length == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, Ability1Lines.Length);
        audioPlayer.clip = Ability1Lines[index];
        audioPlayer.Play();
    }
    private void PlayAbility2Line()
    {
        if (Ability2Lines.Length == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, Ability2Lines.Length);
        audioPlayer.clip = Ability2Lines[index];
        audioPlayer.Play();
    }
}