using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip jump;
    public AudioClip explosion;
    public AudioClip wallHit;
    public AudioClip fall;
    public AudioClip oinking;
    public AudioClip nut;
    public AudioSource musicSource;
    

    void Update()
    {
        musicSource.volume = PlayerPrefs.GetFloat("FxVolume");
         
    }
    public void Jump()
    {
        musicSource.clip = jump;
        musicSource.Play();
    }
    public void Reject()
    {
        musicSource.clip = explosion;
        musicSource.volume /= 4;
        musicSource.Play();
    }
    public void WallHit()
    {
        musicSource.clip = wallHit;
        musicSource.Play();
    }
    public void NutHit()
    {
        musicSource.clip = nut;
        musicSource.Play();
    }
    public void Oinking()
    {
        musicSource.clip = oinking;
        musicSource.Play();
    }
    public void Fall()
    {
        musicSource.clip = fall;
        musicSource.Play();
    }
 
}
