using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip MusicClip;
    public  AudioSource musicSource;
    private float musicVolume = 1f;
    void Start() 
    {
        musicSource.clip = MusicClip;
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.volume = musicVolume;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            musicSource.Play();
        }
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
