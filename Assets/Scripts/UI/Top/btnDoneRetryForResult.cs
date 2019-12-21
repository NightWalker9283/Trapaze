using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnDoneRetryForResult : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Done);
    }

    // Update is called once per frame
    void Done()
    {
        PlayingManager.gameMaster.SwitchAudio(false);
        CanvasTop.canvasTop.ImmediatelyOutScene();

        AdsManager.interstitialAdManager.Show(() =>
        {
            PlayingManager.gameMaster.GameStart();
            PlayingManager.gameMaster.SwitchAudio(PlayingManager.gameMaster.settings.audio_enabled);
            Destroy(FindObjectOfType<PlayingManager>().gameObject);
        });



    }
}
