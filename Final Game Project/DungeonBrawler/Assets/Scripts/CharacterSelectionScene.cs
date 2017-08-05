using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using Rewired;

//Most of the following code was found online at this URL, in a tutorial format: http://blog.teamtreehouse.com/make-loading-screen-unity

public class CharacterSelectionScene : MonoBehaviour
{
    // UI
    public Text loadingText;
    public GameObject backPrompt;
    public GameObject startImage;
    public GameObject selectWarriorPrompt;
    public GameObject cancelWarriorPrompt;
    public GameObject selectRoguePrompt;
    public GameObject cancelRoguePrompt;
    public GameObject selectWizardPrompt;
    public GameObject cancelWizardPrompt;

    public string nextScene;
    public string previousScene;
    public GameObject Warrior;
    public GameObject Rogue;
    public GameObject Wizard;
    public bool isP1Selected = false;
    public bool isP2Selected = false;
    public bool isP3Selected = false;
    public bool isWarriorSelected = false;
    public bool isRogueSelected = false;
    public bool isWizardSelected = false;
    public float intervalForFlash = 0.1f;
    public float intervalForSelect = 0.1f;
    public float intervalForReady = 0.1f;
    public bool readyForNext = false;
    public bool readyContinue = false;

    private float WarriorPositionP1 = -17f;
    private float WarriorPositionP2 = -16f;
    private float WarriorPositionP3 = -15f;
    private float RoguePositionP1 = -1f;
    private float RoguePositionP2 = 0.0f;
    private float RoguePositionP3 = 1f;
    private float WizardPositionP1 = 15f;
    private float WizardPositionP2 = 16f;
    private float WizardPositionP3 = 17f;
    private float[] P1Position = new float[3];
    private float[] P2Position = new float[3];
    private float[] P3Position = new float[3];
    private Vector3 P1pos;
    private Vector3 P2pos;
    private Vector3 P3pos;
    private int P1posCounter = 0;
    private int P2posCounter = 1;
    private int P3posCounter = 2;
    private bool loadingScene = false;
    private bool isSelectingP1 = false;
    private bool isSelectingP2 = false;
    private bool isSelectingP3 = false;
    private float flashTime;
    private float selectTime;
    private float readyTime;
    private GameObject P1;
    private GameObject P2;
    private GameObject P3;
    private Vector3 scale = new Vector3(1.25f, 1.25f, 1);
    private ParticleSystem warriorParticle;
    private ParticleSystem rogueParticle;
    private ParticleSystem wizardParticle;
    private Light warriorLight;
    private Light rogueLight;
    private Light wizardLight;
    private Light doorLight;
    private float moveSelectThreshold = 0.5f;
    private bool optionsMenuOpen = false;

    private Player p1;
    private Player p2;
    private Player p3;

    private void Awake()
    {
        warriorParticle = GameObject.Find("WarriorParticle").GetComponent<ParticleSystem>();
        rogueParticle = GameObject.Find("RogueParticle").GetComponent<ParticleSystem>();
        wizardParticle = GameObject.Find("WizardParticle").GetComponent<ParticleSystem>();
        warriorLight = GameObject.Find("WarriorLight").GetComponent<Light>();
        rogueLight = GameObject.Find("RogueLight").GetComponent<Light>();
        wizardLight = GameObject.Find("WizardLight").GetComponent<Light>();
        doorLight = GameObject.Find("DoorLight").GetComponent<Light>();

        p1 = ReInput.players.GetPlayer(0);
        p2 = ReInput.players.GetPlayer(1);
        p3 = ReInput.players.GetPlayer(2);
    }
    private void Start()
    {
        flashTime = Time.time;
        selectTime = Time.time;
        P1 = GameObject.Find("P1");
        P2 = GameObject.Find("P2");
        P3 = GameObject.Find("P3");

        optionsMenuOpen = false;
        loadingText.GetComponent<Text>().enabled = false;
        startImage.SetActive(false);

        P1Position[0] = WarriorPositionP1;
        P1Position[1] = RoguePositionP1;
        P1Position[2] = WizardPositionP1;
        P2Position[0] = WarriorPositionP2;
        P2Position[1] = RoguePositionP2;
        P2Position[2] = WizardPositionP2;
        P3Position[0] = WarriorPositionP3;
        P3Position[1] = RoguePositionP3;
        P3Position[2] = WizardPositionP3;

        P1pos = P1.transform.position;
        P2pos = P2.transform.position;
        P3pos = P3.transform.position;

        P1pos.x = P1Position[P1posCounter];
        P2pos.x = P2Position[P2posCounter];
        P3pos.x = P3Position[P3posCounter];

        P1.transform.position = P1pos;
        P2.transform.position = P2pos;
        P3.transform.position = P3pos;
        DisableEffects();
    }
    private void Update()
    {
        PlayerCheck();
        StatueController();
        GetNextSceneReady();
        SelectCharacters();
        LoadNextScene();
        OnPauseButtonPressed();
    }

    private bool P1HitSelect()
    {
        return p1.GetButtonDown("A");
    }
    private bool P2HitSelect()
    {
        return p2.GetButtonDown("A");
    }
    private bool P3HitSelect()
    {
        return p3.GetButtonDown("A");
    }
    private bool P1HitCancel()
    {
        return p1.GetButtonDown("B");
    }
    private bool P2HitCancel()
    {
        return p2.GetButtonDown("B");
    }
    private bool P3HitCancel()
    {
        return p3.GetButtonDown("B");
    }

    public void RemovePlayer(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == "Warrior(Clone)" ||
                parent.transform.GetChild(i).name == "Rogue(Clone)" ||
                parent.transform.GetChild(i).name == "Wizard(Clone)")
            {
                //GameObject toDestroy = parent.transform.GetChild(i).gameObject;
                Destroy(parent.transform.GetChild(i).gameObject);
                return;
            }
        }
    }
    public void SelectCharacters()
    {
        if (loadingScene)
        {
            return;
        }

        // Manage for time of Selecting pointers
        if (Time.time > selectTime)
        {
            if (isSelectingP1)
            {
                isSelectingP1 = false;
            }
            if (isSelectingP2)
            {
                isSelectingP2 = false;
            }
            if (isSelectingP3)
            {
                isSelectingP3 = false;
            }

            selectTime += intervalForSelect;
        }

        // Manage for selecting
        if (p1.GetAxis("LS Axis X") > moveSelectThreshold && !isSelectingP1 && !isP1Selected)
        {
            P1posCounter++;
            if (P1posCounter > 2)
            {
                P1posCounter = 0;
            }
            P1pos.x = P1Position[P1posCounter];
            P1.transform.position = P1pos;
            isSelectingP1 = true;
        }
        else if (p1.GetAxis("LS Axis X") < -moveSelectThreshold && !isSelectingP1 && !isP1Selected)
        {
            P1posCounter--;
            if (P1posCounter < 0)
            {
                P1posCounter = 2;
            }
            P1pos.x = P1Position[P1posCounter];
            P1.transform.position = P1pos;
            isSelectingP1 = true;
        }

        if (p2.GetAxis("LS Axis X") > moveSelectThreshold && !isSelectingP2 && !isP2Selected)
        {
            P2posCounter++;
            if (P2posCounter > 2)
            {
                P2posCounter = 0;
            }
            P2pos.x = P2Position[P2posCounter];
            P2.transform.position = P2pos;
            isSelectingP2 = true;
        }
        else if (p2.GetAxis("LS Axis X") < -moveSelectThreshold && !isSelectingP2 && !isP2Selected)
        {
            P2posCounter--;
            if (P2posCounter < 0)
            {
                P2posCounter = 2;
            }
            P2pos.x = P2Position[P2posCounter];
            P2.transform.position = P2pos;
            isSelectingP2 = true;
        }

        if (p3.GetAxis("LS Axis X") > moveSelectThreshold && !isSelectingP3 && !isP3Selected)
        {
            P3posCounter++;
            if (P3posCounter > 2)
            {
                P3posCounter = 0;
            }
            P3pos.x = P3Position[P3posCounter];
            P3.transform.position = P3pos;
            isSelectingP3 = true;
        }
        else if (p3.GetAxis("LS Axis X") < -moveSelectThreshold && !isSelectingP3 && !isP3Selected)
        {
            P3posCounter--;
            if (P3posCounter < 0)
            {
                P3posCounter = 2;
            }
            P3pos.x = P3Position[P3posCounter];
            P3.transform.position = P3pos;
            isSelectingP3 = true;
        }

        if (P1HitSelect() || P2HitSelect() || P3HitSelect())
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }
        }

        // Manage for Enter
        if (P1HitSelect() && !isP1Selected)
        {
            switch (P1posCounter)
            {
                case 0:
                    if(!isWarriorSelected)
                    {
                        isP1Selected = true;
                        isWarriorSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Warrior, P1.transform);
                        tempPlayer.transform.position = new Vector2(P1.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 1;
                        P1.GetComponent<Renderer>().enabled = true;
                    }
                    break;
                case 1:
                    if(!isRogueSelected)
                    {
                        isP1Selected = true;
                        isRogueSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Rogue, P1.transform);
                        tempPlayer.transform.position = new Vector2(P1.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 1;
                        P1.GetComponent<Renderer>().enabled = true;    
                    }
                    break;
                case 2:
                    if (!isWizardSelected)
                    {
                        isP1Selected = true;
                        isWizardSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Wizard, P1.transform);
                        tempPlayer.transform.position = new Vector2(P1.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 1;
                        P1.GetComponent<Renderer>().enabled = true;
                    }
                    break;
            }
        }
        if (P2HitSelect() && !isP2Selected)
        {
            switch (P2posCounter)
            {
                case 0:
                    if (!isWarriorSelected)
                    {
                        isP2Selected = true;
                        isWarriorSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Warrior, P2.transform);
                        tempPlayer.transform.position = new Vector2(P2.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 2;
                        P2.GetComponent<Renderer>().enabled = true;
                    }
                    break;
                case 1:
                    if (!isRogueSelected)
                    {
                        isP2Selected = true;
                        isRogueSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Rogue, P2.transform);
                        tempPlayer.transform.position = new Vector2(P2.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 2;
                        P2.GetComponent<Renderer>().enabled = true;
                    }
                    break;
                case 2:
                    if (!isWizardSelected)
                    {
                        isP2Selected = true;
                        isWizardSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Wizard, P2.transform);
                        tempPlayer.transform.position = new Vector2(P2.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 2;
                        P2.GetComponent<Renderer>().enabled = true;
                    }
                    break;
            }
        }
        if (P3HitSelect() && !isP3Selected)
        {
            switch (P3posCounter)
            {
                case 0:
                    if (!isWarriorSelected)
                    {
                        isP3Selected = true;
                        isWarriorSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Warrior, P3.transform);
                        tempPlayer.transform.position = new Vector2(P3.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 3;
                        P3.GetComponent<Renderer>().enabled = true;
                    }
                    break;
                case 1:
                    if (!isRogueSelected)
                    {
                        isP3Selected = true;
                        isRogueSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Rogue, P3.transform);
                        tempPlayer.transform.position = new Vector2(P3.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 3;
                        P3.GetComponent<Renderer>().enabled = true;
                    }
                    break;
                case 2:
                    if (!isWizardSelected)
                    {
                        isP3Selected = true;
                        isWizardSelected = true;
                        GameObject tempPlayer = GameObject.Instantiate(Wizard, P3.transform);
                        tempPlayer.transform.position = new Vector2(P3.transform.position.x, -2.7f);
                        tempPlayer.GetComponent<PlayerController>().playerNumber = 3;
                        P3.GetComponent<Renderer>().enabled = true;
                    }
                    break;
            }

        }

        if (P1HitCancel() || P2HitCancel() || P3HitCancel())
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }
        }

        // Manage for Cancel
        if (P1HitCancel() && isP1Selected)
        {
            isP1Selected = false;

            switch (P1posCounter)
            {
                case 0:
                    RemovePlayer(P1);
                    break;
                case 1:
                    RemovePlayer(P1);
                    break;
                case 2:
                    RemovePlayer(P1);
                    break;
            }
        }
        if (P2HitCancel() && isP2Selected)
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }

            isP2Selected = false;

            switch (P2posCounter)
            {
                case 0:
                    RemovePlayer(P2);
                    break;
                case 1:
                    RemovePlayer(P2);
                    break;
                case 2:
                    RemovePlayer(P2);
                    break;
            }
        }
        if (P3HitCancel() && isP3Selected)
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }

            isP3Selected = false;

            switch (P3posCounter)
            {
                case 0:
                    RemovePlayer(P3);
                    break;
                case 1:
                    RemovePlayer(P3);
                    break;
                case 2:
                    RemovePlayer(P3);
                    break;
            }
        }

        UpdateWarriorSelectPrompt();
        UpdateRogueSelectPrompt();
        UpdateWizardSelectPrompt();
        UpdateBackPrompt();
        GoBackCheck();
    }
    public void PlayerCheck()
    {
        GameObject tempWarrior = GameObject.Find("Warrior(Clone)");
        GameObject tempRogue = GameObject.Find("Rogue(Clone)");
        GameObject tempWizard = GameObject.Find("Wizard(Clone)");
        if (tempWarrior == null)
        {
            isWarriorSelected = false;
        }
        else
        {
            isWarriorSelected = true;
        }
        if (tempRogue == null)
        {
            isRogueSelected = false;
        }
        else
        {
            isRogueSelected = true;
        }
        if (tempWizard == null)
        {
            isWizardSelected = false;
        }
        else
        {
            isWizardSelected = true;
        }
    }
    public void GetNextSceneReady()
    {
        if(isP1Selected && isP2Selected && isP3Selected &&
            isWarriorSelected && isWizardSelected && isRogueSelected)
        {
            readyContinue = true;
            startImage.SetActive(true);
            doorLight.range = 10f;
        }
        else
        {
            readyContinue = false;
            startImage.SetActive(false);
            doorLight.range = 0;
        }
    }
    public void LoadNextScene()
    {
        if (Input.GetButtonDown("Start") && !loadingScene && readyContinue)
        {
            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadingScene = true;

            // ...change the instruction text to read "Loading..."
            loadingText.GetComponent<Text>().enabled = true;

            // set playernumbers on player stats
            switch (P1posCounter)
            {
                case 0:
                    PlayerStats.warriorNum = 1;
                    break;
                case 1:
                    PlayerStats.rogueNum = 1;
                    break;
                case 2:
                    PlayerStats.wizardNum = 1;
                    break;
            }

            switch (P2posCounter)
            {
                case 0:
                    PlayerStats.warriorNum = 2;
                    break;
                case 1:
                    PlayerStats.rogueNum = 2;
                    break;
                case 2:
                    PlayerStats.wizardNum = 2;
                    break;
            }

            switch (P3posCounter)
            {
                case 0:
                    PlayerStats.warriorNum = 3;
                    break;
                case 1:
                    PlayerStats.rogueNum = 3;
                    break;
                case 2:
                    PlayerStats.wizardNum = 3;
                    break;
            }

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());
        }

        // If the new scene has started loading...
        if (loadingScene == true)
        {
            loadingText.enabled = true;
            startImage.SetActive(false);
        }
    }
    public void GoBackCheck()
    {
        if (!isWarriorSelected && !isRogueSelected && !isWizardSelected)
        {
            if (P1HitCancel() || P2HitCancel() || P3HitCancel())
            {
                if (optionsMenuOpen)
                {
                    optionsMenuOpen = false;
                    return;
                }

                SceneManager.LoadScene(previousScene);
            }
        }
    }
    public void StatueController()
    {
        if(isWarriorSelected)
        {
            warriorParticle.Play();
            warriorParticle.Emit(1);
            warriorLight.range = 5;
        }
        else if(!isWarriorSelected)
        {
            warriorParticle.Stop();
            warriorLight.range = 0;
        }
        
        if(isRogueSelected)
        {
            rogueParticle.Play();
            rogueParticle.Emit(1);
            rogueLight.range = 5;
        }
        else if(!isRogueSelected)
        {
            rogueParticle.Stop();
            rogueLight.range = 0;
        }

        if(isWizardSelected)
        {
            wizardParticle.Play();
            wizardParticle.Emit(1);
            wizardLight.range = 5;
        }
        else if(!isWizardSelected)
        {
            wizardParticle.Stop();
            wizardLight.range = 0;
        }
    }
    public void DisableEffects()
    {
        warriorParticle.Pause();
        rogueParticle.Pause();
        wizardParticle.Pause();
        warriorLight.range = 0;
        rogueLight.range = 0;
        wizardLight.range = 0;
        doorLight.range = 0;
    }
    private void UpdateWarriorSelectPrompt()
    {
        if (isWarriorSelected)
        {
            selectWarriorPrompt.SetActive(false);
            cancelWarriorPrompt.SetActive(true);
        }
        else
        {
            selectWarriorPrompt.SetActive(true);
            cancelWarriorPrompt.SetActive(false);
        }
    }
    private void UpdateRogueSelectPrompt()
    {
        if (isRogueSelected)
        {
            selectRoguePrompt.SetActive(false);
            cancelRoguePrompt.SetActive(true);
        }
        else
        {
            selectRoguePrompt.SetActive(true);
            cancelRoguePrompt.SetActive(false);
        }
    }
    private void UpdateWizardSelectPrompt()
    {
        if (isWizardSelected)
        {
            selectWizardPrompt.SetActive(false);
            cancelWizardPrompt.SetActive(true);
        }
        else
        {
            selectWizardPrompt.SetActive(true);
            cancelWizardPrompt.SetActive(false);
        }
    }
    private void UpdateBackPrompt()
    {
        if (isWarriorSelected || isRogueSelected || isWizardSelected)
        {
            backPrompt.SetActive(false);
        }
        else
        {
            backPrompt.SetActive(true);
        }
    }
    private void OnPauseButtonPressed()
    {
        if (Input.GetButtonDown("Start"))
        {
            optionsMenuOpen = true;
        }
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

}
