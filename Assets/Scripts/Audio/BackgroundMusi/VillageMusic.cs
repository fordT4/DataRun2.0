using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class VillageMusic : MonoBehaviour
{
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying =false;

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (OnDown() && !isPlaying)
        {
            Play();
            isPlaying = true;
           
        }
        else if(!OnDown())
        {
            Stop();
            isPlaying = false;
        }
    }

    bool OnDown()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 58 && focusPosition.y > -2;
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
