using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] uiDistance txtDistance;
    [SerializeField] CanvasGroup ugForPlay, ugController, ugForAfterJump, ugForResult;
    [SerializeField] Camera cmrPlayerView, cmrPublic, cmrPlayer, cmrUI, cmrFace;
    [SerializeField] Rigidbody rb_Player,rb_Trapeze;
    [SerializeField] PlayerController playerController;
    [SerializeField] uiVelocity txtVelocity;
    [SerializeField] float testTrapezeLengs = 8f;
    [SerializeField] GameObject ugNewRecord;
    [SerializeField] AudioMixer am;

    PlayerController.stat_enum _oldPcStat;
    bool isReachElapseTime = false;

    public stat_global stat { get; set; } = stat_global.init;
    public enum stat_global { init, play, pause, jump, result };
    public stat_global _oldStat, statCache;
    public float elapseTime = 0f;
    public static GameMaster gameMaster;
    public static PlayingManager playingManager;

    // Start is called before the first frame update
    private void Awake()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null)
        {
            gameMaster = gameObject.AddComponent<GameMaster>();
            gameMaster.gameMode = new GameMode(-1, "テスト", testTrapezeLengs, -60f, true, "");
            gameMaster.settings = new Settings(true, 1f);
            gameMaster.am = am;
        }

        playingManager = this;


        stat = stat_global.play;
        _oldStat = stat;
        _oldPcStat = playerController.stat;

    }

    void Start()
    {
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
        if (stat == stat_global.init)
        {

        }
        if (stat == stat_global.play || stat == stat_global.jump)
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
        if (stat != _oldStat)
        {
            switch (stat)
            {
                case stat_global.play:
                    break;
                case stat_global.jump:
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
                case stat_global.pause:


                    break;
                case stat_global.result:

                    break;
                default:
                    break;
            }
        }
        if (playerController.stat != _oldPcStat && playerController.stat == PlayerController.stat_enum.finish)
        {
            Result(playerController.transform.position.z, elapseTime);
            ugForResult.gameObject.SetActive(true);
            stat = stat_global.result;
        }
        _oldStat = stat;
        _oldPcStat = playerController.stat;
    }
    public void Result(float distance, float time)
    {
        bool isNewRecord = false;
        var selectRecords = gameMaster.recordDatas.Find(x => x.game_mode_id == gameMaster.gameMode.id);
        if (selectRecords == null) return;
        selectRecords.total_time += time;
        selectRecords.play_count++;
        if (distance > 0f) selectRecords.total_distance += distance;
        if ((distance > selectRecords.max_distance) || (distance == selectRecords.max_distance && time < selectRecords.timespan_maxdistance))
        {
            isNewRecord = true;
            selectRecords.max_distance = distance;
            selectRecords.timespan_maxdistance = time;
        }
        if ((distance < selectRecords.min_distance) || (distance == selectRecords.min_distance && time < selectRecords.timespan_mindistance))
        {
            isNewRecord = true;
            selectRecords.min_distance = distance;
            selectRecords.timespan_mindistance = time;
        }
        if (isNewRecord)
        {
            ugNewRecord.SetActive(true);
        }
    }

    public void SwitchPause()
    {

        if (stat != stat_global.pause)
        {
            Time.timeScale = 0f;
            statCache = stat;
            stat = stat_global.pause;
        }
        else if (stat == stat_global.pause)
        {
            Time.timeScale = 1f;
            stat = statCache;
        }
    }

    IEnumerator InitEffect()
    {
        CanvasTop.canvasTop.FadeinScene();
        cmrFace.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        CanvasTop.canvasTop.FadeoutScene();
        yield return new WaitForSeconds(1f);
        cmrFace.gameObject.SetActive(false);
        rb_Trapeze.isKinematic = false;
        CanvasTop.canvasTop.FadeinScene();
        
        stat = stat_global.play;
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
