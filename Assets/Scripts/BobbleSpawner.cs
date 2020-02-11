using UnityEngine;

public class BobbleSpawner : MonoBehaviour
{

    [SerializeField] public GameObject myPrefabs;
    private float dropRate;
    private float nextDrop;
    void Start()
    {
        dropRate = Random.Range(1.5f, 7f);
        nextDrop = Time.time;
    }

    void Update()
    {
        CheckToDrop();
    }

    void CheckToDrop()
    {
        if (Time.time > nextDrop)
        {
            Instantiate(myPrefabs, transform.position, Quaternion.identity);
            nextDrop = Time.time + dropRate;
            dropRate = Random.Range(1.5f, 5f);
        }
    }
}