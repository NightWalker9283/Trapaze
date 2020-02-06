using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//プレイ画面。ギブアップウィンドウのDONEボタン
public class btnDoneGiveUp : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Done);
    }

    // Update is called once per frame
    void Done()
    {
        PlayingManager.gameMaster.MuteAudio(true);
        CanvasTop.canvasTop.ImmediatelyOutScene();

        AdsManager.interstitialAdManager.Show(() =>
        {
            
            PlayingManager.gameMaster.Title();
            PlayingManager.gameMaster.MuteAudio(false);
            Destroy(FindObjectOfType<GameMaster>().gameObject);
            Destroy(FindObjectOfType<PlayingManager>().gameObject);
        });
    }
}
