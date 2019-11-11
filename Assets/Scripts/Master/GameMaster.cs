using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameMaster : MonoBehaviour
{
    public GameMode gameMode;
    public List<RecordData> recordDatas;
    public Settings settings;
    public static GameMaster gameMaster;
    public List<GameMode> gameModes = new List<GameMode>();

    [SerializeField] Toggle tglModeOrigin;
    private Transform tggModes;
    [SerializeField] AudioMixer am;
   

    void Awake()
    {
        gameMaster = this;
        CreateGameModes();
        
        // 以降破棄しない
        DontDestroyOnLoad(gameObject);
        Load();

        if (settings.audio_enabled)
            SetBgmVolume(settings.audio_volume);
        else
            SetBgmVolume(0f);
    }

    private void Start()
    {
        tggModes = tglModeOrigin.transform.parent;
        CreateRecordUI(tggModes);
    }

    private void CreateGameModes()
    {
        gameModes.Add(new GameMode(1, "サクッと", 4f, 60f, false, "少しの空き時間でサクッと遊びたいときに。"));
        gameModes.Add(new GameMode(2, "スタンダード", 8f, -1f, false, "時間無制限で巨大ブランコを漕ぎまくれ！がんばれば一周できます。"));
        gameModes.Add(new GameMode(3, "チャレンジャー", 20f, -1, false, "夢の超巨大ブランコ。異常に眠くなります。睡眠導入、精神安定などの用途にご利用ください。"));

    }

    private void CreateRecordUI(Transform toggleGroup)
    {
 

        for (int i = 1; i <= gameModes.Count; i++)
        {
            var tglModeElement=Instantiate(tglModeOrigin);
            var matchModeElementName = gameModes.Find(x => (x.id) == i).name;
            tglModeElement.transform.GetComponentInChildren<Text>().text = (matchModeElementName!=null?matchModeElementName:"");
            tglModeElement.transform.parent = toggleGroup;
            tglModeElement.transform.localScale = tglModeOrigin.transform.localScale;
            tglModeElement.isOn = false;
        }
        tglModeOrigin.isOn = true;

    }

    private List<RecordData> InitRecordDatas(List<RecordData> list) //セーブデータが存在しないときに使用するrecordDatas初期値の設定
    {
        for(int i = 1; i <= list.Count; i++)
        {
            list[i].game_mode_id = i;
        }
        return list;
    }

    public void SwitchAudio()
    {

        settings.audio_enabled = !settings.audio_enabled;
        if (!settings.audio_enabled)
        {
            SetBgmVolume(0f);
        }
        else
        {
            SetBgmVolume(settings.audio_volume);
        }
    }

    public float GetBgmVolume()
    {
        float decibel;
        am.GetFloat("BGMVolumeMaster", out decibel);
        return Mathf.Pow(10f, decibel / 20f);
    }

    public void SetBgmVolume(float volume)
    {
        
        float decibel = 20.0f * Mathf.Log10(volume);
        if (float.IsNegativeInfinity(decibel))
        {
            decibel = -96f;
        }
        am.SetFloat("BGMVolumeMaster", decibel);

    }


    void Save()
    {
        SaveData.SetList<RecordData>("recordDatas", recordDatas);
        SaveData.SetClass<Settings>("settings", settings);
        SaveData.Save();
    }

    void Load()
    {
        recordDatas= SaveData.GetList<RecordData>("recordDatas", InitRecordDatas(new List<RecordData>(gameModes.Count)));
        settings=SaveData.GetClass<Settings>("settings", new Settings());
    }

    
    public void GameStart()
    {
        Save();
        SceneManager.LoadScene("Main");
        
    }
   

    public void Title()
    {
        Save();
        SceneManager.LoadScene("Title");
    }
}
