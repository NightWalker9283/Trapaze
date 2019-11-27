using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryElementForRanking : MonoBehaviour
{
    
    public enum categorys
    {
        TOP,
        RIVAL
    }
    public categorys category;
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
        
        if (isOn)
        {
            uIRanking.CreateRankingRecords();
        }

    }
}
