using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPillar : MonoBehaviour {

    private bool read;
    private bool attacking;
    private BoxCollider2D col;
    public Material[] materials;
    public float damage;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
        read = true;
        attacking = false;
    }

    private void Update()
    {
        if(read)
        {
            ReduceXScaleRead();
        }
        if(attacking)
        {
            ReduceXScaleAttack();
            if(transform.localScale.x <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        if(transform.localScale.x <= 0)
        {
            read = false;
            Attack();
        }
    }


    private void ReduceXScaleRead()
    {
        Vector3 scale = transform.localScale;
        scale.x -= Time.deltaTime;
        transform.localScale = scale;
    }

    private void ReduceXScaleAttack()
    {
        Vector3 scale = transform.localScale;
        scale.x -= Time.deltaTime * 8;
        transform.localScale = scale;
    }

    private void Attack()
    {
        attacking = true;
        col.enabled = true;
        this.GetComponent<Renderer>().material = materials[1]; 
        Vector3 scale = transform.localScale;
        scale.x = 3;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CombatInteraction>().damage = damage;
        }
    }

}
