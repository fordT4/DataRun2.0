using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class VillanController : MonoBehaviour
{
    public PlayerEffects music;
    private Rigidbody2D rigidBody;
    public static bool isAnimation=false;
    private bool startAnimation=false;
   
    void Start()
    {
      
        rigidBody = GetComponent<Rigidbody2D>();
        if (startAnimation ||MainMenu.isStart)
        {
            isAnimation = true;
            Show_Me_Villain();
        }
        else
        {
            isAnimation = true;
            transform.position = new Vector3(-221, 268.5f, 0);
        }
    }

    void Update()
    {
        //music.volume = PlayerPrefs.GetFloat("FxMusic");
        if( PlayerController.isRestart)
        { startAnimation = true;}
    }
    public void Show_Me_Villain()
    {
        transform.position = new Vector2(4.64f, -1.24f);
        gameObject.SetActive(true);
        isAnimation = false;
        StartCoroutine(MoveVillanTask());
    }

    IEnumerator MoveVillanTask()
    {
        //yield return new WaitForSeconds(5);//czas scenki to bedzie
        yield return new WaitForSeconds(1);
        music.Jump();
        rigidBody.velocity = new Vector2(6f, 16f);
        yield return new WaitForSeconds(2);
        music.Jump();
        rigidBody.velocity = new Vector2(-6, 16f);
        yield return new WaitForSeconds(2);
        music.Jump();
        rigidBody.velocity = new Vector2(-6, 16f);
        yield return new WaitForSeconds(2);
        music.Jump();
        rigidBody.velocity = new Vector2(0, 16f);
        yield return new WaitForSeconds(1);
        isAnimation = true;
        transform.position=new Vector3(-221,268.5f,0);
    }

}
