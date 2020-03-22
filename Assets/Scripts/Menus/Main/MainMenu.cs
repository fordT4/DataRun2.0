using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool isStart = false;

    void Start()
    {
        Cursor.visible = true;
    }
    public void PlayGame()
    {
        PlayerPrefs.SetInt("Start",1);
        isStart = true;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadPlayer()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }


}
