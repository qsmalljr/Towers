﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudio : MonoBehaviour
{

    public GameObject musicToggle;
    public AudioSource music;

    private Toggle muteToggle;

    void Start()
    { //check what previous setting is
        muteToggle = musicToggle.GetComponent<Toggle>();
        if (PlayerPrefs.GetInt("music") == 0)
        {
            muteToggle.isOn = false;
        }
        else
        {
            muteToggle.isOn = true;
        }
    }

    void Update()
    { //when toggle is pressed mute/unmute
        if (muteToggle.isOn)
        {
            PlayerPrefs.SetInt("music", 1);
            music.mute = true;
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
            music.mute = false;
        }
    }
}
