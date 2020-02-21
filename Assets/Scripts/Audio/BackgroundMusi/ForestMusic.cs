using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ForestMusic : MonoBehaviour
{
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying =false;

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (InForest() && !isPlaying)
        {
            musicSource.Play();
            isPlaying = true;
            musicSource.mute = false;
        }
        else if(!InForest())
        {
            musicSource.mute = true;
            isPlaying = false;
        }
    }

    bool InForest()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 118 && focusPosition.y > 58;
    }
}
