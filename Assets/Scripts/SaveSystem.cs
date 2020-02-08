using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveSystem 
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.neverend";
        FileStream stream = new FileStream(path,FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream,data);
        stream.Close();
    }
    public static void SaveScore()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/score.neverend";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreData data = new ScoreData();

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.neverend";
        if (File.Exists(path))
        {
            BinaryFormatter formatter=new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            PlayerData data =formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }else
        {
            Debug.LogError("Save file not found in "+path);
            return null;
        }
        
    }
    public static ScoreData LoadScore()
    {
        string path = Application.persistentDataPath + "/score.neverend";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreData data = formatter.Deserialize(stream) as ScoreData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }
}
