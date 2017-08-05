using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    public WeaponStats weaponStats;
    public float destroy_time;
    public bool lightning_bolt;
    public Vector2 direction; // This will be changed by the Fireball direction

    void Start()
    {
        weaponStats = GetComponent<WeaponStats>();
        Destroy(this.gameObject, destroy_time);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            CombatInteraction combatInteraction = other.GetComponent<CombatInteraction>();

            if (combatInteraction != null && weaponStats != null)
            {
                combatInteraction.damage += weaponStats.damage;
                combatInteraction.knockback += weaponStats.knockback;
                combatInteraction.direction = direction;
            }
        }

        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);
        }

        if (lightning_bolt && other.tag == "Dead")
        {
            if (other.GetComponent<PlayerController>().isDead == true)
            {
                other.GetComponent<PlayerController>().LightningRevivePlayer();
            }
        }



    }
}
