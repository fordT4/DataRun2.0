using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraControler : MonoBehaviour
{

    public GameObject focusObject;
    private Vector2 focusPosition;
    private float up=12;
    private float down = -3f;
    private float left = -8.5f;
    private float right = 18.5f;
    private readonly float rightLeft = 27f;
    public float followDistance;
    public static bool change = false;
    private bool changeSize=false;
    public static bool startSpawn=false;
    public PostProcessLayer fog;
    public static bool end = false;
    
    void Update()
    {
        
        focusPosition = focusObject.transform.position;

        //camera up-down
        if (focusPosition.y >= up)
        {
            up += 15f;
            down += 15f;
            transform.position += new Vector3(0, 15f, 0);
            //Debug.Log(up);
        }
        if (focusPosition.y < down)
        {
            up -= 15f;
            down -= 15f;
            transform.position += new Vector3(0, -15f, 0);
        }
        //camera lef-right
        if (focusPosition.x <= left)
        {
            
            left -= rightLeft;
            right -= rightLeft;
            transform.position += new Vector3(-rightLeft, 0,0);
            
        }
        if (focusPosition.x > right)
        {
           
            left += rightLeft;
            right += rightLeft;
            transform.position += new Vector3(rightLeft, 0,0);
        }

        

        //down scene
        if (focusPosition.y < 73 && focusPosition.y > 12.5f && focusPosition.x < -145 && focusPosition.x > -166)
        {
            Vector3 distance = focusPosition - (Vector2)transform.position;
            Vector3 moveDistance = Vector2.ClampMagnitude(distance, distance.magnitude - followDistance);
            transform.position+=new Vector3(0,moveDistance.y,0);
            change = true;
        }

        if (change && focusPosition.y > 73)
        {
            change = false;
            transform.position += new Vector3(0, +7.4f, 0);
           
        }
        if (change && focusPosition.y < 12.5)
        {
            change = false;
            transform.position += new Vector3(0, +7.5f, 0);
        }

        //scalling to 4th lvl
        if (focusPosition.x < -197.5f && !changeSize)
        {
            startSpawn = true;
            Vector3 direction = focusObject.transform.localScale;
            focusObject.transform.localScale = direction.x < 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            PlayerController.rayCastSizeX = 0;
            PlayerController.rayCastSizeY = 0.28f;
            PlayerController.rayLength = 0.64f;
            changeSize = true;
        }

        if (changeSize && focusPosition.x > -197.5f)
        {
            startSpawn = false;
            Vector3 direction = focusObject.transform.localScale;
            focusObject.transform.localScale = direction.x < 0 ? new Vector3(-1.37f, 1.37f, 1.37f) : new Vector3(1.37f, 1.37f, 1.37f);
            PlayerController.rayCastSizeX = 0.11f;
            PlayerController.rayCastSizeY = 0.18f;
            PlayerController.rayLength = 0.9f;
            changeSize = false;
        }

        if (InCity() && !PauseMenu.GameIsPaused)
        {
            fog.enabled = true;
            Time.timeScale = 0.5f;
        }
        else if (!PauseMenu.GameIsPaused)
        {
            fog.enabled = false;
            Time.timeScale = 1;
        }

      
        if (end)
        {
            StartCoroutine(NewScene());
        }
    }
    bool InCity()
    {
        return focusPosition.x < -171 && focusPosition.x > -224.5f && focusPosition.y < 193 && focusPosition.y > 117;
    }
   

    private IEnumerator NewScene()
    {
        float fadeTime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
