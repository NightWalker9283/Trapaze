using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ランキング画面。トップ、ライバルの表示の切り替え用トグルグループの制御
public class CategoryElementForRanking : MonoBehaviour
{
    
    public enum categorys
    {
        TOP,
        RIVAL
    }
    public categorys category;
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
        GetComponent<Toggle>().onValueChanged.AddListener(OnChangeCategoryElement);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnChangeCategoryElement(bool isOn)
    {
        
        if (isOn)
        {
            
            uIRanking.CreateRankingViews();
        }

    }
}
