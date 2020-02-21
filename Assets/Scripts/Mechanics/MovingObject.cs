using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public float speed;
    private bool movingRight = true;
    private bool stop = false;
    public Transform groundDetection;
    public LayerMask layerMask;
    void Update()
    {
        if (!stop)
        {
            transform.Translate(Vector2.right*speed*Time.deltaTime);
        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position,Vector2.down,0.8f,layerMask);
        if (!groundInfo.collider)
        {
            if (movingRight)
            {
                stop = true;
                StartCoroutine(Move());
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                stop = true;
                StartCoroutine(Move());
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    private IEnumerator Move()
    {
        yield return  new WaitForSeconds(2);
        stop = false;
     
    }

}
