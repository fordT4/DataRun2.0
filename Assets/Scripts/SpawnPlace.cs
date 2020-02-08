using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlace : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject myPrefabs;
    private float dropRate;
    private float nextDrop;
    void Start()
    {
        dropRate = 3f;
        nextDrop = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        CheckToDrop();
    }

    void CheckToDrop()
    {
        if(Time.time>nextDrop)
        {
            Instantiate(myPrefabs, transform.position, Quaternion.identity);
            nextDrop = Time.time + dropRate;
        }
    }
}
