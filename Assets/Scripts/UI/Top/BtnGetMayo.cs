using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnGetMayo : MonoBehaviour
{
    [SerializeField] GameObject wndBackGround, wndMayo;
    [SerializeField] Mayo mayo;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GetMayo);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetMayo()
    {

        wndMayo.SetActive(false);
        wndBackGround.SetActive(false);
        
        PlayingManager.gameMaster.MuteAudio(true);
#if DEBUG
        WndMessage.wndMessage.ShowMessage("マヨネーズを１つ獲得しました。", () =>
        {
            PlayingManager.gameMaster.MuteAudio(false);
            PlayingManager.playingManager.mayoCount++;
            PlayingManager.playingManager.SwitchPause(false);
        });
#else
        
        AdsManager.rewardAdManager.UserChoseToWatchRewardedAd();

#endif
        mayo.Finish();


    }
}
