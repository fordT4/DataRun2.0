using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlatformWithLaser : MonoBehaviour
{
    /*public GameObject On;
    public GameObject Off;*/
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public LayerMask layerMaskTarget;
    private bool isActive;
    public GameObject closePlatform;
    public GameObject openPlatform;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(IsAvaible());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Laser jest aktywny"+ isActive);
        Debug.Log("Widze postać" +IsTarget());
        if (IsTarget() && isActive)
        {
            Debug.Log("Wylaczam");
            closePlatform.SetActive(false);
            openPlatform.SetActive(true);
        }

        if (!closePlatform.activeSelf)
        {
            StartCoroutine(Active());
        }
        
    }

    private IEnumerator IsAvaible()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            isActive = false;
            yield return new WaitForSeconds(15);
            isActive = true;
  
        }
    }

    private IEnumerator Active()
    {
        yield return new WaitForSeconds(3);
        openPlatform.SetActive(false);
        closePlatform.SetActive(true);
    }
    private bool IsTarget()
    {
        Vector2 pos = transform.position + new Vector3(boxCollider.offset.x-boxCollider.size.x/3.5f, boxCollider.offset.y+boxCollider.size.y/2+0.3f,0);
      
        RaycastHit2D hitL = Physics2D.Raycast(pos, Vector2.right, 0.5f, layerMaskTarget);

        Debug.DrawRay(pos, Vector2.right, Color.green);
        
        return (hitL.collider != null);
    }
}
