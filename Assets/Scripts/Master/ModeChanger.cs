using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeChanger : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;
    [SerializeField] Text txtModeName, txtModeLength, txtModeTimeLimit, txtModeDetail;

    List<GameMode> gameModes ;

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

    public void ChangeModeNext()
    {
        idx = (idx + 1) % gameModes.Count;
        gameMaster.gameMode = gameModes[idx];
        SetModeTexts();
    }

    public void ChangeModePrev()
    {
        idx = (idx - 1 + gameModes.Count) % gameModes.Count;
        gameMaster.gameMode = gameModes[idx];
        SetModeTexts();
    }

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
