[System.Serializable]
public class Settings
{

    public bool audio_enabled;
    public float audio_volume;

    public Settings(bool audio_enabled, float audio_volume)
    {
        this.audio_enabled = audio_enabled;
        this.audio_volume = audio_volume;
    }
    public Settings()
    {
        this.audio_enabled = true;
        this.audio_volume = 1f;
    }

}
