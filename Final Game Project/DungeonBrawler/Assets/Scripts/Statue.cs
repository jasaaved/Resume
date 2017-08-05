using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour {
    private bool warrior;
    private bool rogue;
    private bool wizard;
    public bool on;
    private GameObject light;
    private GameObject particles;

	// Use this for initialization
	void Start () {
        if (transform.name == "WarriorStatue")
        {
            particles = transform.FindChild("WarriorParticle").gameObject;
            light = transform.FindChild("WarriorLight").gameObject;
            warrior = true;
            rogue = false;
            wizard = false;
        }

        if (transform.name == "RogueStatue")
        {
            particles = transform.FindChild("RogueParticle").gameObject;
            light = transform.FindChild("RogueLight").gameObject;
            warrior = false;
            rogue = true;
            wizard = false;
        }

        if (transform.name == "WizardStatue")
        {
            particles = transform.FindChild("WizardParticle").gameObject;
            light = transform.FindChild("WizardLight").gameObject;
            warrior = false;
            rogue = false;
            wizard = true;
        }

        particles.SetActive(false);
        light.SetActive(false);       


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((warrior && other.name == "Warrior") || (warrior && other.name == "Warrior(Clone)"))
        {
            particles.SetActive(true);
            light.SetActive(true);
            on = true;
        }

        if ((rogue && other.name == "Rogue") || (rogue && other.name == "Rogue(Clone)"))
        {
            particles.SetActive(true);
            light.SetActive(true);
            on = true;
        }

        if ((wizard && other.name == "Wizard") || (wizard && other.name == "Wizard(Clone)"))
        {
            particles.SetActive(true);
            light.SetActive(true);
            on = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((warrior && other.name == "Warrior") || (warrior && other.name == "Warrior(Clone)"))
        {
            particles.SetActive(false);
            light.SetActive(false);
            on = false;
        }

        if ((rogue && other.name == "Rogue") || (rogue && other.name == "Rogue(Clone)"))
        {
            particles.SetActive(false);
            light.SetActive(false);
            on = false;
        }

        if ((wizard && other.name == "Wizard") || (wizard && other.name == "Wizard(Clone)"))
        {
            particles.SetActive(false);
            light.SetActive(false);
            on = false;
        }
    }

}
