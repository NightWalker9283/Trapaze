//#define TEST_AD
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

//リワード広告管理用
public class RewardAdManager : MonoBehaviour
{

    private RewardedAd rewardedAd;
    private bool isSuccessLoad = true;
    bool isEarn = false;
//リワード
#if TEST_AD　//テスト

#if UNITY_ANDROID
    public static string adUnitId = "ca-app-pub-3940256099942544/5224354917";   //Android
#elif UNITY_IPHONE
    public static string adUnitId = "ca-app-pub-3940256099942544/1712485313";   //iOS
#else
    public static string adUnitId = "unexpected_platform";
#endif

#else   //本番
#if UNITY_ANDROID
    public static string adUnitId = "ca-app-pub-1610123728558925/7613636021";   //Android
#elif UNITY_IPHONE
    public static string adUnitId = "ca-app-pub-1610123728558925/3902795183";   //iOS
#else
    public static string adUnitId = "unexpected_platform";
#endif
#endif

    // Start is called before the first frame update
    void Start()
    {



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
        isSuccessLoad = true;
        return _rewardedAd;

    }


    //広告の再生開始。リワード広告再生が選択された場合に呼び出す。
    public void UserChoseToWatchRewardedAd()
    {
        StartCoroutine(MonitorLoadingAd());
        return;

        IEnumerator MonitorLoadingAd()
        {
            while (!this.rewardedAd.IsLoaded() && isSuccessLoad)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (isSuccessLoad) { this.rewardedAd.Show(); }
            else
            {
                WndMessage.wndMessage.ShowMessage("広告の再生に失敗しました。", () =>
                {
                    PlayingManager.gameMaster.MuteAudio(false);
                    PlayingManager.playingManager.SwitchPause(false);
                });
            }
        }

    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        isSuccessLoad = false;
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
            PlayingManager.gameMaster.MuteAudio(false);
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
                PlayingManager.gameMaster.MuteAudio(false);
                PlayingManager.playingManager.mayoCount++;
                PlayingManager.playingManager.SwitchPause(false);
            });
        }
        else
        {
            WndMessage.wndMessage.ShowMessage("広告の再生が中断されました。", () =>
            {
                PlayingManager.gameMaster.MuteAudio(false);
                PlayingManager.playingManager.SwitchPause(false);
            });
        }
        isEarn = false;
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
