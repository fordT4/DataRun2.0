using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class VillanController : MonoBehaviour
{
    public Animator animator;

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
            transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
    }

    void Update()
    {
        if( PlayerController.isRestart)
        { startAnimation = true;}
    }
    public void Show_Me_Villain()
    {
        transform.position = new Vector2(-2.7f, -1.26f);
        gameObject.SetActive(true);
        isAnimation = false;
        StartCoroutine(MoveVillanTask());
    }

    IEnumerator MoveVillanTask()
    {
        transform.eulerAngles = new Vector3(0, -180, 0);

        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2(6f, 0);
        animator.SetBool("ToWalk", true);
        yield return new WaitForSeconds(1.3f);
        rigidBody.gravityScale = 2;
        animator.SetBool("ToWalk", false);
        music.Jump();
        animator.SetBool("ToJump", true);
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
        animator.SetBool("ToJump", false);
        isAnimation = true;
        transform.position=new Vector3(-221,268.5f,0);
        transform.localScale=new Vector3(0.8f,0.8f,1);
    }

}
