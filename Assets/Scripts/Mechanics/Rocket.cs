

using UnityEngine;


public class Rocket : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    public LayerMask layerMask;
    private PlayerController target;
    private Vector2 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<PlayerController>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject,3f);

    }
    void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, layerMask);
        if (groundInfo.collider)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name.Equals("Player"))
        {
            target.Reject(transform.position);
            Destroy(gameObject);
        }
      

    }

}
