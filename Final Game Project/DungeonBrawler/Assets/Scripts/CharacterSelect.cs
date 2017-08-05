using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour {

    public string sceneToLoad;

	public void Load()
    {
        SceneManager.LoadScene(sceneToLoad);
        Time.timeScale = 1;
    }

}
