using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is solely for turnin on boss-room adjacency on the minimap

public class MinimapBossRoom : MonoBehaviour {

    private GameObject minimap_blips;

    void Awake()
    {
        minimap_blips = transform.parent.transform.parent.transform.FindChild("MinimapBlips").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Enable enemies in a room as soon as the player enters a room
        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)") //shield throw hotfix
        {
            //Enable camera movement to smoothly transition to the new room
            minimap_blips.SetActive(true);
            //Now the script sets the AdjacencyChecker to false to avoid any unanticipated performance issues
            transform.parent.gameObject.SetActive(false);
        }
    }
}
