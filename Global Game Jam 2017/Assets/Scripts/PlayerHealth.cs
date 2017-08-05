﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int enemiesInRange;
    public int maxEnemiesInRange;
    public int health;
    public int maxHealth;
    public GameObject explosionParticle;
    public AudioClip hurtSound;


	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void CheckDeath()
    {
        if(health <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        CheckDeath();
    }

    public void Death()
    {
        Camera.main.GetComponent<CameraShaking>().Shake(0.3f, 0.3f);

        // Switch to ragdoll form
        // Need to destroy previous collider and rigidbody
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<Rigidbody>());
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "Scaler")
            {
                // Destroy scalar (need to destroy, otherwise enemies still follow it)
                Destroy(transform.GetChild(i).gameObject);
            }
            if (transform.GetChild(i).gameObject.name == "Ragdoll")
            {
                // Activate ragdool
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        //Instantiate(explosionParticle, transform.position, Quaternion.identity);
        GameManager.Instance.GameOver();
        if (hurtSound)
        {
            AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position);
        }
        //TODO: DEATH STUFF
       // Destroy(gameObject);
    }
}
