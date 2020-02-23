using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class HiddenPlatforms : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public float timer;
    public GameObject gameObject;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(IsAvaible());
    }

    private IEnumerator IsAvaible()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(timer - 1.5f);
            //tutaj do animacji zmiany koloru - start animacji
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(true);
            //tutaj koiec animacji - koniec animacji
        }
    }


}