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
    [SerializeField] uiDistance txtDistance;
    [SerializeField] CanvasGroup ugForPlay, ugController, ugForAfterJump, ugForResult;
   
    [SerializeField] public Camera cmrPlayerView, cmrPublic, cmrPlayer, cmrUI, cmrFace;
    [SerializeField] CinemachineVirtualCamera vcamPublic, vcamFace, vcamResult;
    [SerializeField] Rigidbody rb_Player, rb_Trapeze;
    [SerializeField] public Canvas cvsPublic,cvsPlayer,cvsTop;
    [SerializeField] public Rigidbody playerControlPoint;
    [SerializeField] PlayerController playerController;
    [SerializeField] uiVelocity txtVelocity;
    [SerializeField] float testTrapezeLengs = 8f;
    [SerializeField] GameObject ugNewRecord;
    [SerializeField] GameObject btnHighScore;
    [SerializeField] AudioMixer am;

    PlayerController.stat_enum _oldPcStat;
    RankingManager.Save_ranking_item save_Ranking_Item;
    bool isReachElapseTime = false;

    public Stat_global Stat { get; set; }
    public enum Stat_global { init, play, pause, jump, result };
    public Stat_global _oldStat, statCache;
    public float elapseTime = 0f;
    public static GameMaster gameMaster;
    public static PlayingManager playingManager;
    public List<CommentsData> allComments;

    // Start is called before the first frame update
    private void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
        {
            gameMaster = gameObject.AddComponent<GameMaster>();
            gameMaster.gameMode = new GameMode(-1, "テスト", testTrapezeLengs, -60f, true, "");
            gameMaster.settings = new Settings("加藤純一",true, 1f);
            gameMaster.am = am;
        }

        playingManager = this;

        Stat = Stat_global.init;
        _oldStat = Stat;
        _oldPcStat = playerController.stat;

    }

    void Start()
    {
        ugForAfterJump.alpha = 0f;
        if (GameMaster.gameMaster.settings.audio_enabled)
            GameMaster.gameMaster.SetBgmVolume(GameMaster.gameMaster.settings.audio_volume);
        else
            GameMaster.gameMaster.SetBgmVolume(0f);


        allComments = new List<CommentsData>();
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData0"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData1"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData2"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData3"));
        allComments.Add(Resources.Load<CommentsData>("Comments/CommentsData4"));

        StartCoroutine(InitEffect());
    }

    // Update is called once per frame
    void Update()
    {
        if (Stat == Stat_global.init)
        {

        }
        if (Stat == Stat_global.play || Stat == Stat_global.jump)
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
                    StartCoroutine(Fadein(ugForAfterJump));
                    StartCoroutine(Fadeout(ugForPlay));
                    StartCoroutine(CameraRectChangeRight(cmrUI, 1f));
                    StartCoroutine(CameraRectChangeRight(cmrPublic, 1f));
                    StartCoroutine(CameraRectChangeRight(cmrPlayerView, 1f));
                    StartCoroutine(CameraRectChangeLeft(cmrPlayer, 0f));
                    txtDistance.StartMessDistance();
                    cmrPlayerView.transform.parent = rb_Player.transform;

                    cmrPublic.GetComponent<PublicCamera>().stat = PublicCamera.stat_publicCamera.jump;
                    txtVelocity.MassPoint = rb_Player;
                    break;
                case Stat_global.pause:


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
        yield return new WaitForSeconds(0.5f);
        ugForResult.gameObject.SetActive(true);
    }

    
    public void Result(float distance, float time)
    {
        bool isNewRecord = false;
        
        
        var selectRecords = gameMaster.recordDatas.Find(x => x.game_mode_id == gameMaster.gameMode.id);
        if (selectRecords == null) return ;
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
            btnHighScore.GetComponent<Button>().interactable=true;
            ugNewRecord.SetActive(true);
        }
        gameMaster.Save();
        return;
    }

    public void SwitchPause()
    {

        if (Stat != Stat_global.pause)
        {
            Time.timeScale = 0f;
            statCache = Stat;
            Stat = Stat_global.pause;
        }
        else if (Stat == Stat_global.pause)
        {
            Time.timeScale = 1f;
            Stat = statCache;
        }
    }



    IEnumerator InitEffect()
    {
        CanvasTop.canvasTop.FadeinScene();
        vcamFace.gameObject.SetActive(true);
        cmrFace.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        CanvasTop.canvasTop.FadeoutScene();
        yield return new WaitForSeconds(1f);
        cmrFace.gameObject.SetActive(false);
        vcamFace.gameObject.SetActive(false);
        rb_Trapeze.isKinematic = false;
        CanvasTop.canvasTop.FadeinScene();

        Stat = Stat_global.play;
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
