using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UniversityMusic : MonoBehaviour
{
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying =false;

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (OnUniversity() && !isPlaying)
        {
            musicSource.Play();
            isPlaying = true;
            musicSource.mute = false;
        }
        else if(!OnUniversity())
        {
            musicSource.mute = true;
            isPlaying = false;
        }
    }

    bool OnUniversity()
    {
        return focusPosition.x < 19 && focusPosition.x > -8 && focusPosition.y < 72 && focusPosition.y > -1;
    }
}
