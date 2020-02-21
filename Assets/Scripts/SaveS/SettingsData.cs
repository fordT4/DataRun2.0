[System.Serializable]

public class SettingsData
{
    public float volume;

    public SettingsData()
    {
        volume = AudioController.musicVolume;
    }

}
