using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeChanger : MonoBehaviour
{
    [SerializeField] Text txtModeName, txtModeLength, txtModeTimeLimit, txtModeDetail;

    List<GameMode> gameModes = new List<GameMode>();

    int idx = 0;
    private void CreateGameModes()
    {
        gameModes.Add(new GameMode("サクッと", 4f, 60f, false, "少しの空き時間でサクッと遊びたいときに。"));
        gameModes.Add(new GameMode("本気", 8f, -1f, false, "時間無制限1本勝負！"));
        gameModes.Add(new GameMode("チャレンジャー", 20f, -1, false, "夢の超巨大ブランコ。異常に眠くなります。睡眠導入、精神安定などの用途にご利用ください。"));

    }
    private void Awake()
    {
        CreateGameModes();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameMaster.gameMode = gameModes[0];
        SetModeTexts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeModeNext()
    {
        GameMaster.gameMode = gameModes[idx];
        idx = (idx + 1) % gameModes.Count;
        SetModeTexts();
    }

    public void ChangeModePrev()
    {
        GameMaster.gameMode = gameModes[idx];
        idx = (idx - 1 + gameModes.Count) % gameModes.Count;
        SetModeTexts();
    }

    private void SetModeTexts()
    {
        txtModeName.text = GameMaster.gameMode.name;
        txtModeLength.text = "長さ：" + GameMaster.gameMode.trapezeLength.ToString("F1") + "m";

        var tl = GameMaster.gameMode.timeLimit;
        if (tl >= 0)
        {
            txtModeTimeLimit.text = "制限時間：" + ((int)(tl / 60f)).ToString("D1") + ":" + ((int)(tl % 60f)).ToString("D2");
        }
        else
        {
            txtModeTimeLimit.text = "制限時間：∞";
        }
        txtModeDetail.text = GameMaster.gameMode.detail;
    }
}
