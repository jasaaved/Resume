using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehavior : MonoBehaviour
{

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public bool moving;
    public bool attacking;
    public bool invincible;
    public float chargeSpeed = 10f;
    public float maxPaceDistance = 7;
    public float minPaceDistance = 2;
    public float pacePauseTime = 2; // seconds
    public float chargePauseTime = 1; // seconds
    public float maxChargeDistancePastPlayer = 5;
    public Color vulnerableColor = Color.yellow;
    public Color aggroColor = Color.red;
    public Color damageTakenColor = Color.red;

    private GameObject targetObject;
    private Transform groundCheck;
    private Transform wallCheck;
    private Transform ground;
    private Transform wall;
    private EnemyStats enemyStats;
    private AggroTrigger aggroTrigger;
    private CombatInteraction combatInteraction;
    private Animator animator;
    private Color mainColor;
    private Vector2 newPosition;
    private float overlapRadius = 0.2f; // Radius of the overlap circle to determine if hitting ground or a wall
    private float currentPacePauseTime;
    private float currentChargePauseTime;
    private float xScale;
    public bool pacing;
    public bool charging;
    private bool goLeft;
    private bool leftBoundReached;
    private bool rightBoundReached;
    private Rigidbody2D m_Rigidbody2D;

    private float alpha;
    private Color color;
    private float currentRemainTime;
    public float fadeTime = 2f;
    private SpriteRenderer m_Renderer;
    public bool isDead;


    /////////////////////////////////////////////////////////////////
    // PROPERTIES
    /////////////////////////////////////////////////////////////////
    private bool Dead()
    {
        return gameObject.GetComponent<EnemyStats>().health <= 0;
    }
    private bool Stunned()
    {
        return combatInteraction.stunned;
    }
    private bool OffScreen()
    {
        return !GetComponent<Renderer>().isVisible;
    }
    private bool PacePaused()
    {
        return currentPacePauseTime <= pacePauseTime;
    }
    private bool ChargePaused()
    {
        return currentChargePauseTime <= chargePauseTime;
    }
    public void SetInvincible(bool isInvincible)
    {
        invincible = isInvincible;

        if (!invincible)
        {
            mainColor = vulnerableColor;
            SetColor(vulnerableColor);
        }
    }
    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroTrigger = GetComponentInChildren<AggroTrigger>();
        combatInteraction = GetComponent<CombatInteraction>();
        enemyStats = GetComponent<EnemyStats>();
        mainColor = GetComponent<SpriteRenderer>().color;
        groundCheck = transform.FindChild("GroundCheck");
        wallCheck = transform.FindChild("WallCheck");
        m_Renderer = GetComponent<SpriteRenderer>();
        isDead = false;

        moving = false;
        pacing = false;
        attacking = false;
        invincible = true;
        currentPacePauseTime = pacePauseTime;
        xScale = transform.localScale.x;
    }
    void Update()
    {
        if (!isDead)
        {
            // Disable if off screen
            if (OffScreen())
            {
                return;
            }
            if (Dead())
            {
                isDead = true;
                currentRemainTime = fadeTime;
            }
            if (Stunned())
            {
                return;
            }

            targetObject = aggroTrigger.targetObject;

            CombatInteractionHandler();
            Movement();
        }
        else
        {
            fadingOut();
            DisplayDeath();
        }
    }


    /////////////////////////////////////////////////////////////////
    // METHODS
    /////////////////////////////////////////////////////////////////
    private void Movement()
    {
        if (aggroTrigger.isAggroed || charging)
        {
            SetColor(aggroColor);
            AggressiveMovement();
        }
        else
        {
            SetColor(mainColor);
            PassiveMovement();
        }

        AnimateMovement();
    }
    private void PassiveMovement()
    {
        charging = false;

        if (PacePaused())
        {
            currentPacePauseTime += Time.deltaTime;
            moving = false;
            pacing = false;
            return;
        }

        FlipHorizontally();

        // If there is no ground below
        if (!GroundFound())
        {
            // Stop the gameObject's movement
            currentPacePauseTime = 0;
            moving = false;
            pacing = false;

            // Determine what side the boundary is on
            if (transform.position.x > groundCheck.position.x)
            {
                leftBoundReached = true;
            }
            else if (transform.position.x < groundCheck.position.x)
            {
                rightBoundReached = true;
            }
            
            return;
        }
        // If the gameObject has hit a wall
        else if (WallFound())
        {
            // Stop the gameObject's movement
            currentPacePauseTime = 0;
            moving = false;
            pacing = false;

            // Determine what side the boundary is on
            if (transform.position.x > wallCheck.position.x)
            {
                leftBoundReached = true;
            }
            else if (transform.position.x < wallCheck.position.x)
            {
                rightBoundReached = true;
            }
            
            return;
        }
        else
        {
            moving = true;

            if (!pacing)
            {
                StartPacing();
            }
            else
            {
                PaceLeftOrRight();
            }
        }
    }
    private void FlipHorizontally()
    {
        // If the left boundary has been reached, face right
        if (leftBoundReached)
        {
            goLeft = false;
        }
        // If the right boundary has been reached, face left
        else if (rightBoundReached)
        {
            goLeft = true;
        }

        // Flip the gameObject accordingly
        if (goLeft)
        {
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        }
    }
    private void StartPacing()
    {
        // Pick a random distance to pace
        int paceDistance = (int) Random.Range(minPaceDistance, maxPaceDistance);

        // If the left boundary has been reached, go right
        if (leftBoundReached)
        {
            goLeft = false;
            leftBoundReached = false;
        }
        // If the right boundary has been reached, go left
        else if (rightBoundReached)
        {
            goLeft = true;
            paceDistance = -paceDistance;
            rightBoundReached = false;
        }
        // Otherwise, 50/50 chance of going left or right
        else
        {
            float chanceToGoLeft = Random.value * 100;
            if (chanceToGoLeft <= 50)
            {
                goLeft = true;
                paceDistance = -paceDistance;
            }
            else
            {
                goLeft = false;
            }
        }

        newPosition = new Vector2(transform.position.x + paceDistance, transform.position.y);
        pacing = true;
    }
    private void PaceLeftOrRight()
    {
        if (goLeft)
        {
            Vector2 movePosition = new Vector2(transform.position.x - enemyStats.speed * Time.deltaTime, transform.position.y);
            GetComponent<Rigidbody2D>().MovePosition(movePosition);

            // If the new position is reached
            if (transform.position.x <= newPosition.x)
            {
                // Start pause time
                currentPacePauseTime = 0;
            }
        }
        else
        {
            Vector2 movePosition = new Vector2(transform.position.x + enemyStats.speed * Time.deltaTime, transform.position.y);
            GetComponent<Rigidbody2D>().MovePosition(movePosition);

            // If the new position is reached
            if (transform.position.x >= newPosition.x)
            {
                // Start pause time
                currentPacePauseTime = 0;
            }
        }
    }
    private void AggressiveMovement()
    {
        pacing = false;

        if (ChargePaused())
        { 
            // Stop x velocity
            Vector2 vel = GetComponent<Rigidbody2D>().velocity;
            vel.x = 0;
            GetComponent<Rigidbody2D>().velocity = vel;

            currentChargePauseTime += Time.deltaTime;
            moving = false;
            charging = false;
            return;
        }

        FlipHorizontally();

        // If there is no ground below
        if (!GroundFound())
        {
            // Stop the gameObject's movement
            currentChargePauseTime = 0;
            moving = false;
            charging = false;

            // Determine what side the boundary is on
            if (transform.position.x > groundCheck.position.x)
            {
                leftBoundReached = true;
            }
            else if (transform.position.x < groundCheck.position.x)
            {
                rightBoundReached = true;
            }

            return;
        }
        // If the gameObject has hit a wall
        else if (WallFound())
        {
            // Stop the gameObject's movement
            currentChargePauseTime = 0;
            moving = false;
            charging = false;

            // Determine what side the boundary is on
            if (transform.position.x > wallCheck.position.x)
            {
                leftBoundReached = true;
            }
            else if (transform.position.x < wallCheck.position.x)
            {
                rightBoundReached = true;
            }

            return;
        }
        else
        {
            moving = true;

            if (!charging)
            {
                StartCharging();
            }
            else
            {
                ChargeLeftOrRight();
            }
        }
    }
    private void StartCharging()
    {
        float golemPosX = transform.position.x;
        float targetPosX = targetObject.transform.position.x;
        float distanceFromTarget = Mathf.Abs(golemPosX - targetPosX);
        float chargeDistance = distanceFromTarget + maxChargeDistancePastPlayer;

        // Reset bounds reached, since the gameObject will change direction
        if (leftBoundReached)
        {
            leftBoundReached = false;
            goLeft = false;
        }
        else if (rightBoundReached)
        {
            rightBoundReached = false;
            chargeDistance = -chargeDistance;
            goLeft = true;
        }
        else if (golemPosX > targetPosX)
        {
            chargeDistance = -chargeDistance;
            goLeft = true;
        }
        else if (golemPosX < targetPosX)
        {
            goLeft = false;
        }

        newPosition = new Vector2(transform.position.x + chargeDistance, transform.position.y);
        charging = true;
    }
    private void ChargeLeftOrRight()
    {
        // Set the velocity
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;

        if (goLeft)
        {
            vel.x = -chargeSpeed;
        }
        else
        {
            vel.x = chargeSpeed;
        }

        GetComponent<Rigidbody2D>().velocity = vel;

        if (goLeft && transform.position.x <= newPosition.x)
        {
            // Start pause time
            currentChargePauseTime = 0;
        }
        else if (!goLeft && transform.position.x >= newPosition.x)
        {
            // Start pause time
            currentChargePauseTime = 0;
        }
    }
    private bool GroundFound()
    {
        bool groundFound = false;

        // The gameObject is grounded if a circlecast to the groundcheck position hits anything designated as "Ground"
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, overlapRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Ground" || colliders[i].gameObject.tag == "Platform")
            {
                groundFound = true;
            }
        }

        return groundFound;
    }
    private bool WallFound()
    {
        bool wallFound = false;

        // The gameObject is grounded if a circlecast to the wallCheck position hits anything designated as "Ground"
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, overlapRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            // Walls are also tagged "Ground" as of yet, this may change in the future
            if (colliders[i].gameObject.tag == "Ground")
            {
                wallFound = true;
            }
        }

        return wallFound;
    }
    private void CombatInteractionHandler()
    {
        if (combatInteraction.damage > 0)
        {
            if (!invincible)
            {
                // Deal the damage
                enemyStats.health -= combatInteraction.damage;
                GameObject blood = Resources.Load<GameObject>("Prefabs/BloodSplatter");
                ParticleSystem ps = blood.GetComponent<ParticleSystem>();
                var col = ps.colorOverLifetime;
                Gradient grad = new Gradient();
                grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
                col.color = grad;
                GameObject.Instantiate(blood, transform.position, transform.rotation);

                // Show damage has been taken
                StartCoroutine(DisplayDamageTaken());
            }
        }

        // ALWAYS RESET VALUES
        combatInteraction.damage = 0;
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", damageTakenColor);

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.material.SetColor("_Color", mainColor);
        }
    }
    private void AnimateMovement()
    {
        if (moving)
        {
            animator.SetFloat("speed", 1.0f);
        }
        else
        {
            animator.SetFloat("speed", 0.0f);
        }
    }
    private void fadingOut()
    {
        m_Rigidbody2D.velocity = Vector2.zero;

        currentRemainTime -= Time.deltaTime;

        if (currentRemainTime <= 0f)
        {
            Destroy(this.gameObject);
            return;
        }

        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;
    }
    private void DisplayDeath()
    {
        if (transform.rotation.eulerAngles.z < 90)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 10f);
        }
    }
}
