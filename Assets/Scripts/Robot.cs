using System.Collections;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] 
    private GameObject rocket;

    private PlayerController target;
    private BoxCollider2D boxCollider;
    
    public float rayLength = 10f;
    public float speed;
    private bool movingRight = true;
    private bool stop = false;
    public Transform groundDetection;
    public LayerMask layerMaskMove;
    public LayerMask layerMaskTarget;
    private Vector2 direction;

    void Start()
    {
        
        boxCollider = GetComponent<BoxCollider2D>();
        target = GameObject.FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        if (!stop && !IsTarget())
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.8f, layerMaskMove);
        
        if (!groundInfo.collider)
        {
            stop = true;
            if (movingRight)
            {
                
                StartCoroutine(Move());
                transform.eulerAngles = new Vector3(0, -180, 0);
                direction=Vector2.left;
                movingRight = false;
            }
            else
            {
                
                StartCoroutine(Move());
                transform.eulerAngles = new Vector3(0, 0, 0);
                direction = Vector2.right;
                movingRight = true;
            }
        }
      
        CheckIfTimeToFire();
    }

 
    void CheckIfTimeToFire()
    {
        if (IsTarget() && !stop)
        {
            StartCoroutine(Shoot());
        }

    }

    bool IsTarget()
    {
        Vector2 pos = transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y);
        RaycastHit2D hitL = Physics2D.Raycast(pos, direction, rayLength, layerMaskTarget);

        return (hitL.collider != null);
    }
    private IEnumerator Move()
    {
        yield return new WaitForSeconds(3);
        stop = false;
    }
    private IEnumerator Shoot()
    {
        stop = true;
        yield return new WaitForSeconds(2);
        Instantiate(rocket, transform.position, Quaternion.identity);
        stop = false;
    }
}