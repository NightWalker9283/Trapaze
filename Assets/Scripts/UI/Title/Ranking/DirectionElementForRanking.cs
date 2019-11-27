﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionElementForRanking : MonoBehaviour
{
    
    
    public RankingManager.Save_ranking_item directionType;
    UIRanking uIRanking;

    // Start is called before the first frame update
    void Start()
    {
        uIRanking = transform.parent.GetComponent<UIRanking>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnChangeModeElement(bool isOn)
    {
        
        uIRanking.JudgmentTggCategory();
        if (isOn)
        {
            uIRanking.CreateRankingRecords();
        }

    }
}
