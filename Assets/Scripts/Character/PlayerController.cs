﻿using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    //floats
    public float speed = 0.01f;
    public float jumpForce = 3f;
    public static float rayLength =0.9f;
    public static float rayCastSizeX = 0.11f;
    public static float rayCastSizeY = 0.18f;
    public float moveInput;

    //bools
    public bool grounded;
    public bool inWindZone = false;
    public bool inStaticWindZone = false;
    private bool inDownScene;
    public static bool isRestart = false;
    public bool canMove = true;


    
   public GameObject windZone;

    //object classes
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    public LayerMask layerMask;

    //animation
    public Animator animator;
    public bool spacePressed;
    public bool isFacingRight;
    private bool isFalling;

    public PlayerEffects audio;

    // Start is called before the first frame update
    void Start()
    {
        //Start when player play first time or restart game
        if (MainMenu.isStart)
        {
            GiveUp();
            MainMenu.isStart = false;
        }
        LoadPlayer();

        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {

        if (!VillanController.isAnimation || !canMove ||PauseMenu.GameIsPaused) return;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position=new Vector3(0,74,0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            transform.position = new Vector3(-148, 75, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            transform.position = new Vector3(-207, 10.6f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            transform.position = new Vector3(-216, 59, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            transform.position = new Vector3(-212, 121, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            transform.position = new Vector3(-209, 171, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            transform.position = new Vector3(-218, 201, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            transform.position = new Vector3(-214, 226, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            transform.position = new Vector3(-207, 242.5f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            transform.position = new Vector3(-218, 254, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            transform.position = new Vector3(-214, 269, 0);
        }
        // 3 level mechanics
        if (inDownScene)
        {
            //player movement right left
            if (!grounded)
            {
                moveInput = Input.GetAxis("Horizontal");
                rigidBody.velocity = new Vector2(moveInput * speed, rigidBody.velocity.y);
            }
        }
        else
        {
            //player movement right left
            if (grounded && !Input.GetButton("Jump") && !inWindZone && !inStaticWindZone)
            {
                moveInput = Input.GetAxis("Horizontal");
                rigidBody.velocity = new Vector2(moveInput * speed, rigidBody.velocity.y);
            }
            //move block when we hold jump button
            else if (Input.GetButton("Jump") && grounded && !inWindZone && !inStaticWindZone && !animator.GetCurrentAnimatorStateInfo(0).IsName("FarmerFall"))
            {
                spacePressed = true;
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
            else if (spacePressed && !animator.GetCurrentAnimatorStateInfo(0).IsName("FarmerFall"))
            {
                spacePressed = false;
            }

            //set jump force
            if (jumpForce < 18f && Input.GetButton("Jump") && grounded && !inWindZone && !inStaticWindZone && !animator.GetCurrentAnimatorStateInfo(0).IsName("FarmerFall"))
            {
                jumpForce += 0.2f;
            }

            if (!grounded)
            {
                jumpForce = 3;
            }
        }

        //player behaviour in wind zone  
        if (inWindZone)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidBody.AddForce(windZone.GetComponent<WindArea>().direction *
                               windZone.GetComponent<WindArea>().strength);
        }

        if (inStaticWindZone)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidBody.AddForce(windZone.GetComponent<WindArea>().direction *
                               windZone.GetComponent<WindArea>().strength);
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (!VillanController.isAnimation || !canMove || PauseMenu.GameIsPaused) return;
        //animator state variables
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Falling", false);
        animator.SetBool("SpacePressed", spacePressed);
        animator.SetBool("DownScene", inDownScene);

        inDownScene = CameraControler.change;
        grounded = IsGrounded();
        
        // physic change in level 3
        if (inDownScene)
        {
            rigidBody.velocity = new Vector2(0, -4f);
            rigidBody.gravityScale = 0.0f;

        }

        // back to physic from other levels
        if (!inDownScene)
        {
            rigidBody.gravityScale = 2f;
        }

        //jump
        if (grounded && Input.GetButtonUp("Jump") && !inDownScene && !animator.GetCurrentAnimatorStateInfo(0).IsName("FarmerFall"))
        {
            audio.Jump(); 
            rigidBody.velocity = Vector2.up * jumpForce;
            ScoreScript.jumpCount++;
            jumpForce = 3;
        }

        //falling control
        if (grounded && isFalling)
        {
            audio.Fall();
            animator.SetBool("Falling", true);
            isFalling = false;
            ScoreScript.fallNumber++;
        }
        if (rigidBody.velocity.y < -20 && !inDownScene)
        {
            isFalling = true;
        }
        
        //player flipping
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

    }
    bool IsGrounded()
    {
        Vector2 boxPos = transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y);
        
        Vector2 pos = boxPos - new Vector2(boxCollider.size.x-rayCastSizeY, rayLength);
        Vector2 posL = boxPos - new Vector2((boxCollider.size.x / 2) + rayCastSizeX, 0);
        Vector2 posR = boxPos + new Vector2((boxCollider.size.x / 2) + rayCastSizeX, 0);

        Vector2 direction2 = Vector2.down;
        Vector2 direction = Vector2.right;

        RaycastHit2D hitL = Physics2D.Raycast(posL, direction2, rayLength, layerMask);
        RaycastHit2D hitR = Physics2D.Raycast(posR, direction2, rayLength, layerMask);
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, boxCollider.size.x, layerMask);
        Debug.DrawRay(pos, direction, Color.green);
        Debug.DrawRay(posL, direction2, Color.red);
        Debug.DrawRay(posR, direction2, Color.red);

        return (hitL.collider != null || hitR.collider != null || hit.collider != null);
    }
    void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        SaveSystem.SaveScore();
    }

    public void GiveUp()
    {
        
        animator.SetFloat("Speed", 0);
        animator.SetBool("Falling", false);
        animator.SetBool("SpacePressed", false);

        CameraControler.startSpawn = false;
        transform.position = new Vector2(-4f, -1.14f);
        isRestart = true;
        SaveSystem.SavePlayer(this);
        ScoreScript.GiveUp();
    }

    private void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }


    public void Reject(Vector2 position)
    {
        
        audio.Reject();
        float magnitude;
        Vector3 direction =  ( transform.position - (Vector3)position).normalized;
        direction.y += 0.5f;
        Debug.Log(direction);
        magnitude = 700;
        GetComponent<Rigidbody2D>().AddForce(direction * magnitude);
        
    }

 
    private void OnTriggerEnter2D(Collider2D coll)  
    {
        if (coll.gameObject.tag == "windArea")
        {
            inWindZone = true;
        }
        if (coll.gameObject.tag == "staticWindArea")
        {
            inStaticWindZone = true;
        }
      
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
        if (coll.gameObject.tag == "staticWindArea")
        {
            inStaticWindZone = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "wall")
        {
            audio.WallHit();
        }
        if (coll.gameObject.tag == "nut")
        {
            audio.NutHit();
        }
        if (coll.gameObject.tag == "animal")
        {
            audio.Oinking();
            canMove = false;
        }

        if (coll.gameObject.tag == "villian")
        {
            CameraControler.end = true;
            PlayerPrefs.SetInt("Start",0);
        }
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
      
        if (coll.gameObject.tag == "animal")
        {
            canMove = true;
        }
      
        
    }
  
    void OnApplicationQuit()
    {
        SavePlayer();
        SaveSystem.SaveScore();
       
    }

}

