using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : Singleton<AdManager>, IUnityAdsListener
{
    public bool isTargetPlayStore = true;
    public bool isTestAd = true;

    const string playStoreID = "3589375";
    const string appStoreID = "3589374";

    const string interstitialAd = "video";
    const string rewardedVideoAd = "rewardedVideo";

    Action<ShowResult> callback;

    void Start()
    {
        Advertisement.AddListener(this);
        InitializeAd();
    }

    void InitializeAd()
    {
        if (isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestAd);
        }
        else
        {
            Advertisement.Initialize(appStoreID, isTestAd);
        }
    }

    public void PlayInterstitialAd(Action<ShowResult> callback)
    {
        if (!Advertisement.IsReady(interstitialAd)) return;
        this.callback = callback;
        Advertisement.Show(interstitialAd);
    }

    public void PlayRewardedVideoAd(Action<ShowResult> callback)
    {
        if (!Advertisement.IsReady(rewardedVideoAd)) return;
        this.callback = callback;
        Advertisement.Show(rewardedVideoAd);
    }

    public void OnUnityAdsReady(string placementId) { }

    public void OnUnityAdsDidStart(string placementId)
    {
        AudioListener.pause = true;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult result)
    {
        AudioListener.pause = false;
        callback(result);
    }

    public void OnUnityAdsDidError(string message) { }
}