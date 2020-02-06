using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cinemachine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TweetWithScreenShot;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] UiDistance uiDistance; //飛距離表示UI
    [SerializeField] CanvasGroup ugForPlay, ugController, ugForAfterJump, ugForResult; //各種UI表示制御用
    [SerializeField] GameObject ugNewRecord, ugButtonsForResult;
    [SerializeField] GameObject player; //プレイヤーオブジェクトのトップアッシー
    [SerializeField] public Camera cmrPlayerView, cmrPublic, cmrPlayer, cmrUI, cmrUiPlayer, cmrFace;
        //PlayerView:プレイヤー視界カメラ
        //Public:サイドビューカメラ
        //Player;コントローラー用カメラ
        //UI:cvsPublic用のUI専用カメラ
        //UiPlayer:cvsPlayer用のUI専用カメラ
        //Face:開始時演出用のカメラ
    [SerializeField] CinemachineVirtualCamera vcamPublic, vcamFace, vcamResult; //cinemachine用仮想カメラ
        //Public:cmrPublicを模倣する仮想カメラ。vcamResultへ繋ぐために使用
        //Face;開始時演出用
        //Result:リザルト画面用
    [SerializeField] public Rigidbody rb_Player, rb_Trapeze;
    [SerializeField] public Canvas cvsPublic, cvsPlayer, cvsTop;
        //Public:cmrPublic上に表示するUI
        //Player:cmrPlayer上に表示するUI
        //Top:カメラ関係なく画面全体用のUI
    [SerializeField] public Rigidbody playerControlPoint;
    [SerializeField] PlayerController playerController;
    [SerializeField] uiVelocity txtVelocity; //速度表示UI
    [SerializeField] float testTrapezeLengs = 8f;
    [SerializeField] GameObject prfbTitleElement; //リザルト画面用称号表示UI要素のプレハブ
    [SerializeField] GameObject btnHighScore; //リザルト画面のハイスコア登録/ランキング表示ボタン
    [SerializeField] GameObject wndBackGround, wndResultComment, ugTitleElements; 
        //wndBackGround:ポーズ中の半透明背景
        //wndResultComment:リザルト画面の称号表示ウィンドウ
        //ugTitleElements:称号表示ウィンドウの称号表示部
    [SerializeField] Button btnComment, btnCommentRush; //コメントボタン、弾幕コメントボタン
    [SerializeField] AudioMixer am;
    [SerializeField] AudioMixerGroup amgSE;
    [SerializeField] AudioClip BGN, decision, hanko; //各種SE
    [SerializeField] Image imgCutIn; //カットイン用イメージ
    [SerializeField] bool isDebugTutorial = false; //チュートリアルdebug用

    PlayerController.stat_enum _oldPcStat;
    RankingManager.Save_ranking_item save_Ranking_Item;
    AudioSource audioSource;
    public TitleMonitor titleMonitor;
    bool isReachElapseTime = false;
    bool isPause = false;

    public Stat_global Stat { get; set; } //プレイ画面でのステート
    public enum Stat_global { init, play, jump, fly, result };
    public Stat_global _oldStat;
    public float elapseTime = 0f; //プレイ開始からの経過時間
    public static GameMaster gameMaster;
    public static PlayingManager playingManager;
    public List<CommentsData> allComments; //コメントのテキストデータ一覧
    public bool isTraining = false; //trueでトレーニングモード
    public bool isTutorial; //trueでトレーニングモードを開始するとチュートリアル開始
    public float resultDistance; //確定飛距離
    bool isNewRecord = false; //記録更新時true
    int _mayoCount = 0; //マヨ残数
    public bool isUsedMayo = false; //マヨを一度でも使用するとtrue
    public int mayoCount 
    {
        get { return _mayoCount; }
        set
        {
            if (_mayoCount < value)
            {
                audioSource.PlayOneShot(decision);
            }
            _mayoCount = value;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = amgSE;

        if (gameMaster == null) //デバッグ用（シーン直実行）
        {
            gameObject.AddComponent<RankingManager>();
            gameMaster = gameObject.AddComponent<GameMaster>();
            if (SceneManager.GetActiveScene().name != "Training")
            {
                gameMaster.gameMode = new GameMode(-1, "テスト", 3, true, true, testTrapezeLengs, -1f, "");
            }
            else
            {
                gameMaster.gameMode = gameMaster.gmTraining;
            }
            gameMaster.settings = new Settings("加藤純一", true, 1f, true, 0, Application.version, 100000f);
            gameMaster.am = am;
            gameMaster.isTutorial = isDebugTutorial;
        }
        if (gameMaster.gameMode.id == 99) isTraining = true;
        isTutorial = gameMaster.isTutorial;
        if (isTutorial) GetComponent<Tutorial>().enabled = true;
        //コメントテキストデータロード
        allComments = new List<CommentsData>();
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData0"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData1"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData2"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData3"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData4"));

        playingManager = this;

        Stat = Stat_global.init;
        _oldStat = Stat;
        _oldPcStat = playerController.stat;

    }

    void Start()
    {
        titleMonitor = GetComponent<TitleMonitor>();

        ugForAfterJump.alpha = 0f;
        ugForAfterJump.interactable = false;
        if (GameMaster.gameMaster.settings.audio_enabled)
            GameMaster.gameMaster.SetBgmVolume(GameMaster.gameMaster.settings.audio_volume);
        else
            GameMaster.gameMaster.SetBgmVolume(0f);



        StartCoroutine(InitEffect());
    }

    // Update is called once per frame
    void Update()
    {
        if (Stat == Stat_global.init)//開始時演出
        {

        }
        if (Stat == Stat_global.play || Stat == Stat_global.jump || Stat == Stat_global.fly)
        {
            elapseTime += Time.deltaTime; //経過時間計測用
        }
        if (!isReachElapseTime)
        {
            if (gameMaster.gameMode.timeLimit > 0 && elapseTime >= gameMaster.gameMode.timeLimit) //制限時間に到達
            {
                playerController.JUMP_on = true;
                var sld_JUMP = ugController.GetComponentInChildren<Slider>();
                sld_JUMP.value = 1f;
                ugController.alpha = 0f;
            }
        }
        if (Stat != _oldStat)
        {
            switch (Stat)
            {
                case Stat_global.play: //ジャンプまでの状態（漕ぐ＋ジャンプ待機）
                    break;
                case Stat_global.jump: //ジャンプ実行時
                    StartCoroutine(CutInProc());

                    break;
                case Stat_global.fly: //ジャンプ後（飛行中）
                    StartCoroutine(Fadein(ugForAfterJump));
                    StartCoroutine(Fadeout(ugForPlay));
                    if (!isTraining)
                    {
                        StartCoroutine(CameraRectChangeRight(cmrPlayerView, 1f));
                        cmrPlayerView.transform.parent = rb_Player.transform;
                    }
                    StartCoroutine(CameraRectChangeRight(cmrUI, 1f));
                    StartCoroutine(CameraRectChangeRight(cmrPublic, 1f));
                    StartCoroutine(CameraRectChangeLeft(cmrPlayer, 0f));
                    StartCoroutine(CameraRectChangeLeft(cmrUiPlayer, 0f));
                    if (!isTutorial) uiDistance.StartMessDistance();
                    if (isTraining)
                    {
                        cmrPublic.GetComponent<PublicCameraPerspective>().stat = PublicCameraPerspective.stat_publicCamera.jump;
                    }
                    else
                    {
                        cmrPublic.GetComponent<PublicCamera>().stat = PublicCamera.stat_publicCamera.jump;
                    }
                    txtVelocity.MassPoint = rb_Player;

                    break;
                case Stat_global.result: //リザルト画面

                    break;
                default:
                    break;
            }
        }
        if (playerController.stat != _oldPcStat && playerController.stat == PlayerController.stat_enum.finish) //リザルト確定条件
        {
            resultDistance = playerController.transform.position.z;
            Result(resultDistance, elapseTime);
            StartCoroutine(ShowResultUI());

            Stat = Stat_global.result;
        }
        _oldStat = Stat;
        _oldPcStat = playerController.stat;
    }

    //リザルト画面UI表示
    IEnumerator ShowResultUI()
    {
        //リザルト画面用カメラワークへの遷移準備。トレーニングモードと切り分け
        if (isTraining)
        {
            vcamPublic.m_Lens.FieldOfView = cmrPublic.fieldOfView;
            vcamPublic.transform.position = cmrPublic.transform.position;
            vcamPublic.transform.rotation = cmrPublic.transform.rotation;
            vcamPublic.gameObject.SetActive(true);
        }
        else
        {
            vcamPublic.m_Lens.OrthographicSize = cmrPublic.orthographicSize;
            vcamPublic.transform.position = cmrPublic.transform.position;
            vcamPublic.transform.rotation = cmrPublic.transform.rotation;
            vcamPublic.gameObject.SetActive(true);
            cmrPublic.GetComponent<PerspectiveSwitcher>().enabled = true;
            yield return null;
            cmrPublic.GetComponent<PerspectiveSwitcher>().SwitchToPerspectiveMode();
        }
        cmrPublic.GetComponent<CinemachineBrain>().enabled = true;
        // StartCoroutine(SmoothChangePerspective());
        yield return new WaitForSeconds(1f);
        //遷移開始
        vcamResult.gameObject.SetActive(true);
        vcamPublic.gameObject.SetActive(false);
        //飛距離確定
        uiDistance.Finish();
        yield return new WaitForSeconds(3.5f);
        //リザルト画面UI表示
        ugForResult.gameObject.SetActive(true);
        //称号判定
        var sttl = titleMonitor.Result(playerController.transform.position.z);
        if (!isTraining)
        {
            wndResultComment.SetActive(true);
            yield return new WaitForSeconds(1f);

            foreach (var item in titleMonitor.acquiredTitles)
            {
                var titleObject = gameMaster.titles.allTitles.FirstOrDefault(t => t.id == item);
                var titleElement = Instantiate(prfbTitleElement, ugTitleElements.transform);
                var txtTitleElement = titleElement.transform.Find("txtTitleElement");
                txtTitleElement.GetComponent<Text>().text = titleObject.name;

                if (gameMaster.acquiredTitles.FirstOrDefault(i => i == item) == 0)
                {
                    var txtNew = titleElement.transform.Find("txtNew");
                    txtNew.gameObject.SetActive(true);
                    gameMaster.acquiredTitles.Add(item);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        //リザルト画面用ボタン群表示
        ugButtonsForResult.SetActive(true);
        //リザルトボイス再生
        ResultVoice.resultVoice.Play(sttl);
        //レビュー依頼
        if (isNewRecord && gameMaster.settings.time_to_next_review < 0f)
        {
            gameMaster.settings.time_to_next_review = 36000f; //次回は10時間後
            gameMaster.Review();

        }
    }

    //リザルト確定処理
    public void Result(float distance, float time)
    {
        if (isTraining) return; //トレーニングモードはリザルト処理なし（ボタン表示のみのため）

        btnComment.interactable = false;
        btnCommentRush.interactable = false;



        var selectRecords = gameMaster.recordDatas.Find(x => x.game_mode_id == gameMaster.gameMode.id); //現在のゲームモードの成績レコード
        if (selectRecords == null) //デバッグ用
        {
            btnHighScore.GetComponent<BtnHighScore>().ChangeListener();
            ugNewRecord.SetActive(true);
            return;
        }

        selectRecords.total_time += time;
        gameMaster.settings.time_to_next_review -= time;
        selectRecords.play_count++;
        selectRecords.total_distance += Mathf.Abs(distance);

        //飛距離の正負で切り分け
        if ((distance > selectRecords.max_distance) || (distance == selectRecords.max_distance && time < selectRecords.timespan_maxdistance))
        {
            isNewRecord = true;
            selectRecords.max_distance = distance;
            selectRecords.timespan_maxdistance = time;
            if ((selectRecords.max_distance_best < selectRecords.max_distance) ||
                (selectRecords.max_distance_best == selectRecords.max_distance && selectRecords.timespan_maxdistance_best > selectRecords.timespan_maxdistance))
            {
                selectRecords.max_distance_best = selectRecords.max_distance;
                selectRecords.timespan_maxdistance_best = selectRecords.timespan_maxdistance;
            }
            save_Ranking_Item = RankingManager.Save_ranking_item.SAVE_RANKING_HIGH;

        }
        if ((distance < selectRecords.min_distance) || (distance == selectRecords.min_distance && time < selectRecords.timespan_mindistance))
        {
            isNewRecord = true;
            selectRecords.min_distance = distance;
            selectRecords.timespan_mindistance = time;
            if ((selectRecords.min_distance_best > selectRecords.min_distance) ||
                (selectRecords.min_distance_best == selectRecords.min_distance && selectRecords.timespan_mindistance_best > selectRecords.timespan_mindistance))
            {
                selectRecords.min_distance_best = selectRecords.min_distance;
                selectRecords.timespan_mindistance_best = selectRecords.timespan_mindistance;
            }
            save_Ranking_Item = RankingManager.Save_ranking_item.SAVE_RANKING_LOW;

        }
        if (isNewRecord) //記録更新時
        {
            btnRegisterHighScore.save_Ranking_Item = save_Ranking_Item;

            ugNewRecord.SetActive(true);
        }
        else
        {
            btnHighScore.GetComponent<BtnHighScore>().ChangeListener();
        }

        gameMaster.Save();
        return;
    }

    //一時停止切り替え。呼び出すたびに切り替わる
    public void SwitchPause()
    {

        if (!isPause)
        {
            Time.timeScale = 0f;
            isPause = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPause = false;
        }
    }

    //一時停止切り替え。trueを与えると一時停止
    public void SwitchPause(bool enabled)
    {

        if (enabled)
        {
            Time.timeScale = 0f;
            isPause = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPause = false;
        }
    }

    //開始時演出
    IEnumerator InitEffect()
    {
        if (!isTutorial)
        {
            var cgCvsPlayer = cvsPlayer.GetComponent<CanvasGroup>();
            var cgCvsPublic = cvsPublic.GetComponent<CanvasGroup>();
            cgCvsPlayer.interactable = false;
            cgCvsPublic.interactable = false;
            cmrUiPlayer.gameObject.SetActive(false);
            CanvasTop.canvasTop.FadeinScene(); //開始
            vcamFace.gameObject.SetActive(true);
            cmrFace.gameObject.SetActive(true);
            audioSource.PlayOneShot(BGN);
            yield return new WaitForSeconds(0.7f);
            StartVoice.startVoice.Play();
            yield return new WaitForSeconds(4.3f);
            CanvasTop.canvasTop.FadeoutScene(); //ブラックアウト
            yield return new WaitForSeconds(1f);
            cmrFace.gameObject.SetActive(false);
            vcamFace.gameObject.SetActive(false);
            mayoCount = gameMaster.gameMode.initialMayoCnt;
            player.transform.parent = rb_Trapeze.transform; //一時的にプレーヤーをブランコの子に移動
            playerController.SetAllIsKinematic(true);
            rb_Trapeze.transform.Rotate(new Vector3(5f, 0f, 0f)); //ブランコを傾ける

            var cj = rb_Trapeze.GetComponent<ConfigurableJoint>();
            rb_Trapeze.transform.Translate(cj.connectedAnchor - rb_Trapeze.transform.TransformPoint(cj.anchor)); //傾けた分位置補正
            yield return null;
            player.transform.parent = null; //ブランコの子から外す
            playerController.SetAllIsKinematic(false);
            rb_Trapeze.isKinematic = false;
            cgCvsPlayer.interactable = true;
            cgCvsPublic.interactable = true;
        }
        CanvasTop.canvasTop.FadeinScene();
        cmrUiPlayer.gameObject.SetActive(true);
        Stat = Stat_global.play;
    }

    //カットイン演出
    IEnumerator CutInProc()
    {
        if (gameMaster.settings.enable_voice)
        {
            SwitchPause(true);
            wndBackGround.SetActive(true);
            imgCutIn.gameObject.SetActive(true);
            yield return null;
            CutIn.cutIn.MoveIn();
            while (CutIn.cutIn.busy)
            {
                yield return null;
            }
            JumpVoice.jumpVoice.Play(() =>
            {
                CutIn.cutIn.MoveOut(() =>
                {
                    wndBackGround.SetActive(false);
                    Stat = Stat_global.fly;
                });

                SwitchPause(false);

            });
        }
        else
        {
            yield return null;
            Stat = Stat_global.fly;
        }

    }

    //フェードアウト
    IEnumerator Fadeout(CanvasGroup cg)
    {
        for (float f = 1f; f > 0f; f -= 0.2f)
        {
            cg.alpha = f;
            yield return null;
        }
        cg.interactable = false;

    }


    //フェードイン
    IEnumerator Fadein(CanvasGroup cg)
    {
        for (float f = 0f; f <= 1f; f += 0.2f)
        {
            cg.alpha = f;
            yield return null;
        }
        cg.interactable = true;

    }

    //ジャンプ後のコントローラー用画面をスライドアウトさせるために使用
    IEnumerator CameraRectChangeRight(Camera targetCamera, float width)
    {
        float nowWidth = targetCamera.rect.width;
        float diff = 0.05f;

        if (nowWidth < width)
        {
            while (nowWidth < width)
            {
                Rect rect = new Rect(targetCamera.rect);
                nowWidth += diff;
                rect.width = Mathf.Clamp01(nowWidth);
                targetCamera.rect = rect;
                yield return null;
            }
        }
        else if (nowWidth >= width)
        {
            while (nowWidth > width)
            {
                Rect rect = new Rect(targetCamera.rect);
                nowWidth -= diff;
                rect.width = Mathf.Clamp01(nowWidth);
                targetCamera.rect = rect;
                yield return null;
            }

        }
    }
    //ジャンプ後のcmrPublicを全画面化するために使用
    IEnumerator CameraRectChangeLeft(Camera targetCamera, float width)
    {
        float nowWidth = targetCamera.rect.width;
        float nowX = targetCamera.rect.x;
        float diff = 0.05f;

        if (nowWidth < width)
        {
            while (nowWidth < width)
            {
                Rect rect = new Rect(targetCamera.rect);

                nowWidth += diff;
                nowX -= diff;
                rect.width = Mathf.Clamp01(nowWidth);
                rect.x = Mathf.Clamp01(nowX);
                targetCamera.rect = rect;
                yield return null;
            }
        }
        else if (nowWidth >= width)
        {
            while (nowWidth > width)
            {
                Rect rect = new Rect(targetCamera.rect);
                nowWidth -= diff;
                nowX += diff;
                rect.width = Mathf.Clamp01(nowWidth);
                rect.x = Mathf.Clamp01(nowX);
                targetCamera.rect = rect;
                yield return null;
            }

        }
    }

}
