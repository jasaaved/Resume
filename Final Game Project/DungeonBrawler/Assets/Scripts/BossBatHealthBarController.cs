using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossBatHealthBarController : MonoBehaviour
{
    private GameObject bossbat;
    private EnemyStats stats;
    private float currentHealth;
    private float maxHealth;
    private String characterClass;
    private Slider _slider;
    private GameObject HB;
    private PlayerMovement movement;
    private Vector3 BarScale;

    void Start()
    {
        bossbat = transform.parent.FindChild("Enemies/BossBat").gameObject;
        stats = bossbat.GetComponent<EnemyStats>();
        _slider = transform.FindChild("Slider").gameObject.GetComponent<Slider>();

        HB = this.gameObject;
        maxHealth = stats.health;
        currentHealth = maxHealth;
    }

    void Update()
    {
        currentHealth = stats.health;

        _slider.value = 100 - (currentHealth / maxHealth * 100);

        if (currentHealth <= 0)
        {
            HB.SetActive(false);
        }
    }

}