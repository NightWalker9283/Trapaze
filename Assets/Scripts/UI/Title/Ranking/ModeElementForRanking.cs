using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ModeElementForRanking : MonoBehaviour
{
    
    public int id = 0;
    UIRanking uIRanking;
    
    // Start is called before the first frame update
    void Start()
    {
        uIRanking= transform.parent.GetComponent<UIRanking>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnChangeModeElement(bool isOn)
    {
        uIRanking.ResetToggles();
        uIRanking.JudgmentTggCategory();
        if (isOn)
        {
            uIRanking.CreateRankingRecords();  
        }

    }
    
}
