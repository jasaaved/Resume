using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour {

    public GUISkin guiSkin;

    void OnGUI()
    {
        GUI.skin = guiSkin;

        if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height - 75, 248, 46), "Restart"))
                {
            Destroy(GameObject.Find("Winner"));
            SceneManager.LoadScene("ServerBoardScene");
                }


    }

    // Update is called once per frame
    void Update () {
	
	}
}
