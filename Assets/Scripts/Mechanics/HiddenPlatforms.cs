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
    public GameObject prepare;
    public float startTime;
    private bool isStart = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Time.time > startTime && !isStart)
        {
            StartCoroutine(IsAvaible());
            isStart = true;
        }
    }
    private IEnumerator IsAvaible()
    {
        while (true)
        {
            prepare.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            prepare.SetActive(false);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(3);//czas wlaczenia
            gameObject.SetActive(false);
            yield return new WaitForSeconds(21f - 2.5f);//ile czasu nie dziala
           
            //tutaj koiec animacji - koniec animacji
        }
    }


}