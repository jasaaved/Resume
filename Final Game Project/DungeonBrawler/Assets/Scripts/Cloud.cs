using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
    private GameObject wizard;
    private GameObject lightning;
    private float timer;
    private bool spawn_lightning;
    private bool done;
    public float cooldown = 3f;
    

	// Use this for initialization
	void Start () {
        wizard = transform.parent.gameObject;
        transform.parent = null;
        lightning = transform.FindChild("LightningBolt").gameObject;
        lightning.transform.FindChild("LightningEnd").GetComponent<LightningBolt>().m_Player = wizard;
        lightning.SetActive(false);
        timer = 0.3f;
        transform.position = wizard.transform.position;
        spawn_lightning = false;
        done = false;
        wizard.GetComponent<PlayerController>().maxAbility2Cooldown = cooldown;
        wizard.GetComponent<PlayerController>().ability2Cooldown = 0;

    }

    void FixedUpdate()
    {
        if (!spawn_lightning)
        {
            transform.position =  new Vector2(transform.position.x, transform.position.y + 0.4f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer <= 0 && !spawn_lightning)
        {
            spawn_lightning = true;
        }

        if (spawn_lightning)
        {
            Spawn();
        }

		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            spawn_lightning = true;
        }
    }


    void Spawn()
    {
        if (done)
        {
            
            return;
        }
        
        lightning.SetActive(true);
        done = true;


    }
}
