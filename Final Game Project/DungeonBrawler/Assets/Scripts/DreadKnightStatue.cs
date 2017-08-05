using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadKnightStatue : MonoBehaviour {
    private GameObject WarriorStatue;
    private GameObject RogueStatue;
    private GameObject WizardStatue;

	// Use this for initialization
	void Start () {
        WarriorStatue = transform.parent.transform.FindChild("WarriorStatue").gameObject;
        RogueStatue = transform.parent.transform.FindChild("RogueStatue").gameObject;
        WizardStatue = transform.parent.transform.FindChild("WizardStatue").gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        if (WizardStatue.GetComponent<Statue>().on && WarriorStatue.GetComponent<Statue>().on && RogueStatue.GetComponent<Statue>().on)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
            
            if (transform.localPosition.y >= 0)
            {
                transform.root.GetComponent<DreadKnightRoomManager>().lockDoors();
                Destroy(transform.parent.gameObject);
            }
        }

        if (!WizardStatue.GetComponent<Statue>().on || !WarriorStatue.GetComponent<Statue>().on || !RogueStatue.GetComponent<Statue>().on)
        {
            if (transform.localPosition.y > -2)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.1f);
            }
        }


    }
}
