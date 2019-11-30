using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NCMB;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;

[System.Serializable]
public class GameMaster : MonoBehaviour
{
    public GameMode gameMode;
    public List<RecordData> recordDatas;
    public Settings settings;
    public static GameMaster gameMaster;
    public static RankingManager rankingManager;
    public List<GameMode> gameModes = new List<GameMode>();

    [SerializeField] Toggle tglModeOrigin;
    [SerializeField] GameObject cvsInputName;

    private Transform tggModes;
    [SerializeField] public AudioMixer am;
    [SerializeField] bool ResetSaveFile = false;
    bool _oldResetSaveFile;

    void Awake()
    {
        gameMaster = this;
        CreateGameModes();

        // 以降破棄しない
        DontDestroyOnLoad(gameObject);
        Load();
        rankingManager =GetComponent<RankingManager>();
        if (settings.name.Length <= 0) cvsInputName.gameObject.SetActive(true);

    }

    private void Start()
    {
        if (settings.audio_enabled)
            SetBgmVolume(settings.audio_volume);
        else
            SetBgmVolume(0f);
        if (tglModeOrigin != null)
        {
            tggModes = tglModeOrigin.transform.parent;
            CreateRecordUI(tggModes);
        }
    }

    private void Update()
    {
        if (ResetSaveFile != _oldResetSaveFile && ResetSaveFile)
        {
            SaveData.Clear();
            ResetSaveFile = false;
        }

        _oldResetSaveFile = ResetSaveFile;
    }

    private void CreateGameModes()
    {

        gameModes.Add(new GameMode(1, "サクッと", 4f, 60f, false, "少しの空き時間でサクッと遊びたいときに。"));
        gameModes.Add(new GameMode(2, "スタンダード", 8f, -1f, false, "時間無制限で巨大ブランコを漕ぎまくれ！がんばれば一周できます。"));
        gameModes.Add(new GameMode(3, "チャレンジャー", 20f, -1, false, "夢の超巨大ブランコ。異常に眠くなります。睡眠導入、精神安定などの用途にご利用ください。"));

    }

    private void CreateRecordUI(Transform toggleGroup)
    {


        for (int i = 0; i < gameModes.Count; i++)
        {

            var tglModeElementObj = Instantiate(tglModeOrigin);
            var modeElement = gameModes[i];
            tglModeElementObj.transform.GetComponent<ModeElementForRecords>().id = modeElement.id;


            tglModeElementObj.transform.GetComponentInChildren<Text>().text = (modeElement.name != null ? modeElement.name : "");
            tglModeElementObj.transform.parent = toggleGroup;
            tglModeElementObj.transform.localScale = tglModeOrigin.transform.localScale;
            tglModeElementObj.isOn = false;
            tglModeElementObj.GetComponent<Toggle>().onValueChanged.AddListener(tglModeElementObj.GetComponent<ModeElementForRecords>().OnChangeModeElement);
        }
        tglModeOrigin.GetComponent<Toggle>().onValueChanged.AddListener(tglModeOrigin.GetComponent<ModeElementForRecords>().OnChangeModeElement);
        tglModeOrigin.isOn = true;
        tglModeOrigin.GetComponent<ModeElementForRecords>().SetGeneralRecords();

        toggleGroup.GetComponentInChildren<Button>().transform.SetAsLastSibling();
    }

    private List<RecordData> InitRecordDatas(List<RecordData> list, int num) //セーブデータが存在しないときに使用するrecordDatas初期値の設定
    {
        for (int i = 0; i < num; i++)
        {
            var recordData = new RecordData();
            recordData.game_mode_id = gameModes[i].id;
            list.Add(recordData);
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


    public void Save()
    {
        SaveData.SetList<RecordData>("recordDatas", recordDatas);
        SaveData.SetClass<Settings>("settings", settings);
        SaveData.Save();
    }

    void Load()
    {
        var list = new List<RecordData>(gameModes.Count);
        recordDatas = SaveData.GetList<RecordData>("recordDatas", InitRecordDatas(list, gameModes.Count));
        settings = SaveData.GetClass<Settings>("settings", new Settings());
        Debug.Log(settings.name);
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
