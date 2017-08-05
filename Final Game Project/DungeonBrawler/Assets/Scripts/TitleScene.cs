using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;


public class TitleScene : MonoBehaviour
{
    public GameObject optionsMenu;

    private bool loadScene = false;
    private bool optionsMenuOpen = false;

    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        OnAButtonPressed();
        OnPauseButtonPressed();
    }

    public void OnAButtonPressed()
    {
        if (Input.GetButtonDown("A"))
        {
            if (optionsMenuOpen)
            {
                optionsMenuOpen = false;
                return;
            }

            bool isPaused = optionsMenu.GetComponent<Pause>().isPaused;

            if (!loadScene && !isPaused)
            {
                loadScene = true;
                SceneManager.LoadScene("ControlScheme");
            }
        }
    }

    private void OnPauseButtonPressed()
    {
        if (Input.GetAxis("Menu") > 0)
        {
            optionsMenuOpen = true;
        }
    }

}