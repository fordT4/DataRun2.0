
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
        animator.SetBool("IsDroping", CameraControler.startSpawn);
        if (CameraControler.startSpawn)
        {
            Drop();
        }
    }

    void Drop()
    {
        if(Time.time>nextDrop)
        {
            Instantiate(myPrefabs, new Vector3(transform.position.x,transform.position.y,transform.position.z-1), Quaternion.identity);
            nextDrop = Time.time + dropRate;
        }
    }
}
