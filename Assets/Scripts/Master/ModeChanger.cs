using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//タイトル画面用
//現在選択中のゲームモードを管理
public class ModeChanger : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;
    [SerializeField] Text txtModeName, txtModeLength, txtModeTimeLimit, txtModeDetail;　//選択中のモードの各種情報表示先
    [SerializeField] GameObject cgChangeTrainingLen;    //トレーニングモードのブランコ長変更用UI
    List<GameMode> gameModes;

    int idx = 0;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        gameModes = gameMaster.gameModes;
        gameMaster.gameMode = gameModes[0];
        SetModeTexts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //右ボタン処理
    public void ChangeModeNext()
    {
        idx++;
        if (idx > gameModes.Count) idx = 0;
        SetGameMode(idx);

    }

    //左ボタン処理
    public void ChangeModePrev()
    {
        idx--;
        if (idx < 0) idx = gameModes.Count;
        SetGameMode(idx);
    }


    public void SetGameMode(int index)
    {
        if (index < gameModes.Count)
        {
            cgChangeTrainingLen.SetActive(false);
            gameMaster.gameMode = gameModes[index];
        }
        if (index == gameModes.Count)　//トレーニングモード選択中
        {
            cgChangeTrainingLen.SetActive(true);
            gameMaster.gameMode = gameMaster.gmTraining;
        }
        SetModeTexts();
    }

    //トレーニングモードブランコ長増
    public void UpTrainingLen()
    {
        var len = gameMaster.gmTraining.trapezeLength;
        if (len < 15f)
        {
            gameMaster.gmTraining.trapezeLength += 1f;
            SetGameMode(idx);
        }
    }

    //ブランコ長減
    public void DownTrainingLen()
    {
        var len = gameMaster.gmTraining.trapezeLength;
        if (len >4f)
        {
            gameMaster.gmTraining.trapezeLength -= 1f;
            SetGameMode(idx);
        }
    }

    //選択中モードの各種情報表示
    private void SetModeTexts()
    {
        txtModeName.text = gameMaster.gameMode.name;
        txtModeLength.text = "長さ：" + gameMaster.gameMode.trapezeLength.ToString("F1") + "m";

        var tl = gameMaster.gameMode.timeLimit;
        if (tl >= 0)
        {
            txtModeTimeLimit.text = "制限時間：" + tl.ToString("F0") + "秒";
        }
        else
        {
            txtModeTimeLimit.text = "制限時間：∞";
        }
        txtModeDetail.text = gameMaster.gameMode.detail;
    }
}
