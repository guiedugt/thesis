using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public bool isTargetPlayStore = true;
    public bool isTestAd = true;

    const string playStoreID = "3589375";
    const string appStoreID = "3589374";

    const string interstitialAd = "video";
    const string rewardedVideoAd = "rewardedVideo";

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

    public void PlayInterstitialAd()
    {
        if (!Advertisement.IsReady(interstitialAd)) return;
        Advertisement.Show(interstitialAd);
    }

    public void PlayRewardedVideoAd()
    {
        if (!Advertisement.IsReady(rewardedVideoAd)) return;
        Advertisement.Show(rewardedVideoAd);
    }

    public void OnUnityAdsReady(string placementId) { }

    public void OnUnityAdsDidStart(string placementId)
    {
        // TODO: MUTE THE AUDIO
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // TODO: UNMUTE THE AUDIO
        switch (showResult)
        {
            case ShowResult.Finished:
                // TODO: REWARD
                if (placementId == rewardedVideoAd) Debug.Log("Reward the player");
                if (placementId == interstitialAd) Debug.Log("Finished Interstitial Ad");
                break;
            case ShowResult.Skipped:
                // TODO: LET  USER KNOW HE DIDN`T GET THE REWARD
                break;
            case ShowResult.Failed:
                // TODO: LET USER KNOW SOMETHING WENT WRONG
                break;
        }
    }

    public void OnUnityAdsDidError(string message) { }
}