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

public class PlayingManager : MonoBehaviour
{
    [SerializeField] UiDistance uiDistance;
    [SerializeField] CanvasGroup ugForPlay, ugController, ugForAfterJump, ugForResult;

    [SerializeField] public Camera cmrPlayerView, cmrPublic, cmrPlayer, cmrUI, cmrFace;
    [SerializeField] CinemachineVirtualCamera vcamPublic, vcamFace, vcamResult;
    [SerializeField] Rigidbody rb_Player, rb_Trapeze;
    [SerializeField] public Canvas cvsPublic, cvsPlayer, cvsTop;
    [SerializeField] public Rigidbody playerControlPoint;
    [SerializeField] PlayerController playerController;
    [SerializeField] uiVelocity txtVelocity;
    [SerializeField] float testTrapezeLengs = 8f;
    [SerializeField] GameObject ugNewRecord, ugButtonsForResult;
    [SerializeField] GameObject prfbTitleElement;
    [SerializeField] GameObject btnHighScore;
    [SerializeField] GameObject wndBackGround, wndResultComment, ugTitleElements;
    [SerializeField] Button btnComment, btnCommentRush;
    [SerializeField] AudioMixer am;
    [SerializeField] AudioMixerGroup amgSE;
    [SerializeField] AudioClip BGN, decision, hanko;
    [SerializeField] Image imgCutIn;

    PlayerController.stat_enum _oldPcStat;
    RankingManager.Save_ranking_item save_Ranking_Item;
    AudioSource audioSource;
    TitleMonitor titleMonitor;
    bool isReachElapseTime = false;
    bool isPause = false;

    public Stat_global Stat { get; set; }
    public enum Stat_global { init, play, jump, fly, result };
    public Stat_global _oldStat;
    public float elapseTime = 0f;
    public static GameMaster gameMaster;
    public static PlayingManager playingManager;
    public List<CommentsData> allComments;
    int _mayoCount = 0;
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

        if (gameMaster == null)
        {
            gameObject.AddComponent<RankingManager>();
            gameMaster = gameObject.AddComponent<GameMaster>();
            gameMaster.gameMode = new GameMode(-1, "テスト", testTrapezeLengs, -60f, true, "");
            gameMaster.settings = new Settings("加藤純一", true, 1f, true);
            gameMaster.am = am;
        }
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
        if (GameMaster.gameMaster.settings.audio_enabled)
            GameMaster.gameMaster.SetBgmVolume(GameMaster.gameMaster.settings.audio_volume);
        else
            GameMaster.gameMaster.SetBgmVolume(0f);



        StartCoroutine(InitEffect());
    }

    // Update is called once per frame
    void Update()
    {
        if (Stat == Stat_global.init)
        {

        }
        if (Stat == Stat_global.play || Stat == Stat_global.jump || Stat == Stat_global.fly)
        {
            elapseTime += Time.deltaTime;
        }
        if (!isReachElapseTime)
        {
            if (gameMaster.gameMode.timeLimit > 0 && elapseTime >= gameMaster.gameMode.timeLimit)
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
                case Stat_global.play:
                    break;
                case Stat_global.jump:
                    StartCoroutine(CutInProc());

                    break;
                case Stat_global.fly:
                    StartCoroutine(Fadein(ugForAfterJump));
                    StartCoroutine(Fadeout(ugForPlay));
                    StartCoroutine(CameraRectChangeRight(cmrUI, 1f));
                    StartCoroutine(CameraRectChangeRight(cmrPublic, 1f));
                    StartCoroutine(CameraRectChangeRight(cmrPlayerView, 1f));
                    StartCoroutine(CameraRectChangeLeft(cmrPlayer, 0f));
                    uiDistance.StartMessDistance();
                    cmrPlayerView.transform.parent = rb_Player.transform;

                    cmrPublic.GetComponent<PublicCamera>().stat = PublicCamera.stat_publicCamera.jump;
                    txtVelocity.MassPoint = rb_Player;

                    break;
                case Stat_global.result:

                    break;
                default:
                    break;
            }
        }
        if (playerController.stat != _oldPcStat && playerController.stat == PlayerController.stat_enum.finish)
        {
            Result(playerController.transform.position.z, elapseTime);
            StartCoroutine(ShowResultUI());

            Stat = Stat_global.result;
        }
        _oldStat = Stat;
        _oldPcStat = playerController.stat;
    }

    IEnumerator ShowResultUI()
    {
        vcamPublic.m_Lens.OrthographicSize = cmrPublic.orthographicSize;
        vcamPublic.transform.position = cmrPublic.transform.position;
        vcamPublic.transform.rotation = cmrPublic.transform.rotation;
        vcamPublic.gameObject.SetActive(true);
        cmrPublic.GetComponent<PerspectiveSwitcher>().enabled = true;
        yield return null;
        cmrPublic.GetComponent<PerspectiveSwitcher>().SwitchToPerspectiveMode();
        cmrPublic.GetComponent<CinemachineBrain>().enabled = true;
        // StartCoroutine(SmoothChangePerspective());
        yield return null;
        vcamResult.gameObject.SetActive(true);
        vcamPublic.gameObject.SetActive(false);
        uiDistance.Finish();
        yield return new WaitForSeconds(1.5f);
        ugForResult.gameObject.SetActive(true);
        wndResultComment.SetActive(true);
        yield return new WaitForSeconds(1f);

        titleMonitor.Result(playerController.transform.position.z);
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

        ugButtonsForResult.SetActive(true);
        ResultVoice.resultVoice.Play();
    }


    public void Result(float distance, float time)
    {
        bool isNewRecord = false;
        btnComment.interactable = false;
        btnCommentRush.interactable = false;



        var selectRecords = gameMaster.recordDatas.Find(x => x.game_mode_id == gameMaster.gameMode.id);
        if (selectRecords == null)
        {
            btnHighScore.GetComponent<BtnHighScore>().ChangeListener();
            return;
        }
        selectRecords.total_time += time;
        selectRecords.play_count++;
        if (distance > 0f) selectRecords.total_distance += distance;
        if ((distance > selectRecords.max_distance) || (distance == selectRecords.max_distance && time < selectRecords.timespan_maxdistance))
        {
            isNewRecord = true;
            selectRecords.max_distance = distance;
            selectRecords.timespan_maxdistance = time;
            save_Ranking_Item = RankingManager.Save_ranking_item.SAVE_RANKING_HIGH;

        }
        if ((distance < selectRecords.min_distance) || (distance == selectRecords.min_distance && time < selectRecords.timespan_mindistance))
        {
            isNewRecord = true;
            selectRecords.min_distance = distance;
            selectRecords.timespan_mindistance = time;
            save_Ranking_Item = RankingManager.Save_ranking_item.SAVE_RANKING_LOW;

        }
        if (isNewRecord)
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


    IEnumerator InitEffect()
    {


        CanvasTop.canvasTop.FadeinScene();
        vcamFace.gameObject.SetActive(true);
        cmrFace.gameObject.SetActive(true);
        audioSource.PlayOneShot(BGN);
        yield return new WaitForSeconds(0.7f);
        StartVoice.startVoice.Play();
        yield return new WaitForSeconds(4.3f);
        CanvasTop.canvasTop.FadeoutScene();
        yield return new WaitForSeconds(1f);
        cmrFace.gameObject.SetActive(false);
        vcamFace.gameObject.SetActive(false);
        rb_Trapeze.isKinematic = false;
        CanvasTop.canvasTop.FadeinScene();

        Stat = Stat_global.play;
    }

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


    IEnumerator Fadeout(CanvasGroup cg)
    {
        for (float f = 1f; f > 0f; f -= 0.2f)
        {
            cg.alpha = f;
            yield return null;
        }
        cg.interactable = false;

    }



    IEnumerator Fadein(CanvasGroup cg)
    {
        for (float f = 0f; f <= 1f; f += 0.2f)
        {
            cg.alpha = f;
            yield return null;
        }
        cg.interactable = true;

    }


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
