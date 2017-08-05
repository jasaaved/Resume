using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lasso : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public float lifespan = 2f;
    public float range = 1.5f;
    public float cooldown = 5f;
    public float pullSpeed = 15f;
    public float animateSpeed = 10f;
    public float lineWidth = 1.5f;
    public Material material;

    private GameObject player;
    private GameObject otherObject;
    private GameObject loop;
    private int originalColliderLayer;
    private bool attached;
    private bool returnLasso = false;
    private Collider2D otherObjCollider;
    private Rigidbody2D otherObjRigidbody;
    private CombatInteraction otherObjCombatInteraction;
    private Vector2 catchPoint;
    private Vector2 nextDrawPos;

    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR METHODS
    /////////////////////////////////////////////////////////////////
    void Awake ()
    {
        if (transform.parent != null && transform.parent.gameObject != null)
        {
            player = transform.parent.gameObject;
        }
        else
        {
            player = GameObject.Find("Rogue");

            if (player == null)
            {
                player = GameObject.Find("Rogue(Clone)");
            }
        }

        // Sets the OnItem2ConsecutiveEvent to be Detach. This makes it possible for the player to
        // detach the lasso when consecutively hitting the Item2 button
        UnityEvent onItem2ConsecEvent = player.GetComponent<PlayerController>().OnAbility2ConsecutiveEvent;
        onItem2ConsecEvent.AddListener(Detach);

        // Set the position based on the reticle
        GameObject reticle = player.transform.FindChild("Reticle").gameObject;
        float angle = reticle.GetComponent<ReticleMovement>().angle;
        float posx = reticle.transform.position.x;
        float posy = reticle.transform.position.y;

        transform.position = new Vector2(Mathf.Cos(angle) * range + posx, Mathf.Sin(angle) * range + posy);
        transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

        // Set cooldown for player and disable player movement while "throwing" lasso
        player.GetComponent<PlayerController>().maxAbility2Cooldown = cooldown;
        player.GetComponent<PlayerController>().ability2Cooldown = 0;
        player.GetComponent<PlayerController>().movement = false;
        
        // Values used for drawing the lasso
        catchPoint = transform.FindChild("CatchPoint").position;
        nextDrawPos = player.transform.position;
        loop = transform.FindChild("Loop").gameObject;
        loop.transform.position = player.transform.position;

        // Disable the player's movement
        player.GetComponent<PlayerController>().movement = false;

        // Disable parent until attached to an object
        transform.parent = null;
    }
    void Update()
    {
        if (player == null)
        {
            Destroy(gameObject);
        }

        if (attached)
        {
            // Re-enable parent as player
            transform.parent = player.transform;

            // Change scale and position to match player. This needs to happen
            // so that the DistanceJoint2D acts as if it's attached to the player.
            Vector2 scale = transform.localScale;
            scale.x = player.transform.localScale.x;
            transform.position = player.transform.position;

            loop.SetActive(false);

            // If the other object has a non-trigger collider, make it so that the object
            // will fall through platforms anytime the player is below the other object
            if (otherObjCollider != null)
            {
                if (otherObject.transform.position.y > player.transform.position.y + 0.5f)
                {
                    otherObjCollider.gameObject.layer = LayerMask.NameToLayer("Phase");
                }
                else
                {
                    otherObjCollider.gameObject.layer = originalColliderLayer;
                }
            }

            float playerPosY = player.transform.position.y;
            float otherObjPosY = otherObject.transform.position.y;
            float playerVelY = player.GetComponent<Rigidbody2D>().velocity.y;

            // Make it so that the other object with follow the player
            // vertically anytime the player jumps or dashes
            if (playerPosY > otherObjPosY && playerVelY > 10f)
            {
                float velY = playerVelY - (playerVelY * 0.25f);
                otherObjRigidbody.velocity = new Vector2(0f, velY);
            }
            else
            {
                otherObjRigidbody.velocity = new Vector2(0f, otherObjRigidbody.velocity.y);
            }

            DrawLassoToObject(otherObject);
            return;
        }

        if (returnLasso)
        {
            // Return the lasso inwards, starting from the "catch point" and moving back towards the player
            if (player != null)
            {
                nextDrawPos = Vector2.MoveTowards(nextDrawPos, player.transform.position, animateSpeed * Time.deltaTime);

                if (nextDrawPos == (Vector2)player.transform.position)
                {
                    Detach();
                }
            }
        }
        else
        {
            // Draw the lasso outwards, starting from the player and moving towards the "catch point"
            nextDrawPos = Vector2.MoveTowards(nextDrawPos, catchPoint, animateSpeed * Time.deltaTime);
        }

        if (nextDrawPos == catchPoint)
        {
            returnLasso = true;
        }

        if (player != null)
        {
            DrawLassoMovement(player.transform.position, nextDrawPos);
        }
    }
    void OnDestroy()
    {
        if (player != null && player.GetComponent<LineRenderer>() != null)
        {
            Destroy(player.GetComponent<LineRenderer>());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (attached)
        {
            return;
        }

        switch (other.tag)
        {
            case "Enemy":
            case "Player":
            case "Dead":
                if (other.gameObject.name != "Rogue" && other.gameObject.name != "Rogue(Clone)")
                {
                    Attach(other);
                }
                break;
            default:
                break;
        }
    }

    /////////////////////////////////////////////////////////////////
    // PUBLIC METHODS
    /////////////////////////////////////////////////////////////////
    // Made public because the player can re-activate the item again while
    // attached to detach before the lifespan ends
    public void Detach()
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().movement = true;
            Destroy(player.GetComponent<LineRenderer>());
        }
        if (otherObject != null)
        {
            if (otherObjCollider != null)
            {
                otherObjCollider.gameObject.layer = originalColliderLayer;
            }

            if (otherObjCombatInteraction != null)
            {
                otherObjCombatInteraction.disabled = false;
            }

            otherObject.GetComponent<CombatInteraction>().stunned = false;
            GetComponent<DistanceJoint2D>().connectedBody = null;
        }
        if (gameObject != null)
        {
            Destroy(gameObject);
        }

        attached = false;
    }

    /////////////////////////////////////////////////////////////////
    // PRIVATE METHODS
    /////////////////////////////////////////////////////////////////
    private void Attach(Collider2D other)
    {
        // Save values of the attached object
        otherObject = other.gameObject;
        otherObjRigidbody = DBUtils.GetRigidbodyOfObject(otherObject);
        otherObjCollider = DBUtils.GetNonTriggerColliderOfObject(otherObject);
        otherObjCombatInteraction = DBUtils.GetCombatInteractionOfObject(otherObject);
        if (otherObjCollider != null)
        {
            originalColliderLayer = otherObjCollider.gameObject.layer;
        }

        // If the attached object is immune to CC then detach
        if (otherObject.GetComponent<CombatInteraction>().immuneToCC)
        {
            Detach();
            return;
        }
        
        // Stun the other object
        otherObject.GetComponent<CombatInteraction>().stunned = true;

        // Re-enable player movement
        player.GetComponent<PlayerController>().movement = true;

        // Attach the other object to the joint
        GetComponent<DistanceJoint2D>().connectedBody = otherObjRigidbody;
        attached = true;

        // Disable any combat interaction for the other object (don't want it dying so easy!)
        if (otherObjCombatInteraction != null)
        {
            otherObjCombatInteraction.disabled = true;
        }

        // Detach at the end of the lifespan
        Invoke("Detach", lifespan);
    }
    private void DrawLassoMovement(Vector2 startPos, Vector2 nextPos)
    {
        LineRenderer line = player.GetComponent<LineRenderer>();

        if (line == null)
        {
            line = player.AddComponent<LineRenderer>();
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.numPositions = 2;
            line.material = material;
            line.GetComponent<Renderer>().enabled = true;
        }

        line.SetPosition(0, startPos);
        line.SetPosition(1, nextPos);

        if (!attached)
        {
            loop.SetActive(true);
            loop.transform.position = nextPos;
        }
    }
    private void DrawLassoToObject(GameObject obj)
    {
        LineRenderer line = player.GetComponent<LineRenderer>();

        if (line == null)
        {
            line = player.AddComponent<LineRenderer>();
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.numPositions = 2;
            line.material = material;
            line.GetComponent<Renderer>().enabled = true;
        }
        
        line.SetPosition(0, player.transform.position);
        line.SetPosition(1, obj.transform.position);

        if (!attached)
        {
            loop.SetActive(true);
            loop.transform.position = obj.transform.position;
        }
    }

}
