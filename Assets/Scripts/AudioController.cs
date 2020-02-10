using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip jump;
    public AudioClip reject;
    public AudioClip wallHit;
    public AudioClip fall;
    public  AudioSource musicSource;
    private float musicVolume = 1f;
    /*void Start() 
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        musicSource.volume = musicVolume;
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
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
