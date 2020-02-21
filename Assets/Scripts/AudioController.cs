using UnityEngine;
using UnityEngine.UIElements;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip jump;
    public AudioClip reject;
    public AudioClip wallHit;
    public AudioClip fall;
    public AudioSource musicSource;
    public Slider volume;
    public Slider Sfx;
    public static float musicVolume = 1f;
    void Start()
    {
       LoadVolume();
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.volume = musicVolume;
        SaveSystem.SaveSettings();
    }

    public void Jump()
    {
        musicSource.clip = jump;
        musicSource.Play();
    }
    public void Reject()
    {
        musicSource.clip = reject;
        musicSource.Play();
    }
    public void WallHit()
    {
        musicSource.clip = wallHit;
        musicSource.Play();
    }
    public void Fall()
    {
        musicSource.clip = fall;
        musicSource.Play();
    }

    public void LoadVolume()
    {
       SettingsData data= SaveSystem.LoadSettings();
       musicVolume = data.volume;
       musicSource.volume = musicVolume;
    }
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
