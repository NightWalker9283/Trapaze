using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitial;
    private Action action;
    private bool isSuccessLoad = true;

#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    string adUnitId = "unexpected_platform";
#endif


    // Start is called before the first frame update
    void Start()
    {

        
        this.interstitial = CreateInterstitialAd();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Show(Action callback)
    {
        StartCoroutine(MonitorLoadingAd());
        return;

        IEnumerator MonitorLoadingAd()
        {
            while (!this.interstitial.IsLoaded() && isSuccessLoad)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (isSuccessLoad)
            {
                this.action = callback;
                this.interstitial.Show();
            }
            else
            {
                callback();
            }
        }

    }


    private InterstitialAd CreateInterstitialAd()
    {
        var _interstitial = new InterstitialAd(adUnitId);
        _interstitial.OnAdLoaded += HandleOnAdLoaded;
        _interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _interstitial.OnAdOpening += HandleOnAdOpened;
        _interstitial.OnAdClosed += HandleOnAdClosed;
        _interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        _interstitial.LoadAd(request);
        isSuccessLoad = true;
        return _interstitial;
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isSuccessLoad = false;
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.interstitial.Destroy();
        this.interstitial = CreateInterstitialAd();
        this.action();
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

}
