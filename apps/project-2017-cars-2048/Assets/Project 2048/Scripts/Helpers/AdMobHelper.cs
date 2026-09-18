using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleMobileAds.Api;
using UnityEngine;

public interface IAdMobHelper
{
    void Initialize(string JSONPath);

    BannerView RequestBanner(string AdName);
    InterstitialAd RequestInterstitial(string AdName);

    AdRequest BuildRequest();
}

[Serializable]
public class AdMobData
{
    public bool TagForChildDirectedTreatment;
    public string Excl_cat;
    public string[] Keywords;
    public AdData[] Ads;
}

[Serializable]
public class AdData
{
    public string Name;
    public string ID;
}

// IMPL *************************************************************

// https://developers.google.com/admob/unity/start

class AdMobHelper : IAdMobHelper
{
    AdMobData data;

    public void Initialize(string JSONPath)
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        data = Load(JSONPath);
    }

    public AdRequest BuildRequest()
    {
        AdRequest.Builder builder = new AdRequest.Builder();
        builder.TagForChildDirectedTreatment(data.TagForChildDirectedTreatment);
        builder.AddExtra("excl_cat", data.Excl_cat);
        foreach (string keyword in data.Keywords)
            builder.AddKeyword(keyword);
        AdRequest request = builder.Build();
        return request;
    }

    public BannerView RequestBanner(string AdName)
    {
        BannerView bannerView = new BannerView(
            FindAdID(AdName),
            AdSize.Banner,
            AdPosition.Bottom);
        bannerView.LoadAd(BuildRequest());
        return bannerView;
    }

    public InterstitialAd RequestInterstitial(string AdName)
    {
        InterstitialAd interstitial = new InterstitialAd(
            FindAdID(AdName));
        interstitial.LoadAd(BuildRequest());
        return interstitial;
    }

    // ****************************************************

    private AdMobData Load(string JSONPath)
    {
        TextAsset txt = Resources.Load<TextAsset>(JSONPath);
        AdMobData data = JsonUtility.FromJson<AdMobData>(txt.text);
        return data;
    }

    private string FindAdID(string AdName)
    {
#if UNITY_EDITOR
        return "unused";
#elif UNITY_ANDROID
        return FindAd(AdName).ID;
#elif UNITY_IPHONE
        return"INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
        return "unexpected_platform";
#endif
    }

    private AdData FindAd(string AdName)
    {
        foreach (AdData ad in data.Ads)
            if (AdName.ToLower().Equals(ad.Name.ToLower()))
                return ad;
        return new AdData()
        {
            Name = AdName,
            ID = "unused"
        };
    }

}



