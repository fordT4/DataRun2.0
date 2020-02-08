using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI fallNumberText;
    public TextMeshProUGUI jumpNumberText;

    public static float startTime;

    public static int fallNumber;
    public static int jumpCount;
    public static float tmpT;
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
    }

    // Update is called once per frame
    void Update()
    {
        tmpT = Time.time + startTime;
       
        string hours = ((int)tmpT / 3600).ToString("f0");
        string minutes = ((int)tmpT / 60).ToString("f0");
        string seconds = (tmpT % 60).ToString("f0");

        timerText.text ="In-game time: " + hours + ":" + minutes + ":" + seconds;
        jumpNumberText.text = "Jump number: " + jumpCount;
        fallNumberText.text = "Fall number: " + fallNumber;

    }

    private void LoadScore()
    {
        ScoreData data = SaveSystem.LoadScore();
        fallNumber = data.fallNumber;
        jumpCount = data.jumpCount;
        startTime = data.startTime;
    }

    public static void GiveUp()
    {
        fallNumber = 0;
        jumpCount = 0;
        startTime = 0;
        SaveSystem.SaveScore();
    }
}
