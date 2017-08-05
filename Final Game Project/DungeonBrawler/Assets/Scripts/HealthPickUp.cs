using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour {

    public float healthRestore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if(playerStats.health == playerStats.maxHealth)
            {
                return;
            }
            else if(playerStats.health < playerStats.maxHealth)
            {
                playerStats.health += healthRestore;
                if(playerStats.health > playerStats.maxHealth)
                {
                    playerStats.health = playerStats.maxHealth;
                }
                Destroy(this.gameObject);
            }
        }
    }

}
