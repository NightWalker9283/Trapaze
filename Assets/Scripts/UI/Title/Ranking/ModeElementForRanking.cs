using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;


//ランキング画面。モード切り替え用トグルグループの制御
public class ModeElementForRanking : MonoBehaviour
{

    public int id = 0;
    public object MyProperty { get; set; }
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
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnChangeModeElement(bool isOn)
    {
        if (isOn)
        {
            uIRanking.JudgmentTggCategory();
            uIRanking.ResetAllToggles();
            uIRanking.CreateRankingViews();
        }

    }

}
