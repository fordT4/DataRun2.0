using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public GameObject windZone;
    public GameObject focusObject;
    public AudioSource musicSource;
    private Vector2 focusPosition;
    private bool isPlaying = false;
    private int i = 0;
    void Start()
    {
     
    }

    void Update()
    {
        focusPosition = focusObject.transform.position;
        musicSource.volume = PlayerPrefs.GetFloat("FxVolume")/4;
        if (PauseMenu.GameIsPaused)
        {
            musicSource.Pause();
        }
        else if (!PauseMenu.GameIsPaused && !musicSource.isPlaying)
        {
            musicSource.UnPause();
        }

        if (OnRoof() && !isPlaying)
        {
            Debug.Log(i);
            isPlaying = true;
            StartCoroutine(Wind());
            
        }
   
    }

    bool OnRoof()
    {
        return focusPosition.x < 19 && focusPosition.x > -164 && focusPosition.y < 95 && focusPosition.y > 72;
    }

    IEnumerator Wind()
    {
        /*while (true)
        {*/
        i++;
            yield return new WaitForSeconds(5);
            windZone.SetActive(true);
            musicSource.Play();
            yield return new WaitForSeconds(3);
            windZone.SetActive(false);
            isPlaying = false;

            //}
    }
}
