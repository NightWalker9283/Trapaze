using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionElementForRanking : MonoBehaviour
{
    
    
    public RankingManager.Save_ranking_item directionType;
    UIRanking uIRanking=UIRanking.uIRanking;

    // Start is called before the first frame update
    void Start()
    {
       
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
            uIRanking.CreateRankingViews();
        }

    }
}
