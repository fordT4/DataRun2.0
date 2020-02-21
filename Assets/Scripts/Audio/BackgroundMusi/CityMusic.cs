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
            musicSource.Play();
            isPlaying = true;
            musicSource.mute = false;
        }
        else if(!InCity())
        {
            musicSource.mute = true;
            isPlaying = false;
        }
    }

    bool InCity()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 193 && focusPosition.y > 118;
    }
}
