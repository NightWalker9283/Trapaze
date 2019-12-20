using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager adsManager;
    private RewardedAd rewardedAd;
    private string adUnitId;
    bool isEarn=false;
    // Start is called before the first frame update
    void Start()
    {
        adsManager = this;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = CreateRewardedAd();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private RewardedAd CreateRewardedAd()
    {
        var _rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _rewardedAd.LoadAd(request);

        return _rewardedAd;

    }

    public void UserChoseToWatchRewardedAd()
    {
        StartCoroutine(MonitorLoadingAd());
        return;

        IEnumerator MonitorLoadingAd()
        {
            while (!this.rewardedAd.IsLoaded())
            {
                yield return new WaitForSeconds(0.1f);
            }
            
                this.rewardedAd.Show();
        }

    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        WndMessage.wndMessage.ShowMessage("広告の再生に失敗しました。", () =>
        {
           
            PlayingManager.playingManager.SwitchPause(false);
        });
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (isEarn)
        {
            WndMessage.wndMessage.ShowMessage("マヨネーズを１つ獲得しました!", () =>
            {
                PlayingManager.gameMaster.SwitchAudio(true);
                PlayingManager.playingManager.mayoCount++;
                PlayingManager.playingManager.SwitchPause(false);
            });
        }
        else
        {
            WndMessage.wndMessage.ShowMessage("広告の再生が中断されました。", () =>
            {
                PlayingManager.gameMaster.SwitchAudio(true);
                PlayingManager.playingManager.SwitchPause(false);
            });
        }
        rewardedAd = CreateRewardedAd();
       
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isEarn = true;
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }
}
