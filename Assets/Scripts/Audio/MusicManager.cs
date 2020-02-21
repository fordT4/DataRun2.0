﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource music;
    
    void Update()
    {
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

}
