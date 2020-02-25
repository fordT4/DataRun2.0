
using UnityEngine;

public class SpawnPlaceNutDropAnimation : MonoBehaviour
{
    
    [SerializeField] public GameObject myPrefabs;

    public Animator animator;
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
            animator.SetBool("IsDroping", CameraControler.startSpawn);
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
