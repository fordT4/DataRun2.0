using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FxMenu : MonoBehaviour
{
    public AudioSource musicSource;
    void Update()

    {
        musicSource.volume = PlayerPrefs.GetFloat("FxVolume");
    }


}