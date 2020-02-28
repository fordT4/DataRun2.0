using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public GameObject on;
    public GameObject off;


    void Start()
    {
        StartCoroutine(IsAvaible());
    }
    private IEnumerator IsAvaible()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            on.SetActive(true);
            off.SetActive(false);
            yield return new WaitForSeconds(15);
            on.SetActive(false);
            off.SetActive(true);

        }
    }
}