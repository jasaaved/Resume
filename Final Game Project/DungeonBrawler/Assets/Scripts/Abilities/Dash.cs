using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Dash : MonoBehaviour
{
    public float damage = 15f;
    public float cooldown = 1f;
    public float speed;
    public float length;
    private float angle;

    private GameObject player;
    private Vector3 direction;
    private float h, v;
    private int playerNumber;

    void Start()
    {
        // Set values
        player = transform.parent.gameObject;
        playerNumber = player.GetComponent<PlayerController>().playerNumber;
        player.GetComponent<PlayerController>().ability1Cooldown = 0;
        player.GetComponent<PlayerController>().maxAbility1Cooldown = cooldown;

        // Make position the same as the player
        transform.position = player.transform.position;

        Activate();
    }
    void FixedUpdate()
    {
        player.GetComponent<Rigidbody2D>().velocity = direction;     // Keep the velocity in direction constant while dash is alive
    }
    void Activate()
    {
        // Allow Player to pass through platforms and set the lifespan of the dash
        this.transform.parent.gameObject.layer = LayerMask.NameToLayer("Phase");
        Destroy(this.gameObject, length);

        // Calculate the trajectory of the dash
        /*
        switch (playerNumber)
        {
            case 1:
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                break;
            case 2:
                h = Input.GetAxis("HorizontalP2");
                v = Input.GetAxis("VerticalP2");
                break;
            case 3:
                h = Input.GetAxis("HorizontalP3");
                v = Input.GetAxis("VerticalP3");
                break;
        }
        */
        angle = getReticleAngle() * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed, 0f);

        // Set the Max Speed of Player Movement to speed of Dash (So dash is not slowed horizontally)
        player.GetComponent<PlayerMovement>().maxSpeed = speed;
    }
    void OnDestroy()
    {
        player.GetComponent<PlayerMovement>().maxSpeed = 10f;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.layer = LayerMask.NameToLayer("Players");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<CombatInteraction>().damage += damage;
        }
    }

    public float getReticleAngle()
    {
        for(int i = 0; i < player.transform.childCount; i++)
        {
            if(player.transform.GetChild(i).name == "Reticle")
            {
                GameObject reticle = player.transform.GetChild(i).gameObject;
                return reticle.transform.rotation.eulerAngles.z;
            }
        }
        return 0;
    }

}