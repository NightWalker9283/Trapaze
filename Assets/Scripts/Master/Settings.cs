[System.Serializable]
public class Settings
{
    public string name;
    public bool audio_enabled;
    public float audio_volume;

    public Settings(string name,bool audio_enabled, float audio_volume)
    {
        this.name = name;
        this.audio_enabled = audio_enabled;
        this.audio_volume = audio_volume;
    }
    public Settings()
    {
        this.name = "";
        this.audio_enabled = true;
        this.audio_volume = 1f;
    }

}
