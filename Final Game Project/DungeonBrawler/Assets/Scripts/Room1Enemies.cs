using UnityEngine;
using System.Collections;

public class Room1Enemies : MonoBehaviour {

    public int numberOfEnemies = 10;

    private GameObject warrior;
    private GameObject rogue;
    private GameObject wizard;
    private GameObject doorLeft;
    private bool doReset = true;

    void Start()
    {
        warrior = GameObject.Find("Warrior");
        rogue = GameObject.Find("Rogue");
        wizard = GameObject.Find("Wizard");
        doorLeft = GameObject.Find("DoorLeft");
    }

	void FixedUpdate ()
    {
	    if(numberOfEnemies <= 0)
        {
            if(doorLeft.transform.position.y < 8)
            {
                doorLeft.transform.position = new Vector2(doorLeft.transform.position.x, doorLeft.transform.position.y + 0.1f);
            }
            else if (doorLeft.transform.position.y >= 8)
            {
                Destroy(this);
            }

            if (doReset)
            {
                warrior.GetComponent<PlayerController>().ResetPlayer();
                rogue.GetComponent<PlayerController>().ResetPlayer();
                wizard.GetComponent<PlayerController>().ResetPlayer();

                doReset = false;
            }
        }
	}

}
