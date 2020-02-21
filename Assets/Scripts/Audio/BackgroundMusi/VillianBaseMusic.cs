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
            musicSource.Play();
            isPlaying = true;
            musicSource.mute = false;
        }
        else if(!InBase())
        {
            musicSource.mute = true;
            isPlaying = false;
        }
    }

    bool InBase()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 268 && focusPosition.y > 193;
    }
}
