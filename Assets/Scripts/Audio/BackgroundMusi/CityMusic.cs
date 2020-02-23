using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CityMusic : MonoBehaviour
{
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying =false;

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (InCity() && !isPlaying)
        {
            Play();
            isPlaying = true;
            
        }
        else if(!InCity())
        {
            Stop();
            isPlaying = false;
        }
    }

    bool InCity()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 193 && focusPosition.y > 118;
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
