﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiSwitchView : MonoBehaviour
{
    enum stat_view { playerView, publicView };
    stat_view stat = stat_view.publicView;
    [SerializeField] Image image;
    [SerializeField] Sprite sprPlayerView, sprPublicView;
    [SerializeField] Camera cmrPlayerView, cmrPublicView;
    [SerializeField] Canvas cnvsPublic;
        // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SwitchView);
    }

    // Update is called once per frame
    void SwitchView()
    {
        if (stat == stat_view.playerView)
        {
            stat = stat_view.publicView;
            image.sprite = sprPlayerView;
            cmrPlayerView.depth = -1f;
            cmrPublicView.depth = 0f;
            cnvsPublic.worldCamera = cmrPublicView;
        }
        else if (stat == stat_view.publicView)
        {
            stat = stat_view.playerView;
            image.sprite = sprPublicView;
            cmrPlayerView.depth = 0f;
            cmrPublicView.depth = -1f;
            cnvsPublic.worldCamera = cmrPlayerView;
        }
    }
}
