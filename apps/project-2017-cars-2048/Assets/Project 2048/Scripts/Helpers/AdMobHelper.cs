using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using GoogleMobileAds.Api;
using UnityEngine;

public interface IAdMobHelper
{
    void Start();

    void LoadLoadInterstitialAd();
    void ShowInterstitialAd();

    void CreateBannerView();
    void LoadAd();
}

// IMPL *************************************************************

// https://developers.google.com/admob/unity/start

class AdMobHelper : IAdMobHelper
{
    BannerView _bannerView;
    InterstitialAd _interstitialAd;

#if UNITY_ANDROID
    string _bannerView_adUnitId = "ca-app-pub-2137428709527727/5934974036";
    string _interstitialAd_adUnitId = "ca-app-pub-2137428709527727/8313858812";
#else
    string _bannerView_adUnitId = "unused";
    string _interstitialAd_adUnitId = "unused";
#endif

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
//        if (_bannerView != null)
//        {
//            DestroyAd();
//        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_bannerView_adUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }


    public void LoadLoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_interstitialAd_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    internal void SetOnInterstitialAdClosed(Action handleOnAdClosed)
    {
        if (_interstitialAd != null)
            _interstitialAd.OnAdFullScreenContentClosed += handleOnAdClosed;
        else
            Debug.LogError("Interstitial ad is not ready yet.");
    }
}



