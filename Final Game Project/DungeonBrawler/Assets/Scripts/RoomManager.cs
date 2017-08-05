using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public bool doors_open;

    private Vector3 velocity = Vector3.zero;
    public bool move_camera = false;
    public bool set_key;
    private GameObject enemies_folder;
    private GameObject doors_folder;
    private GameObject minimap_blips;
    private GameObject warrior;
    private GameObject wizard;
    private GameObject rogue;
    private GameObject gamemanager;
    private GameObject key;
    private GameObject teleport;
    private SoundManager soundManager;
    private bool did_already;

    void lockDoors()
    {
        doors_folder.SetActive(true);
    }
    void Awake()
    {
        doors_open = false;

        // These need to be set in Awake, otherwise there is a null ref exception in OnTriggerEnter
        enemies_folder = transform.FindChild("Enemies").gameObject;
        doors_folder = transform.FindChild("Doors").gameObject;
        minimap_blips = transform.FindChild("MinimapBlips").gameObject;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        gamemanager = GameObject.Find("GameManager");
    }
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
        //minimap_blips.SetActive(false);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Enable enemies in a room as soon as the player enters a room
        if(other.gameObject.tag == "Player"  && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)" && enemies_folder.transform.childCount != 0) //shield throw hotfix
        {
            //Enable camera movement to smoothly transition to the new room
            move_camera = true; //put this back when done testing
            enemies_folder.SetActive(true);
            minimap_blips.SetActive(true);
            gamemanager.GetComponent<GameManager>().InCombat = true;
            PlaySound();


            Vector3 target;

            if (other.gameObject.transform.position.y <= transform.position.y-2)
            {
                target = Vector3.MoveTowards(other.transform.position, transform.position, 2.5f);
                Invoke("lockDoors", 0.5f);
            }

            else
            {
                //Try putting the movement in a function instead, and lock the doors first then move the players
                target = Vector3.MoveTowards(other.transform.position, transform.position, 1f); //These numbers may need tweaking
                lockDoors();
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

        if (other.gameObject.tag == "Player" && other.transform.name != "BasicShield" && other.transform.name != "ShieldThrow" && other.transform.name != "BasicShield(Clone)" && other.transform.name != "ShieldThrow(Clone)" && enemies_folder.transform.childCount == 0) //shield throw hotfix
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.Find("Enemies").gameObject.SetActive(false);
            move_camera = false;
        }
    }

    /*
    void OnTriggerStay2D(Collider2D other)
    {
        if (enemies_folder.transform.childCount != 0)
        {
            doors_folder.SetActive(true);

        }

        if (enemies_folder.transform.childCount <= 0)
        {
            doors_folder.SetActive(false);
        }
    }
    */

    void LateUpdate(){
        if (move_camera){
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z-10), ref velocity, 0.25f);
            if (Vector3.Distance(Camera.main.transform.position,new Vector3(transform.position.x, transform.position.y, transform.position.z - 10)) <= 0.001f){
                move_camera = false;
            }
        }
    }

    void Update()
    {
        if (enemies_folder.transform.childCount <= 0 && !did_already)
        {
            did_already = true;
            doors_folder.SetActive(false);
            set_key = gamemanager.GetComponent<GameManager>().Get_Key();

            if (set_key)
            {
                key.SetActive(true);
            }

            gamemanager.GetComponent<GameManager>().InCombat = false;


        }
    }

    public void PlaySound()
    {
        if (ContainsEnemy("BossBat"))
        {
            soundManager.KyleBossBatLine();
        }
        if(ContainsEnemy("DreadKnight"))
        {
            soundManager.KyleDreadKnightLine();
        }
        float chance = Random.Range(0f, 1f);

        if(chance <= 0.49)
        {
            return;
        }
        
        int enemyType = Random.Range(0, 100);
        if(ContainsEnemy("Slime") && enemyType < 33)
        {
            soundManager.KyleSlimeLine();
        }
        else if(ContainsEnemy("Knight") && enemyType < 66 && enemyType >= 33)
        {
            soundManager.KyleKnightLine();
        }
        else if(ContainsEnemy("Golem") && enemyType < 99 && enemyType >= 66)
        {
            soundManager.KyleGolemLine();
        }
    }
    public bool ContainsEnemy(string name)
    {
        for(int i = 0; i < enemies_folder.transform.childCount; i++)
        {
            if(enemies_folder.transform.GetChild(i).name.Contains(name))
            {
                return true;
            }
        }
        return false;
    }

}
