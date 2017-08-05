using UnityEngine;
using System.Collections;

public class RoomSwitch : MonoBehaviour {

    private GameObject warrior;
    private GameObject rogue;
    private GameObject wizard;
    private GameObject doorLeft;
    private bool moveDoor = false;

    // Use this for initialization
    void Start ()
    {
        warrior = GameObject.Find("Warrior");
        rogue = GameObject.Find("Rogue");
        wizard = GameObject.Find("Wizard");
        doorLeft = GameObject.Find("DoorLeft");
    }
	
	void FixedUpdate ()
    {
        if(moveDoor)
        {
            if(doorLeft.transform.position.y > 0)
            {
                doorLeft.transform.position = new Vector2(doorLeft.transform.position.x, doorLeft.transform.position.y - 0.1f);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            warrior.transform.position = new Vector2(-30, 0);
            rogue.transform.position = new Vector2(-30, 0);
            wizard.transform.position = new Vector2(-30, 0);

            Camera.main.transform.Translate(-52, 0, 0);
            moveDoor = true;
        }
    }
}
