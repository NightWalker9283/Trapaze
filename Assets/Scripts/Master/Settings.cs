using UnityEngine;

//設定ファイル用
[System.Serializable]
public class Settings
{
    public string name; //ユーザー名
    public bool audio_enabled; //オーディオ全体のON・OFF
    public float audio_volume; //オーディオ全体の音量
    public bool enable_voice; //ボイスのみのON・OFF
    public int play_count; //広告表示判定用のプレイ回数
    public string ver; //セーブ時のアプリのVer.
    public float time_to_next_review; //次のレビューまでの残りプレイ時間[s]
    


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
        this.time_to_next_review = 18000f;
      

    }

}
