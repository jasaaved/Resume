using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    [SerializeField] public float maxSpeed;         // The fastest the player can travel in the x axis.
    public bool facingRight = true;                       // For determining which way the player is currently facing.

    [SerializeField] private float m_JumpForce;    // Amount of force added when the player jumps.
    [SerializeField] private LayerMask m_WhatIsGround;    // A mask determining what is ground to the character
    private Transform m_GroundCheck;                      // A position marking where to check if the player is grounded.
    private const float k_GroundedRadius = .2f;           // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;                              // Whether or not the player is grounded.
    private const float k_CeilingRadius = .01f;           // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;                              // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private float m_Horizontal;
    private float m_Vertical;
    private PlayerStats stats;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        maxSpeed = stats.maxSpeed;
        m_JumpForce = stats.maxJumpForce;
    }

    void Update()
    {
        maxSpeed = stats.maxSpeed;
        m_JumpForce = stats.maxJumpForce;
    }

    void FixedUpdate()
    {
        OnGround();

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }


    /////////////////////////////////////////////////////////////////
    // METHODS
    /////////////////////////////////////////////////////////////////
    public void MovePlayer(float horizontal, float vertical, bool jump)
    {
        m_Horizontal = horizontal;
        m_Vertical = vertical;

        //only control the player if grounded or airControl is turned on
        if (m_Grounded)
        {
            if (Mathf.Abs(vertical) < 0.9)
            {
                // Move the character
                m_Rigidbody2D.velocity = new Vector2(horizontal * maxSpeed, m_Rigidbody2D.velocity.y);
                m_Anim.SetFloat("Speed", Mathf.Abs(horizontal));
            }
            else if (Mathf.Abs(vertical) >= 0.9)
            {
                m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                m_Anim.SetFloat("Speed", Mathf.Abs(0));
            }

            // If the input is moving the player right and the player is facing left...
            if (horizontal > 0 && !facingRight)
            {
                FlipPlayer();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontal < 0 && facingRight)
            {
                FlipPlayer();
            }
        }

        if (!m_Grounded)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(horizontal));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(horizontal * maxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (horizontal > 0 && !facingRight)
            {
                FlipPlayer();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontal < 0 && facingRight)
            {
                FlipPlayer();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.gravityScale = 0f;    
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

    }
    private void OnGround()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Ground" || colliders[i].gameObject.tag == "Platform")
            {
                m_Grounded = true;
            }
        }
        m_Anim.SetBool("Ground", m_Grounded);
    }
    private void FlipPlayer()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

