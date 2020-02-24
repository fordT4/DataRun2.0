

using UnityEngine;


public class Rocket : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    public LayerMask layerMask;
    private PlayerController target;
    private Vector2 moveDirection;
    public Animator animator;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<PlayerController>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        if (moveDirection.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name.Equals("Player"))
        {
            rb.velocity = new Vector2(0, 0);
            target.Reject(transform.position);
            Destroy(gameObject,0.2f);
            animator.SetBool("Exist", false);
        }
        if (coll.gameObject.tag == "floor" || coll.gameObject.tag == "wall")
        {
            rb.velocity = new Vector2(0, 0);
            Destroy(gameObject, 0.2f);
            animator.SetBool("Exist", false);
        }
    }


   

}
