using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//設定ウィンドウ。ボイスON・OFFチェックボックス
public class tglEnableVoice : MonoBehaviour
{
    Toggle tgl;
    Settings settings;
    bool oldVal;
    // Start is called before the first frame update
    void Start()
    {
        tgl = GetComponent<Toggle>();
        settings = GameMaster.gameMaster.settings;
        tgl.isOn = settings.enable_voice;
        oldVal = tgl.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldVal != tgl.isOn)
        {
            if (tgl.isOn)
            {
                GameMaster.gameMaster.VoiceOn();
            }
            else
            {
                GameMaster.gameMaster.VoiceOff();
            }

            settings.enable_voice = tgl.isOn;


            GameMaster.gameMaster.Save();
        }
        oldVal = tgl.isOn;
    }
}
