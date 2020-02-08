[System.Serializable]

public class ScoreData
{
    public int fallNumber;
    public int jumpCount;
    public float startTime;
    public ScoreData()
    {
        fallNumber =ScoreScript.fallNumber;
        jumpCount = ScoreScript.jumpCount;
        startTime = ScoreScript.tmpT;
    }
}


