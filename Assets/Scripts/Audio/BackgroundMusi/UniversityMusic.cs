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
           Play();
            isPlaying = true;
            
        }
        else if(!OnUniversity())
        {
           Stop();
            isPlaying = false;
        }
    }

    bool OnUniversity()
    {
        return focusPosition.x < 19 && focusPosition.x > -8 && focusPosition.y < 72 && focusPosition.y > -2;
    }
    void Play()
    {
        musicSource.Play();
    }
    void Stop()
    {
        musicSource.Stop();
    }
}
