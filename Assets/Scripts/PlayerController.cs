using System.Collections;
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
    private bool isFalling;
    private bool inDownScene;
    public static bool isRestart = false;


    //serialized fields
    [SerializeField] public GameObject windZone;
    [SerializeField] public int timer;

    //object classes
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    public LayerMask layerMask;

    //animation
    public Animator animator;
    public bool spacePressed;
    public bool IsFacingRight;
    public bool fallingrn;



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

        //start wind system
        StartCoroutine(Wind());

        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

    }
    private void FixedUpdate()
    {

        if (!VillanController.isAnimation) return;

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
            else if (Input.GetButton("Jump") && grounded && !inWindZone && !inStaticWindZone)
            {
                spacePressed = true;
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
            else if (spacePressed)
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
        
        //animator state variables
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Falling", isFalling);
        animator.SetBool("SpacePressed", spacePressed);
        animator.SetBool("DownScene", inDownScene);
      


        inDownScene = CameraControler.change;
        grounded = IsGrounded();
       

        if (!VillanController.isAnimation) return;

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
            rigidBody.velocity = Vector2.up * jumpForce;
            ScoreScript.jumpCount++;
            jumpForce = 3;
        }

        //falling control
        if (grounded && isFalling)
        {
            isFalling = false;
            ScoreScript.fallNumber++;
        }
        if (rigidBody.velocity.y < -20 && !inDownScene)
        {
            isFalling = true;
        }
        
        //player flipping
        if (moveInput > 0 && !IsFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && IsFacingRight)
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
        IsFacingRight = !IsFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    //wind on and off
    IEnumerator Wind()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            windZone.SetActive(false);
            yield return new WaitForSeconds(timer);
            windZone.SetActive(true);
        }
    }

    public void Reject(Vector2 position)
    {
        float magnitude;
        Vector3  direction =  ( transform.position- (Vector3)position).normalized;
        magnitude = IsGrounded() ? 1600 : 700;
        GetComponent<Rigidbody2D>().AddForce(direction*magnitude);
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

    /*void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "rocket")
        {
            Vector2 direction = -(coll.contacts[0].point - (Vector2)transform.position).normalized;
            rigidBody.AddForce(direction*10f);
        }
    }*/
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
    void OnApplicationQuit()
    {
        SavePlayer();
        SaveSystem.SaveScore();
    }

}

