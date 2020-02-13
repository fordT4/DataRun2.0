using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = 0.5f;
    private Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

   
    void Update()
    {
        Vector2 offset = new Vector2(0,Time.time * speed);
        renderer.material.mainTextureOffset = offset;
    }
}
