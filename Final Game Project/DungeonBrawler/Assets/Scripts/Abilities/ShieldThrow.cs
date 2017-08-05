using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldThrow : MonoBehaviour {

    public GameObject shield;
    public float throwSpeed;
    public float range;
    public float cooldown = 3f;

    private GameObject warriorReticle;
    private Vector3 startPosition;
    private Rigidbody2D body;
    private Vector3 direction;
    private bool returning;
    private GameObject warrior;

    private void Start()
    {
        transform.parent = null;
        warrior = GameObject.Find("Warrior(Clone)");
        if (warrior == null)
        {
            warrior = GameObject.Find("Warrior");
        }

        warriorReticle = GameObject.Find("Warrior(Clone)");

        if (warriorReticle == null)
        {
            warriorReticle = GameObject.Find("Warrior").transform.FindChild("Reticle").gameObject;
        }
        else
        {
            warriorReticle = GameObject.Find("Warrior(Clone)").transform.FindChild("Reticle").gameObject; ;
        }

        body = GetComponent<Rigidbody2D>();
        transform.position = warriorReticle.transform.position;
        Vector3 rotation = warriorReticle.transform.rotation.eulerAngles;
        rotation.z += 90;
        transform.eulerAngles = rotation;
        returning = false;
        startPosition = transform.position;
        warrior.GetComponent<PlayerController>().ability2Cooldown = 0;
        warrior.GetComponent<PlayerController>().maxAbility2Cooldown = cooldown;

        throwSpeed = 45f;
        range = 30f;
        direction = new Vector3(Mathf.Cos(warriorReticle.transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(warriorReticle.transform.eulerAngles.z * Mathf.Deg2Rad), 0);
        //basicShield = Resources.Load<GameObject>("Prefabs/Items/Warrior/BasicShield");
    }

    private void Update()
    {
        if(returning)
        {
            Return();
            body.velocity = Vector3.zero;
        }
        else
        {
            body.velocity = direction * throwSpeed;
        }

        if(Vector3.Distance(startPosition, transform.position) >= range)
        {
            returning = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && (collision.name == "Wizard" || collision.name == "Rogue"  || collision.name == "Wizard(Clone)" || collision.name == "Rogue(Clone)"))
        {
            //Instantiate BasicShield making the ally the parent of the shield.
            GameObject.Instantiate(shield, collision.transform);
            Destroy(this.gameObject);
        }
        if((collision.name == "Warrior" || collision.name == "Warrior(Clone)") && returning)
        {
            Destroy(this.gameObject);
        }
        if(collision.name == "Bottom" || collision.name == "Top" || collision.name == "Left" || collision.name == "Right")
        {
            returning = true;
        }
    }

    void Return()
    {
        /*
        if (this.transform.position.x > warrior.transform.position.x)
        {
            Vector2 target = new Vector2(warrior.transform.position.x + 0.5f, warrior.transform.position.y);
            this.transform.position = Vector2.MoveTowards(this.transform.position, target, throwSpeed * Time.deltaTime);
        }
        else
        {
            Vector2 target = new Vector2(warrior.transform.position.x - 0.5f, warrior.transform.position.y);
            this.transform.position = Vector2.MoveTowards(this.transform.position, target, throwSpeed * Time.deltaTime);
        }
        */
        if (warrior != null)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, warrior.transform.position, throwSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
