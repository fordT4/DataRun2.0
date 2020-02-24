using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Disable : MonoBehaviour
{
    public UnityEngine.UI.Button load;

    private int active;
    // Start is called before the first frame update
    void Start()
    {
        active = PlayerPrefs.GetInt("Start");
        Check();
    }

    public void Check()
    {
        if (active != 1)
        {
            load.interactable = false;
        }
        else
        {
            load.enabled = true;
        }
    }
}
