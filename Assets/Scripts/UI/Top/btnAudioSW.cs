using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//プレイ画面設定ウィンドウのオーディオON/OFF切り替えボタン
public class btnAudioSW : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite sprOn, sprOff;
    bool oldEnabled; 

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GameMaster.gameMaster.SwitchAudio);
        if (GameMaster.gameMaster.settings.audio_enabled) image.sprite = sprOn;
        else image.sprite = sprOff;

    }
    private void OnEnable()
    {
    }

    private void Update()
    {
        
        //Debug.Log(oldEnabled);
        if (oldEnabled !=GameMaster.gameMaster.settings.audio_enabled) {
            if (GameMaster.gameMaster.settings.audio_enabled) image.sprite = sprOn;
            else image.sprite = sprOff;
        }
        oldEnabled = GameMaster.gameMaster.settings.audio_enabled;
    }
}
