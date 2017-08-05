using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public string restartScene;
    public GameObject gameOverText;
    public GameObject restartingText;
    public GameObject pressAText;
    private GameObject levelmanager;
    public GameObject minimap;
    public GameObject winScreenUI;
    public GameObject pauseMenuUI;
    public EventSystem eventSystem;
    public GameObject winScreenFirstSelected;
    public float percentage;
    public float chance;
    public bool key_found;
    public bool DK_key_found;
    public bool warriorIsDead;
    public bool rogueIsDead;
    public bool wizardIsDead;
    public bool InCombat;
    public bool winState = false;

    private GameObject warrior;
    private GameObject wizard;
    private GameObject rogue;
    private bool restarting = false;

    void Awake()
    {
        levelmanager = GameObject.Find("LevelGenerator");
        percentage = 1f / levelmanager.GetComponent<LevelGenerator>().num_rooms;
    }

    void Start()
    {
        warrior = GameObject.Find("Warrior");

        if (warrior == null)
        {
            warrior = GameObject.Find("Warrior(Clone)");
            rogue = GameObject.Find("Rogue(Clone)");
            wizard = GameObject.Find("Wizard(Clone)");
        }

        else
        {
            rogue = GameObject.Find("Rogue");
            wizard = GameObject.Find("Wizard");
        }


        chance = -percentage;
        key_found = false;
        DK_key_found = false;
        InCombat = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            //If we are running in a standalone build of the game
            #if UNITY_STANDALONE
            //Quit the application
            Application.Quit();
            #endif

            //If we are running in the editor
            #if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        if (InCombat)
        {
            minimap.SetActive(false);
        }

        if (!InCombat && minimap.activeSelf == false)
        {
            minimap.SetActive(true);
        }

        warriorIsDead = warrior.GetComponent<PlayerController>().isDead;
        rogueIsDead = rogue.GetComponent<PlayerController>().isDead;
        wizardIsDead = wizard.GetComponent<PlayerController>().isDead;

        if (warriorIsDead && rogueIsDead && wizardIsDead)
        {
            Text resTextComp = restartingText.GetComponent<Text>();

            if (!gameOverText.activeSelf && !pressAText.activeSelf && !restartingText.activeSelf)
            {
                gameOverText.SetActive(true);
                pressAText.SetActive(true);
                restartingText.SetActive(true);
            }

            if (Input.GetButtonDown("A") && !restarting)
            {
                pressAText.SetActive(false);
                StartCoroutine(RestartGame());
                resTextComp.text = "Restarting...";
                restarting = true;
            }
            if (restarting)
            {
                // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
                resTextComp.color = new Color(resTextComp.color.r, resTextComp.color.g, resTextComp.color.b, Mathf.PingPong(Time.time, 1));
            }


        }


    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator RestartGame()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(restartScene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public bool Get_Key()
    {
        if (!key_found)
        {
            chance += percentage;
            if (Random.value > (1 - chance))
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        else
        {
            return false;
        }
    }

    public void WinScreen()
    {
        winState = true;
        winScreenUI.SetActive(true);
        eventSystem.firstSelectedGameObject = winScreenFirstSelected;
        winScreenUI.GetComponent<ShowPanels>().ShowMenu();
        pauseMenuUI.SetActive(false);
    }

}
