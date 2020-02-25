
using UnityEngine;

public class SpawnPlace : MonoBehaviour
{
    
    [SerializeField] public GameObject myPrefabs;

    private float dropRate;
    private float nextDrop;
    void Start()
    {
        dropRate = 3f;
        nextDrop = Time.time;
    }

    void Update()
    {
        if (CameraControler.startSpawn)
        {
            Drop();
        }
    }

    void Drop()
    {
        if(Time.time>nextDrop)
        {
            Instantiate(myPrefabs, transform.position, Quaternion.identity);
            nextDrop = Time.time + dropRate;
        }
    }
}
