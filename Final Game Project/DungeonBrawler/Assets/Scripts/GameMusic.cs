using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour {

    private AudioSource combatMusic;
    private AudioSource mainTheme;
    public AudioSource[] music;
    private float combatMusicVolume;
    private float mainThemeVolume;
    private GameObject manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
        combatMusic = music[1];
        mainTheme = music[0];
        mainThemeVolume = 1;
        combatMusicVolume = 0;
    }

    private void Update()
    {
        ManageTracks();
    }

    private void ManageTracks()
    {
        if(manager.GetComponent<GameManager>().InCombat == true)
        {
            if(mainThemeVolume >= 0)
            {
                mainThemeVolume -= Time.deltaTime;
                mainTheme.volume = mainThemeVolume;
            }
            if(combatMusicVolume <= 1)
            {
                combatMusicVolume += Time.deltaTime;
                combatMusic.volume = combatMusicVolume;
            }
        }
        else if(manager.GetComponent<GameManager>().InCombat == false)
        {
            if (mainThemeVolume <= 1)
            {
                mainThemeVolume += Time.deltaTime;
                mainTheme.volume = mainThemeVolume;
            }
            if (combatMusicVolume >= 0)
            {
                combatMusicVolume -= Time.deltaTime;
                combatMusic.volume = combatMusicVolume;
            }
        }
    }


}
