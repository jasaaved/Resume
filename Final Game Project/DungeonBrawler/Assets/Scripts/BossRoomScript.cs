using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScript : MonoBehaviour
{

    private GameObject gamemanager;
    private GameObject requirementbox;
    private int layermask = ~(1 << 0 | 1 << 12 | 1 << 20);

    void Start()
    {
        gamemanager = GameObject.Find("GameManager").gameObject;
        requirementbox = this.transform.FindChild("RequirementBox").gameObject;
        requirementbox.SetActive(false);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Enable enemies in a room as soon as the player enters a room
        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)" && gamemanager.GetComponent<GameManager>().key_found  && !gamemanager.GetComponent<GameManager>().InCombat) //shield throw hotfix
        {
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)" && !gamemanager.GetComponent<GameManager>().key_found && !gamemanager.GetComponent<GameManager>().InCombat) //shield throw hotfix
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, other.transform.position, layermask);

            if (hit.transform != null && hit.transform.tag == "Player")
            {
                requirementbox.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)" && !gamemanager.GetComponent<GameManager>().key_found && !gamemanager.GetComponent<GameManager>().InCombat) //shield throw hotfix
        {
            requirementbox.SetActive(false);
        }
    }


}

