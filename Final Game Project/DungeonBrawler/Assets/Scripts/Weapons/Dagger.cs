using UnityEngine;
using System.Collections;

public class Dagger : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public WeaponStats weaponStats;

    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Update()
    {
        if (weaponStats.projectileLifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
        weaponStats.projectileLifeSpan -= Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            CombatInteraction combatInteraction = other.GetComponent<CombatInteraction>();

            if (combatInteraction != null)
            {
                combatInteraction.damage += weaponStats.damage;
                combatInteraction.knockback += weaponStats.knockback;
                combatInteraction.direction = gameObject.transform.right;
            }

            Destroy(gameObject);
        }
        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.tag == "Ground")
        {
            Destroy(gameObject);
        }
        if (other.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }

}
