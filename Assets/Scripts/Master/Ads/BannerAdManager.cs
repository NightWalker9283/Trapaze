//#define TEST_AD
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdManager : MonoBehaviour
{
	public BannerView bannerView;
	private bool isSuccessLoad = true;
//バナー
#if TEST_AD //テスト

#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/6300978111"; //Android
#elif UNITY_IPHONE
    string adUnitId = "ca-app-pub-3940256099942544/2934735716"; //iOS
#else
	string adUnitId = "unexpected_platform";
#endif

#else   //本番
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-1610123728558925/7230492645"; //Android
#elif UNITY_IPHONE
    string adUnitId = "ca-app-pub-1610123728558925/5338826471"; //iOS
#else
	string adUnitId = "unexpected_platform";
#endif
#endif

    // Start is called before the first frame update
    void Start()
	{


		this.bannerView = CreateBannerView();

	}

	// Update is called once per frame
	void Update()
	{
        
	}

	

	private BannerView CreateBannerView()
	{
		var _bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
        _bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        _bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        _bannerView.OnAdOpening += this.HandleOnAdOpened;
        _bannerView.OnAdClosed += this.HandleOnAdClosed;
        _bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
        AdRequest request = new AdRequest.Builder().Build();
        _bannerView.LoadAd(request);
        return _bannerView;
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
		
		MonoBehaviour.print("HandleAdClosed event received");
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

}
