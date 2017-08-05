using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDeselectLine : MonoBehaviour {

    private string statueType;
    private GameObject player;
    private bool isSelected;
    public AudioClip[] DeselectedLines;
    private AudioSource audioPlayer;

    private void Start()
    {
        GetClass();
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        player = GameObject.Find(statueType + "(Clone)");
        if(player == null && isSelected)
        {
            isSelected = false;
            PlayDeselectedLine();
        }
        else if(player != null && !isSelected)
        {
            isSelected = true;
        }

        if(audioPlayer.isPlaying && isSelected)
        {
            audioPlayer.Stop();
        }
    }



    private void GetClass()
    {
        if(this.name.Contains("Warrior"))
        {
            statueType = "Warrior";
        }
        else if(this.name.Contains("Rogue"))
        {
            statueType = "Rogue";
        }
        else if(this.name.Contains("Wizard"))
        {
            statueType = "Wizard";
        }
    }

    private void PlayDeselectedLine()
    {
        if(DeselectedLines.Length == 0)
        {
            return;
        }
        int index = Random.Range(0, DeselectedLines.Length);
        audioPlayer.clip = DeselectedLines[index];
        audioPlayer.Play();
    }


}
