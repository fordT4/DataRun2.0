using UnityEngine;

public class BobbleSpawner : MonoBehaviour
{

    [SerializeField] public GameObject myPrefabs;
    private float dropRate;
    private float nextDrop;
    void Start()
    {
        nextDrop = Time.time;
    }

    void Update()
    {
        
        if (CameraControler.change)
        {
            CheckToDrop();
        }
    }

    void CheckToDrop()
    {
        if (Time.time > nextDrop)
        {
            Instantiate(myPrefabs, transform.position, Quaternion.identity);
            dropRate = Random.Range(1.5f, 7f);
            nextDrop = Time.time + dropRate;
            
        }
    }
}