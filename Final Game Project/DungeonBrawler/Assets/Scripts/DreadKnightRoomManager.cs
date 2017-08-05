using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadKnightRoomManager : MonoBehaviour {
    public bool doors_open;
    private Vector3 velocity = Vector3.zero;
    public bool move_camera = false;
    public bool set_key;
    private GameObject enemies_folder;
    private GameObject doors_folder;
    private GameObject minimap_blips;
    private GameObject boss_healthbar;
    private GameObject warrior;
    private GameObject wizard;
    private GameObject rogue;
    private GameObject gamemanager;
    private GameObject key;
    private GameObject wallhazard;
    private GameObject teleport;
    private SoundManager soundManager;
    private bool did_already;

    void Awake()
    {
        doors_open = false;
        enemies_folder = transform.FindChild("Enemies").gameObject;
        doors_folder = transform.FindChild("Doors").gameObject;
        minimap_blips = transform.FindChild("MinimapBlips").gameObject;
        boss_healthbar = transform.FindChild("DKHealthBar").gameObject;
        wallhazard = transform.FindChild("WallHazard").gameObject;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        gamemanager = GameObject.Find("GameManager");
    }
    // Use this for initialization
    void Start () {
        warrior = GameObject.Find("Warrior(Clone)");

        if (warrior != null)
        {
            rogue = GameObject.Find("Rogue(Clone)");
            wizard = GameObject.Find("Wizard(Clone)");
        }

        if (warrior == null)
        {
            warrior = GameObject.Find("Warrior");
            rogue = GameObject.Find("Rogue");
            wizard = GameObject.Find("Wizard");
        }
        did_already = false;


        Transform childKey = transform.FindChild("Key");

        if (childKey != null)
        {
            key = childKey.gameObject;
            key.SetActive(false);
        }


        //Disable all enemies in all rooms on start
        enemies_folder.SetActive(false);
        doors_folder.SetActive(false);
        wallhazard.SetActive(false);
        boss_healthbar.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        if (enemies_folder.transform.childCount <= 0 && !did_already)
        {
            doors_folder.SetActive(false);
            gamemanager.GetComponent<GameManager>().InCombat = false;
            did_already = true;
            gamemanager.GetComponent<GameManager>().WinScreen();

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)") //shield throw hotfix
        {
            move_camera = true;
            minimap_blips.SetActive(true);

            Vector3 target;

            if (other.gameObject.transform.position.y <= transform.position.y - 2)
            {
                target = Vector3.MoveTowards(other.transform.position, transform.position, 2.5f);
            }

            else
            {
                //Try putting the movement in a function instead, and lock the doors first then move the players
                target = Vector3.MoveTowards(other.transform.position, transform.position, 1f);
            }

            if (warrior != null && rogue != null && wizard != null)
            {
                warrior.transform.position = target;
                rogue.transform.position = target;
                wizard.transform.position = target;
                warrior.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                wizard.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                rogue.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            teleport = GameObject.Find("Teleport(Clone)");

            if (teleport == null)
            {
                teleport = GameObject.Find("Teleport");
            }

            if (teleport != null)
            {
                teleport.transform.position = target;
                Destroy(teleport);
            }
        }



    }

    void LateUpdate()
    {
        if (move_camera)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 10), ref velocity, 0.25f);
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 10)) <= 0.001f)
            {
                move_camera = false;
            }
        }
    }

    public void lockDoors()
    {
        doors_folder.SetActive(true);
        enemies_folder.SetActive(true);
        wallhazard.SetActive(true);
        boss_healthbar.SetActive(true);
        gamemanager.GetComponent<GameManager>().InCombat = true;
    }
}
