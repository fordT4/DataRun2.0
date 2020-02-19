using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlatformWithLaser : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public LayerMask layerMaskTarget;
    private bool isActive;
    public GameObject gameObject;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(IsAvaible());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("aktyw"+ isActive);
        Debug.Log(IsTarget());
        if (IsTarget() && isActive)
        {
            gameObject.SetActive(false);
        }

        if (!gameObject.activeSelf)
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
            yield return new WaitForSeconds(3);
            isActive = true;
        }
    }

    private IEnumerator Active()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(true);
    }
    private bool IsTarget()
    {
        Vector2 pos = transform.position + new Vector3(boxCollider.offset.x-boxCollider.size.x/1.33f, boxCollider.offset.y+boxCollider.size.y/2+0.3f,0);
      
        RaycastHit2D hitL = Physics2D.Raycast(pos, Vector2.right, 3f, layerMaskTarget);

        Debug.DrawRay(pos, Vector2.right, Color.green);
        return (hitL.collider != null);
    }
}
