using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Button : MonoBehaviour
{
    public GameObject tilemap;
    private Rigidbody2D rb;
    private bool stand;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (stand)
        {
            rb.velocity-=new Vector2(0,0.05f);
        }
        if (Math.Abs(transform.position.y) < 252.9)
        {
            rb.velocity = new Vector2(0,0);
        }
        if (!stand)
        {
            rb.velocity += new Vector2(0, 0.05f);
        }
        if (!stand && Math.Abs(transform.position.y) > 253)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            tilemap.SetActive(false);
            stand = true;
        }
       
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            tilemap.SetActive(true);
            stand = false;
        }

    }
}
