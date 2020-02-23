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
           Play();
            isPlaying = true;
            
        }
        else if(!InForest())
        {
            Stop();
            isPlaying = false;
        }
    }

    bool InForest()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 118 && focusPosition.y > 58;
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
