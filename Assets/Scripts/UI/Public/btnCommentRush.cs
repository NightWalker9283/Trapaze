using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
//弾幕コメントボダン。マヨ1つ以上所持で有効化
public class btnCommentRush : MonoBehaviour
{


    [SerializeField] AudioClip ac;
    [SerializeField] GameObject PrefComment, wndBackGround, imgCutIn;
    AudioSource audioSource;
    Button btn;



    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        btn.onClick.AddListener(ShotComments);

    }
    private void Update()
    {
        if (PlayingManager.playingManager.mayoCount <= 0 && btn.interactable == true)
        {
            btn.interactable = false;
        }

        if (PlayingManager.playingManager.mayoCount > 0 && btn.interactable == false)
        {
            btn.interactable = true;
        }
    }

    //弾幕コメ発射
    public void ShotComments()
    {
        PlayingManager.playingManager.isUsedMayo = true;
        if (PlayingManager.playingManager.mayoCount > 0)
        {
            PlayingManager.playingManager.mayoCount--;
            StartCoroutine(CutInProc(() =>
            {
                StartCoroutine(CommentRush());
            }));
        }
    }
    IEnumerator CommentRush()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(PrefComment, PlayingManager.playingManager.cvsPublic.transform);
            yield return new WaitForSeconds(Random.Range(0.02f, 0.15f));
        }

    }
    //弾幕コメ発射時のカットイン演出
    IEnumerator CutInProc(System.Action action)
    {
        if (GameMaster.gameMaster.settings.enable_voice)
        {
            PlayingManager.playingManager.SwitchPause(true);
            wndBackGround.SetActive(true);
            imgCutIn.gameObject.SetActive(true);
            yield return null;
            CutIn.cutIn.MoveIn();
            while (CutIn.cutIn.busy)
            {
                yield return null;
            }
            CommentVoice.commentVoice.Play(() =>
            {
                CutIn.cutIn.MoveOut(() =>
                {
                    StartCoroutine(Finish(action));
                });



            });
        }
        else
        {
            audioSource.PlayOneShot(ac);
            yield return new WaitForSeconds(3f);
            action();
        }

    }

    IEnumerator Finish(System.Action action)
    {
        wndBackGround.SetActive(false);
        PlayingManager.playingManager.SwitchPause(false);
        yield return new WaitForSeconds(3f);
        action();
    }
}


