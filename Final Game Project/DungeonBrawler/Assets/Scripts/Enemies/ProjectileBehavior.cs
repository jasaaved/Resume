using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {

    private float damage;
    private Vector3 direction;
    private Rigidbody2D projectile;

    void Start()
    {
        damage = 5f; //GetComponentInParent<EnemyStats>().damage;
        direction = this.transform.eulerAngles;
        projectile = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        projectile.velocity = Rotation2Velocity() * 10f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Shield")
        {
            other.GetComponent<CombatInteraction>().damage = damage;
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

    Vector3 Rotation2Velocity()
    {
        float angle = this.transform.eulerAngles.z * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        return new Vector3(x, y, 0f);
    }
}
