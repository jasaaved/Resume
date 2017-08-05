using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterBarUIController : MonoBehaviour
{
    public GameObject player;
    public Image weapon;
    public Image ability1;
    public Image ability2;

    private PlayerController playerController;
    private float weaponCooldown;
    private float ability1Cooldown;
    private float ability2Cooldown;
    private float maxWeaponCooldown;
    private float maxAbility1Cooldown;
    private float maxAbility2Cooldown;

    void Start()
    {
        if (gameObject.name == "WarriorBar")
        {
            player = GameObject.Find("Warrior");

            if (player == null)
            {
                player = GameObject.Find("Warrior(Clone)");
            }
        }
        else if (gameObject.name == "RogueBar")
        {
            player = GameObject.Find("Rogue");

            if (player == null)
            {
                player = GameObject.Find("Rogue(Clone)");
            }
        }
        else if (gameObject.name == "WizardBar")
        {
            player = GameObject.Find("Wizard");

            if (player == null)
            {
                player = GameObject.Find("Wizard(Clone)");
            }
        }

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }
    void Update()
    {
        if (player == null)
        {
            if (gameObject.name == "WarriorBar")
            {
                player = GameObject.Find("Warrior");

                if (player == null)
                {
                    player = GameObject.Find("Warrior(Clone)");
                }
            }
            else if (gameObject.name == "RogueBar")
            {
                player = GameObject.Find("Rogue");

                if (player == null)
                {
                    player = GameObject.Find("Rogue(Clone)");
                }
            }
            else if (gameObject.name == "WizardBar")
            {
                player = GameObject.Find("Wizard");

                if (player == null)
                {
                    player = GameObject.Find("Wizard(Clone)");
                }
            }

            playerController = player.GetComponent<PlayerController>();
        }

        if(playerController == null)
        {
            return;
        }

        if (playerController.isDead)
        {
            weapon.fillAmount = 0;
            ability1.fillAmount = 0;
            ability2.fillAmount = 0;
            return;
        }

        weaponCooldown = playerController.weaponCooldown;
        ability1Cooldown = playerController.ability1Cooldown;
        ability2Cooldown = playerController.ability2Cooldown;
        maxWeaponCooldown = playerController.maxWeaponCooldown;
        maxAbility1Cooldown = playerController.maxAbility1Cooldown;
        maxAbility2Cooldown = playerController.maxAbility2Cooldown;

        weapon.fillAmount = weaponCooldown / maxWeaponCooldown;
        ability1.fillAmount = ability1Cooldown / maxAbility1Cooldown;
        ability2.fillAmount = ability2Cooldown / maxAbility2Cooldown;
    }
}
