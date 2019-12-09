﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class btnCommentRush : MonoBehaviour
{


    
    [SerializeField] GameObject PrefComment,wndBackGround,imgCutIn;
    
    Button btn;



    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();


        btn.onClick.AddListener(ShotComments);

    }
    private void Update()
    {
        if(PlayingManager.playingManager.mayoCount<=0 && btn.interactable == true)
        {
            btn.interactable = false;
        }

        if(PlayingManager.playingManager.mayoCount>0 && btn.interactable==false)
        {
            btn.interactable = true;
        }
    }

    // Update is called once per frame
    void ShotComments()
    {
        StartCoroutine(CutInProc(() =>
        {
            PlayingManager.playingManager.mayoCount--;
            StartCoroutine(CommentRush());
        }));
        
    }
    IEnumerator CommentRush()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(PrefComment, PlayingManager.playingManager.cvsPublic.transform);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        }

    }

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
                    wndBackGround.SetActive(false);
                    PlayingManager.playingManager.SwitchPause(false);
                    action();
                });

                

            });
        }
        
    }
}


