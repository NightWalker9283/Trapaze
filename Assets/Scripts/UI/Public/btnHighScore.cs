using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//リザルト画面。ランキングへのハイスコア登録ボタン。登録後or記録更新時以外はランキングボード表示
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
                wndHighScore.SetActive(true); //ハイスコア登録ウィンドウ表示
            });
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    //ランキングボード表示へリスナー変更
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
