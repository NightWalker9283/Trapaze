using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ランキング画面。正負の記録切り替え用トグルグループの制御
public class DirectionElementForRanking : MonoBehaviour
{
    
    
    public RankingManager.Save_ranking_item directionType;
    UIRanking _uIRanking;
    UIRanking uIRanking
    {
        get
        {
            if (_uIRanking == null) _uIRanking = UIRanking.uIRanking;
            return _uIRanking;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnChangeDirectionElement);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnChangeDirectionElement(bool isOn)
    {
        if (isOn)
        {
            uIRanking.ResetCategoryToggles();
            uIRanking.JudgmentTggCategory();
            uIRanking.CreateRankingViews();
        }

    }
}
