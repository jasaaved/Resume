using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    private AudioSource player;
    private bool SlimeIntroDone;
    private bool KnightIntroDone;
    private bool GolemIntroDone;
    private bool BossBatIntroDone;


    [Header("---Title Lines---]")]
    [Space(10)]
    public AudioClip[] Corrupted;

    [Header("---Character Select Entry Lines---]")]
    [Space(10)]
    public AudioClip[] KyleSpeaking;

    [Header("---Entrance Room Lines---")]
    [Space(10)]
    public AudioClip[] StartingRoom;


    ////////////////////////////////////////////////////////////////////////
    /// These character lines are for backand forth dialogue management. ///
    ////////////////////////////////////////////////////////////////////////

    [Header("---Alice Lines---")]
    [Space(10)]
    public AudioClip[] AliceVoiceLines;

    [Header("---Alex Lines---")]
    [Space(10)]
    public AudioClip[] AlexVoiceLines;

    [Header("---Gavin Lines---")]
    [Space(10)]
    public AudioClip[] GavinVoiceLines;

    [Header("---Enemy Related Lines---")]
    [Space(10)]
    public AudioClip[] KnightVoiceLines;
    public AudioClip[] SlimeVoiceLines;
    public AudioClip[] CrystalGolemLines;
    public AudioClip[] BossBatLines;
    public AudioClip[] BossBatDeathLines;
    public AudioClip[] DreadKnightEncounterLines;
    public AudioClip[] DreadKnightDeathLines;

    public void Start()
    {
        player = GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene().name == "RoomGenerator")
        {
            PlayEntryLine();
        }
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            PlayCharacterSelectLine();
        }
        if (SceneManager.GetActiveScene().name == "Title")
        {
            PlayTitleScreenLine();
        }
    }


    public void PlayEntryLine()
    {
        int index = Random.Range(0, StartingRoom.Length);
        player.clip = StartingRoom[index];
        player.Play();
    }
    public void PlayTitleScreenLine()
    {
        int index = Random.Range(0, Corrupted.Length);
        player.clip = Corrupted[index];
        player.Play();
    }
    public void PlayCharacterSelectLine()
    {
        int index = Random.Range(0, KyleSpeaking.Length);
        player.clip = KyleSpeaking[index];
        player.Play();
    }
    public void KyleSlimeLine()
    {
        if (!player.isPlaying)
        {
            if (SlimeIntroDone)
            {
                return;
            }
            player.clip = SlimeVoiceLines[0];
            player.Play();
            SlimeIntroDone = true;
        }
    }
    public void KyleKnightLine()
    {
        if (!player.isPlaying)
        {
            if (KnightIntroDone)
            {
                return;
            }
            player.clip = KnightVoiceLines[0];
            player.Play();
            KnightIntroDone = true;
        }
    }
    public void KyleGolemLine()
    {
        if (!player.isPlaying)
        {
            if (GolemIntroDone)
            {
                return;
            }
            player.clip = CrystalGolemLines[0];
            player.Play();
            GolemIntroDone = true;
        }
    }
    public void KyleBossBatLine()
    {
        if (!player.isPlaying)
        {
            player.clip = BossBatLines[0];
            player.Play();
            BossBatIntroDone = true;
        }
    }
    public void KyleDreadKnightLine()
    {
        if (!player.isPlaying)
        {
            player.clip = DreadKnightEncounterLines[0];
            player.Play();
        }
    }
}
