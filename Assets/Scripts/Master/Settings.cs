using UnityEngine;

[System.Serializable]
public class Settings
{
    public string name;
    public bool audio_enabled;
    public float audio_volume;
    public bool enable_voice;
    public int play_count;
    public string ver;
    public float time_to_next_review;
    


    public Settings(string name, bool audio_enabled, float audio_volume, bool enable_voice, int play_count, string ver,float time_to_next_review)
    {
        this.name = name;
        this.audio_enabled = audio_enabled;
        this.audio_volume = audio_volume;
        this.enable_voice = enable_voice;
        this.play_count = play_count;
        this.ver = ver;
        this.time_to_next_review = time_to_next_review;
        

    }
    public Settings()
    {
        this.name = "";
        this.audio_enabled = true;
        this.audio_volume = 1f;
        this.enable_voice = true;
        this.play_count = 0;
        this.ver = "";
        this.time_to_next_review = 0f;
      

    }

}
