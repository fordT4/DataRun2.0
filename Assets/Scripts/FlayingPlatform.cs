using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlayingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
    }
    void Update()
    {
        rb.velocity = new Vector2(0, 5f);
        if (rb.transform.position.y > 68 || !CameraControler.change)
        {
            Destroy(gameObject);
        }
    }

}