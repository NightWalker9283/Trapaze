using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnHighScore : MonoBehaviour
{
    [SerializeField] GameObject wndBackGround, wndHighScore,cvsRanking;
    bool isChangedLister = false;


    // Start is called before the first frame update
    void Start()
    {

        var btn = GetComponent<Button>();

        if(!isChangedLister)
        {
            btn.onClick.AddListener(() =>
            {
                wndBackGround.SetActive(true);
                wndHighScore.SetActive(true);
            });
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeListener()
    {
        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            cvsRanking.SetActive(true);
        });
        isChangedLister = true;
    }
}
