using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    public bool isPaused;                              //Boolean to check if the game is paused or not

    private ShowPanels showPanels;						//Reference to the ShowPanels script used to hide and show UI panels
	
	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanels = GetComponent<ShowPanels>();
	}
	// Update is called once per frame
	void Update()
    {
        OnPauseButton();
        OnCancelButton();
	}

	public void DoPause()
	{
		isPaused = true;
		Time.timeScale = 0;
        showPanels.ShowMenu();
	}
	public void UnPause()
	{
		isPaused = false;
		Time.timeScale = 1;
		showPanels.HideMenu();
	}

    private void OnPauseButton()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (!isPaused)
            {
                DoPause();
            }
        }
    }
    private void OnCancelButton()
    {
        if (Input.GetButtonDown("B"))
        {
            if (isPaused)
            {
                UnPause();
            }
        }
    }

}
