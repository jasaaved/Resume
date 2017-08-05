using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private PlayerStats stats;
    private float currentHealth;
    private float maxHealth;
    private String characterClass;
    private Slider _slider;
    private GameObject HB;
    private PlayerMovement movement;
    private Vector3 BarScale;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
        characterClass = this.name;
        HB = transform.FindChild("HealthBar").gameObject;
        BarScale = HB.transform.localScale;

        _slider = GameObject.Find(characterClass + "HealthBar/Slider").GetComponent<Slider>();
    }

    void Update()
    {
        maxHealth = stats.maxHealth;
        currentHealth = stats.health;

        _slider.value = 100 - (currentHealth / maxHealth * 100);

        if (currentHealth <= 0)
        {
            HB.SetActive(false);
        }

        if (!movement.facingRight && BarScale.x > 0)
        {
            FlipBar();
        }
        else if (movement.facingRight && BarScale.x < 0)
        {
            FlipBar();
        }
    }

    private void FlipBar()
    {
        BarScale.x *= -1;
        HB.transform.localScale = BarScale;
    }
}