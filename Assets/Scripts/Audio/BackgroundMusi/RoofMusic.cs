using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RoofMusic : MonoBehaviour
{
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying =false;

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (OnRoof() && !isPlaying)
        {
            Play();
            isPlaying = true;
           
        }
        else if(!OnRoof())
        {
            Stop();
            isPlaying = false;
        }
    }

    bool OnRoof()
    {
        return focusPosition.x < 19 && focusPosition.x > -164 && focusPosition.y < 95 && focusPosition.y > 72;
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
