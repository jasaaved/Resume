using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBullet : MonoBehaviour{

    private float angle;
    private Rigidbody2D m_RigidBody;
    private ParticleSystem particles;
    private SpriteRenderer m_SpriteRenderer;
    private float aliveTimer;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        angle = transform.rotation.eulerAngles.z;
        m_RigidBody.velocity = AngleToVelocity();
        aliveTimer = 0;
    }

    private void Update()
    {
        particles.startColor = LowerAlpha();
        m_SpriteRenderer.color = LowerAlpha();
        if(particles.startColor.a <= 0)
        {
            Destroy(this.gameObject);
        }
        aliveTimer += Time.deltaTime;
    }

    private Vector3 AngleToVelocity()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(x, y, 0) * 15;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(this.name.Contains("Bullet"))
            {
                collision.gameObject.GetComponent<CombatInteraction>().damage = 0.5f;
            }
            else if(this.name.Contains("Lob"))
            {
                collision.gameObject.GetComponent<CombatInteraction>().damage = 3f;
            }
            Destroy(this.gameObject);
        }
    }

    private Color LowerAlpha()
    {
        Color start = particles.startColor;
        float alpha = 1 - (aliveTimer / 10f);
        //float confirm = Time.deltaTime / 10;
        //alpha -= Time.deltaTime / 9;
        Color newColor = new Color(start.r, start.g, start.b, alpha);
        return newColor;
    }

}
