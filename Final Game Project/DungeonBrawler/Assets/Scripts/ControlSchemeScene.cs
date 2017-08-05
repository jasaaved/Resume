using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

//Most of the following code was found online at this URL, in a tutorial format: http://blog.teamtreehouse.com/make-loading-screen-unity

public class ControlSchemeScene : MonoBehaviour {

    public string scene;
    public GameObject loadingText;
    public GameObject continueText;
    public GameObject backPrompt;
    public string previousScene;

    private bool loadScene = false;
    private bool optionsMenuOpen = false;

    void Start()
    {
        optionsMenuOpen = false;
    }
    void Update()
    {
        OnAButtonPressed();
        OnBButtonPressed();
        OnPauseButtonPressed();
    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

    private void OnPauseButtonPressed()
    {
        if (Input.GetButtonDown("Start"))
        {
            optionsMenuOpen = true;
        }
    }
    private void OnAButtonPressed()
    {
        if (Input.GetButtonDown("A"))
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }
        }

        if (Input.GetButtonDown("A") && !loadScene && !optionsMenuOpen)
        {
            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            // ...change the instruction text to read "Loading..."
            loadingText.SetActive(true);
            continueText.SetActive(false);

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());
        }
    }
    private void OnBButtonPressed()
    {
        if (Input.GetButtonDown("B"))
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }
        }

        if (Input.GetButtonDown("B") && !loadScene && !optionsMenuOpen)
        {
            SceneManager.LoadScene(previousScene);
        }
    }

}
