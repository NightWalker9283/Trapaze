using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public static AdsManager adsManager;
    public static RewardAdManager rewardAdManager;
    public static InterstitialAdManager interstitialAdManager;
    BannerAdManager bannerAdManager;

    // Start is called before the first frame update
    void Awake()
    {
        adsManager = this;
        if (SceneManager.GetActiveScene().name == "Title")
        {
            bannerAdManager = gameObject.AddComponent<BannerAdManager>();
        }

        rewardAdManager = gameObject.AddComponent<RewardAdManager>();
        interstitialAdManager = gameObject.AddComponent<InterstitialAdManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bannerAdManager != null && bannerAdManager.bannerView != null && SceneManager.GetActiveScene().name!="Title")
        {
            bannerAdManager.bannerView.Destroy();
            bannerAdManager.bannerView = null;
            bannerAdManager = null;

        }


    }

}
