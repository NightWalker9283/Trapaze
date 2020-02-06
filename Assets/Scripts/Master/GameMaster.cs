using System.Collections;
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
    public GameMode gameMode; //選択中のゲームモード
    public List<string> acquiredVoices; //取得済みボイス
    public List<RecordData> recordDatas; //各ゲームモードの成績
    public Settings settings; //設定データ
    public Titles titles = new Titles(); //称号一覧
    public List<int> acquiredTitles; //取得済み称号（IDのみ保持）
    public static GameMaster gameMaster;
    public static RankingManager rankingManager;
    public List<GameMode> gameModes = new List<GameMode>(); //ゲームモード一覧
    public GameMode gmTraining; //トレーニングモード用のGameModeオブジェクト
    [SerializeField] public AudioMixer am;


    [SerializeField] Toggle tglModeOrigin;  //成績画面用のモード選択トグルオブジェクトのコピー元
    [SerializeField] GameObject cvsInputName; //初回起動時ユーザ名入力UI用キャンバス
    [SerializeField] GameObject imgBrack; //ロード画面用背景
    [SerializeField] WndTitles wndTitles; //アーカイブ画面の称号一覧生成処理実装クラス
    [SerializeField] Transform contentTitles, contentVoices; //アーカイブ画面の称号一覧、取得済みボイス一覧
    [SerializeField] GameObject prfbListElementTitle, prfbListElementVoice; //アーカイブ画面の称号一覧、取得済みボイス一覧の要素プレハブ
    [SerializeField] bool ResetSaveFile = false; //デバッグ用。trueでセーブデータ削除。
    bool _oldResetSaveFile; //デバッグ用。
    Transform tggModes; //成績画面のモード選択用トグルグループ(スクリプト中で取得）
    float wdtInitializeAd = 5f;
    AsyncOperation aoSceneLoad; //非同期シーン読み込み用
    bool isFinishInitializeAds = false; //trueで広告初期化完了
    public bool isTutorial = false; //trueでトレーニングモードをチュートリアル化

    //広告表示間隔管理用のプレイ回数カウンタ
    public int playCount
    {
        get { return settings.play_count; }
        set
        {
            settings.play_count = (value) % 4; //0〜3でループ
            Save();
        }
    }
    void Awake()
    {

        gameMaster = this;
        if (RemoteSettingsManager.rSM == null) gameObject.AddComponent<RemoteSettingsManager>();
        //広告初期化
#if DEBUG
        isFinishInitializeAds = true;
#endif
        if (!isFinishInitializeAds) MobileAds.Initialize(initStatus =>
        {
            wdtInitializeAd = 0;
            isFinishInitializeAds = true;
        });
        gameObject.AddComponent<AdsManager>();
        CreateGameModes();

        // 以降破棄しない
        DontDestroyOnLoad(gameObject);
        Load();

        //バージョンチェック。
        if (settings.ver != Application.version)
        {
            settings.ver = Application.version;
            settings.time_to_next_review = 18000f;　//次回レビュー依頼ダイアログ表示までのプレイ時間(s)
            ChangeVerRecordDatas(); //ランキング登録用のレコードをクリア

            Save();
        }
        rankingManager = GetComponent<RankingManager>();
        if (settings.name.Length <= 0) cvsInputName.gameObject.SetActive(true);　//ユーザー名が初期値の場合設定ダイアログ表示

    }

    private void Start()
    {

        if (!isFinishInitializeAds && imgBrack != null) //広告初期化完了待ち
        {
            imgBrack.SetActive(true);
            StartCoroutine(MonitorLoadingAd());
        }
        if (settings.audio_enabled) //設定から音量を復元
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
    //広告初期化待機（念の為）
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
        //デバッグ用。ローカルセーブデータの削除
        if (ResetSaveFile != _oldResetSaveFile && ResetSaveFile)
        {
            SaveData.Clear();
            ResetSaveFile = false;
        }

        _oldResetSaveFile = ResetSaveFile;
    }
    //ゲームモードリスト生成
    private void CreateGameModes()
    {

        gameModes.Add(new GameMode(1, "ショート", 1, false, false, 4f, 90f, "少しの空き時間でサクッと遊びたいときに。たったひとつのマヨネーズをどう使う？"));
        gameModes.Add(new GameMode(2, "スタンダード", 1, true, true, 9f, 180f, "ブランコをしっかり楽しみたい方に。ブランコ漕ぎの技術で差をつけろ！"));
        gameModes.Add(new GameMode(3, "チャレンジャー", 0, true, true, 20f, -1, "夢の超巨大ブランコ。異常に眠くなります。睡眠導入、精神安定などの用途にご利用ください。がんばれば一周できます。"));
        gmTraining = new GameMode(99, "トレーニング", 3, false, false, 4f, -1, "チュートリアル付き。初めての方はまずこちらから。ボイス・称号・ハイスコアは記録されません。");

    }
    //成績画面のUI生成
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
    //セーブデータが存在しないときに使用するrecordDatas初期値の設定
    private List<RecordData> InitRecordDatas(List<RecordData> list, int num) 
    {
        for (int i = 0; i < num; i++)
        {
            var recordData = new RecordData();
            recordData.game_mode_id = gameModes[i].id;
            list.Add(recordData);
        }
        return list;
    }
    //（バージョンアップ時用）ランキング登録用レコードをクリア。過去のレコードはベスト記録として保持
    public void ChangeVerRecordDatas()
    {

        foreach (var item in recordDatas)
        {
            if (item.max_distance_best < item.max_distance)
            {
                item.max_distance_best = item.max_distance;
                item.timespan_maxdistance_best = item.timespan_maxdistance;
            }
            if (item.min_distance_best > item.min_distance)
            {
                item.min_distance_best = item.min_distance;
                item.timespan_mindistance_best = item.timespan_mindistance;
            }
            item.max_distance = 0;
            item.timespan_maxdistance = 0;
            item.min_distance = 0;
            item.timespan_mindistance = 0;

        }
    }
    //アーカイブ画面用のリストビューの要素を生成
    private void CreateLibraryListViews()
    {
        CreateLibListTitles();
        CreateLibListVoices();

    }
    //アーカイブ画面用の称号リスト（取得の有無を反映）を生成
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
    //アーカイブ画面用の取得済みボイスリストを生成
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
    //ミュート切り替え。呼ばれるたびにON/OFF
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
    //ミュート切り替え。falseでミュート。
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
    //演出用強制ミュート。設定でミュートにしていない場合はfalseを与えると元のボリュームに戻る。
    public void MuteAudio(bool enable)
    {
        if (enable)
        {
            SetBgmVolume(0f);
        }
        else
        {
            if (settings.audio_enabled) SetBgmVolume(settings.audio_volume);　//元々ミュートの場合は戻さない
        }
    }

    //現在の音量設定値を取得
    public float GetBgmVolume()
    {
        float decibel;
        am.GetFloat("BGMVolumeMaster", out decibel);
        return Mathf.Pow(10f, decibel / 20f);
    }

    //BGMのみのボリューム調整
    public void SetBgmVolume(float volume)
    {

        float decibel = 20.0f * Mathf.Log10(volume);
        if (float.IsNegativeInfinity(decibel))
        {
            decibel = -96f;
        }
        am.SetFloat("BGMVolumeMaster", decibel);

    }
    //ボイスON
    public void VoiceOn()
    {
        am.SetFloat("VoiceVolume", 20.0f * Mathf.Log10(10f));
    }
    //ボイスOFF
    public void VoiceOff()
    {
        am.SetFloat("VoiceVolume", -96f);
    }

    public void Review()
    {
#if UNITY_IOS
        if (!UnityEngine.iOS.Device.RequestStoreReview())　//ReuestStoreReviewが使える場合は自作ダイアログを使用しない
#endif
        {
            Ask();        // レビューするかどうか聞く
        }
    }

    //ストアレビュー依頼ダイアログ表示
    void Ask()
    {
        Dialog dialog = new Dialog("ストアレビュー", "開発の励みになるのでよろしければ★5の評価をお願いします！！", "ストアでレビューを書く", "あとで");
        dialog.OnComplete += (Dialog.DialogResult result) =>
        {
            if (result == Dialog.DialogResult.YES)
            {
#if UNITY_IOS
                string url = "itms-apps://itunes.apple.com/jp/app/id1489878241?mt=8&action=write-review";
#else
                string url = "market://details?id=com.NightWalker.Trapeze";
#endif
                Application.OpenURL(url);
            }

        };
    }

    //ローカルセーブ
    public void Save()
    {
        SaveData.SetList<RecordData>("recordDatas", recordDatas);
        SaveData.SetList<string>("acquiredVoices", acquiredVoices);
        SaveData.SetClass<Settings>("settings", settings);
        SaveData.SetList<int>("acquiredTitles", acquiredTitles);
        SaveData.Save();
    }

    //ローカルロード
    void Load()
    {
        var list = new List<RecordData>(gameModes.Count);
        recordDatas = SaveData.GetList<RecordData>("recordDatas", InitRecordDatas(list, gameModes.Count));
        acquiredVoices = SaveData.GetList<string>("acquiredVoices", new List<string>());
        settings = SaveData.GetClass<Settings>("settings", new Settings());
        acquiredTitles = SaveData.GetList<int>("acquiredTitles", new List<int>());
        Debug.Log(settings.name);
    }

    //リトライ用
    public void Play()
    {
        Save();
        aoSceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        playCount++;
        Time.timeScale = 1f;　//念の為
    }

    //タイトルに移動
    public void Title()
    {
        isTutorial = false;
        Save();
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;　//念の為
    }

    //プレイ画面に移動
    public void GameStart()
    {
        imgBrack.SetActive(true);

        StartCoroutine(ShowLoadingProc());
        return;

        IEnumerator ShowLoadingProc()
        {
            //ロード画面遷移
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
