﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NCMB;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

[System.Serializable]
public class GameMaster : MonoBehaviour
{
    public GameMode gameMode;
    public List<string> acquiredVoices;
    public List<RecordData> recordDatas;
    public Settings settings;
    public Titles titles = new Titles();
    public List<int> acquiredTitles;
    public static GameMaster gameMaster;
    public static RankingManager rankingManager;
    public List<GameMode> gameModes = new List<GameMode>();
    public GameMode gmTraining;


    [SerializeField] Toggle tglModeOrigin;
    [SerializeField] GameObject cvsInputName;
    [SerializeField] GameObject imgBrack;
    [SerializeField] WndTitles wndTitles;

    [SerializeField] Transform contentTitles, contentVoices;
    [SerializeField] GameObject prfbListElementTitle, prfbListElementVoice;
    private Transform tggModes;
    [SerializeField] public AudioMixer am;
    [SerializeField] bool ResetSaveFile = false;
    bool _oldResetSaveFile;
    float wdtInitializeAd = 5f;
    AsyncOperation aoSceneLoad;
    bool isFinishInitializeAds = false;
    public int playCount {
        get { return settings.play_count; }
        set
        {
            settings.play_count = (value)%5;
            Save();
        }
    }
    void Awake()
    {
#if DEBUG
        isFinishInitializeAds = true;
#endif

        gameMaster = this;

        if (!isFinishInitializeAds) MobileAds.Initialize(initStatus => {
            wdtInitializeAd = 0;
            isFinishInitializeAds = true;
        });
        gameObject.AddComponent<AdsManager>();
        CreateGameModes();

        // 以降破棄しない
        DontDestroyOnLoad(gameObject);
        Load();
        rankingManager = GetComponent<RankingManager>();
        if (settings.name.Length <= 0) cvsInputName.gameObject.SetActive(true);

    }

    private void Start()
    {
        if (!isFinishInitializeAds && imgBrack != null)
        {
            imgBrack.SetActive(true);
            StartCoroutine(MonitorLoadingAd());
        }
        if (settings.audio_enabled)
            SetBgmVolume(settings.audio_volume);
        else
            SetBgmVolume(0f);


        if (settings.enable_voice)
            VoiceOn();
        else
            VoiceOff();

        if (tglModeOrigin != null)
        {
            tggModes = tglModeOrigin.transform.parent;
            CreateRecordUI(tggModes);
        }
        if (wndTitles != null)
        {
            CreateLibraryListViews();
        }
    }
    IEnumerator MonitorLoadingAd()
    {
        while (true)
        {


            {
                if (wdtInitializeAd > 0f) wdtInitializeAd -= Time.deltaTime;
                if (wdtInitializeAd <= 0f)
                {

                    imgBrack.SetActive(false);
                    break;
                }
            }
            yield return null;
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

        gameModes.Add(new GameMode(1, "ショート", 1, false, false, 4f, 90f, "少しの空き時間でサクッと遊びたいときに。たったひとつのマヨネーズをどう使う？"));
        gameModes.Add(new GameMode(2, "スタンダード", 1, true, true, 9f, 180f, "ブランコをしっかり楽しみたい方に。ブランコ漕ぎの技術で差をつけろ！"));
        gameModes.Add(new GameMode(3, "チャレンジャー", 0, true, true, 20f, -1, "夢の超巨大ブランコ。異常に眠くなります。睡眠導入、精神安定などの用途にご利用ください。がんばれば一周できます。"));
        gmTraining= new GameMode(99, "トレーニング", 3, false, false, 4f, -1, "チュートリアル付き。初めての方はまずこちらから。ボイス・称号・ハイスコアは記録されません。");

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

    private void CreateLibraryListViews()
    {
        CreateLibListTitles();
        CreateLibListVoices();

    }

    private void CreateLibListTitles()
    {
        var lstListElementsTitles = new List<ListElementTitle>();
        var tggTitles = contentTitles.GetComponent<ToggleGroup>();
        for (int i = 0; i < titles.allTitles.Count; i++)
        {
            var objLe = Instantiate(prfbListElementTitle, contentTitles);
            var tgl = objLe.GetComponent<Toggle>();
            tgl.group = tggTitles;
            tgl.onValueChanged.AddListener(wndTitles.UpdateTitleInfo);
            objLe.GetComponentInChildren<Text>().text = "?????";
            var le = objLe.GetComponent<ListElementTitle>();
            le.enable = false;
            le.id = titles.allTitles[i].id;
            lstListElementsTitles.Add(le);
        }
        foreach (var item in acquiredTitles)
        {
            var le = lstListElementsTitles.Find(dt => dt.id == item);
            if (le != null)
            {
                le.enable = true;
                le.GetComponentInChildren<Text>().text = titles.allTitles.Find(dt => dt.id == item).name;
            }
        }

    }

    private void CreateLibListVoices()
    {

        var tggVoices = contentVoices.GetComponent<ToggleGroup>();
        for (int i = 0; i < acquiredVoices.Count; i++)
        {
            var objLe = Instantiate(prfbListElementVoice, contentVoices);
            var tgl = objLe.GetComponent<Toggle>();
            tgl.group = tggVoices;

            objLe.GetComponentInChildren<Text>().text =
                acquiredVoices[i].Substring(acquiredVoices[i].LastIndexOf('/') + 1);
            var le = objLe.GetComponent<ListElementVoice>();
            le.path = acquiredVoices[i];


        }

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

    public void SwitchAudio(bool value)
    {

        settings.audio_enabled = value;
        if (!settings.audio_enabled)
        {
            SetBgmVolume(0f);
        }
        else
        {
            SetBgmVolume(settings.audio_volume);
        }
    }

    public void MuteAudio(bool enable)
    {
        if (enable)
        {
            SetBgmVolume(0f);
        }
        else
        {
            if (settings.audio_enabled) SetBgmVolume(settings.audio_volume);
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

    public void VoiceOn()
    {
        am.SetFloat("VoiceVolume", 20.0f * Mathf.Log10(10f));
    }

    public void VoiceOff()
    {
        am.SetFloat("VoiceVolume", -96f);
    }

    public void Save()
    {
        SaveData.SetList<RecordData>("recordDatas", recordDatas);
        SaveData.SetList<string>("acquiredVoices", acquiredVoices);
        SaveData.SetClass<Settings>("settings", settings);
        SaveData.SetList<int>("acquiredTitles", acquiredTitles);
        SaveData.Save();
    }

    void Load()
    {
        var list = new List<RecordData>(gameModes.Count);
        recordDatas = SaveData.GetList<RecordData>("recordDatas", InitRecordDatas(list, gameModes.Count));
        acquiredVoices = SaveData.GetList<string>("acquiredVoices", new List<string>());
        settings = SaveData.GetClass<Settings>("settings", new Settings());
        acquiredTitles = SaveData.GetList<int>("acquiredTitles", new List<int>());
        Debug.Log(settings.name);
    }


    public void Play()
    {
        Save();
        aoSceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        playCount++;
        Time.timeScale = 1f;
    }


    public void Title()
    {
        Save();
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
    }

    public void GameStart()
    {
        imgBrack.SetActive(true);

        StartCoroutine(ShowLoadingProc());
        return;

        IEnumerator ShowLoadingProc()
        {

            var img = imgBrack.GetComponent<Image>();
            img.color = new Color(0f, 0f, 0f);
            for (float f = 0f; f <= 1f; f += 0.1f)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, f);
                yield return null;
            }
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            if (gameMode.id == 99)
            {
                aoSceneLoad = SceneManager.LoadSceneAsync("Training");
            }
            else
            {
                aoSceneLoad = SceneManager.LoadSceneAsync("Main");
            }
            playCount++;
        }

    }
}
