using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class VillianBaseMusic : MonoBehaviour
{
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying =false;

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (InBase() && !isPlaying)
        {
            Play();
            isPlaying = true;
            
        }
        else if(!InBase())
        {
           Stop();
            isPlaying = false;
        }
    }

    bool InBase()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 268 && focusPosition.y > 193;
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
