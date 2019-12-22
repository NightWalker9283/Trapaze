﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnDoneRetryForResult : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Done);
    }

    // Update is called once per frame
    void Done()
    {
        PlayingManager.gameMaster.MuteAudio(true);
        CanvasTop.canvasTop.ImmediatelyOutScene();

        AdsManager.interstitialAdManager.Show(() =>
        {
            PlayingManager.gameMaster.GameStart();
            PlayingManager.gameMaster.MuteAudio(false);
            Destroy(FindObjectOfType<PlayingManager>().gameObject);
        });



    }
}
