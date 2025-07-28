using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    [Header("Background Music Clips")]
    public AudioClip bgmDay;
    public AudioClip bgmNight;
    [Header("Sound Effects")]
    public AudioClip footstepPlayer;
    public AudioClip footstepNPC;
    public AudioClip chopTree;
    public AudioClip mineRock;
    public AudioClip buyItem;
    public AudioClip dialoguePopup;
    public AudioClip pickupItem;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip uiClick;
    public AudioClip rain;
    public AudioClip thunder;
    public AudioClip endDay;
    private Dictionary<string, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        sfxDict = new Dictionary<string, AudioClip>()
        {
            { "footstep_player", footstepPlayer },
            { "footstep_npc", footstepNPC },
            { "chop_tree", chopTree },
            { "mine_rock", mineRock },
            { "buy_item", buyItem },
            { "dialogue", dialoguePopup },
            { "pickup_item", pickupItem },
            { "door_open", doorOpen },
            { "door_close", doorClose },
            { "ui_click", uiClick },
            { "rain", rain },
            { "thunder", thunder },
            { "end_day", endDay }
        };
    }

    public void PlayMusic(string type)
    {
        switch (type)
        {
            case "day":
                musicSource.clip = bgmDay;
                break;
            case "night":
                musicSource.clip = bgmNight;
                break;
        }
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.ContainsKey(name) && sfxDict[name] != null)
        {
            sfxSource.PlayOneShot(sfxDict[name]);
        }
    }
}
