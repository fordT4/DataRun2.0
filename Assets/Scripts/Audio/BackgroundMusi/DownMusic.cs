using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DownMusic : MonoBehaviour
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
            musicSource.Play();
            isPlaying = true;
            musicSource.mute = false;
        }
        else if(!OnDown())
        {
            musicSource.mute = true;
            isPlaying = false;
        }
    }

    bool OnDown()
    {
        return focusPosition.x < -143 && focusPosition.x > -171 && focusPosition.y < 72 && focusPosition.y > -2;
    }
}
