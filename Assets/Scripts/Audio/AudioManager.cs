using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Slider volume;
    public Slider fxVolume;

    // Start is called before the first frame update
    void Start()
    {
        volume.value = PlayerPrefs.GetFloat("MusicVolume");
        fxVolume.value = PlayerPrefs.GetFloat("FxVolume");
    }

    public void Volume()
    {
        PlayerPrefs.SetFloat("MusicVolume",volume.value);
    }
    public void FxVolume()
    {
        PlayerPrefs.SetFloat("FxVolume", fxVolume.value);
    }


}
