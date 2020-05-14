using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;

public class AdManager : Singleton<AdManager>, IUnityAdsListener
{
    public bool isTargetPlayStore = true;
    public bool isTestAd = true;

    const string playStoreID = "3589375";
    const string appStoreID = "3589374";

    public string rewardedVideoAd = "rewardedVideo";

    public enum AdType { SecondChance, SuperBomb }
    static AdType currentAdType;
    static Action<ShowResult> callback;
    bool previousAudioListenerPauseState;

    void Start()
    {
        Advertisement.AddListener(this);
        previousAudioListenerPauseState = AudioListener.pause;
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

    public void PlayRewardedVideoAd(Action<ShowResult> callback, AdType adType)
    {
        if (!Advertisement.IsReady(rewardedVideoAd)) return;
        AdManager.callback = callback;
        AdManager.currentAdType = adType;
        Advertisement.Show(rewardedVideoAd);

    }

    public void OnUnityAdsReady(string placementId) { }

    public void OnUnityAdsDidStart(string placementId)
    {
        previousAudioListenerPauseState = AudioListener.pause;
        AudioListener.pause = true;
        AnalyticsEvent.AdStart(true, null, rewardedVideoAd, new Dictionary<string, object>
        {
            { "ad_type", Enum.GetName(typeof(AdType), AdManager.currentAdType) }
        });
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult result)
    {
        AudioListener.pause = previousAudioListenerPauseState;

        if (result == ShowResult.Skipped)
        {
            AnalyticsEvent.AdSkip(true, null, rewardedVideoAd, new Dictionary<string, object>
            {
                { "ad_type", Enum.GetName(typeof(AdType), AdManager.currentAdType) }
            });
        }

        if (result == ShowResult.Finished)
        {
            AnalyticsEvent.AdComplete(true, null, rewardedVideoAd, new Dictionary<string, object>
            {
                { "ad_type", Enum.GetName(typeof(AdType), AdManager.currentAdType) }
            });
        }

        AdManager.callback(result);
    }

    public void OnUnityAdsDidError(string message)
    {
        AudioListener.pause = previousAudioListenerPauseState;
    }
}