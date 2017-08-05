using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ShowPanels : MonoBehaviour {
    
	public GameObject menuPanel;
    public EventSystem eventSystem;

    void Awake()
    {
        HideMenu();
    }
    
	public void ShowMenu()
	{
		menuPanel.SetActive (true);

        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
	}
	public void HideMenu()
	{
		menuPanel.SetActive (false);
	}

}
