using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(UIFade))]
public class SecondChanceModal : TimeBar
{
    [SerializeField] TimeBar secondChanceTimeBar;
    [SerializeField] UIFade summaryFade;

    UIFade fade;

    void Start()
    {
        fade = GetComponent<UIFade>();
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameOver()
    {
        if (SecondChanceManager.isSecondChance)
        {
            summaryFade.Show();
        }
        else
        {
            secondChanceTimeBar.Tick(HandleTimeUp);
            AnalyticsEvent.AdOffer(true, null, AdManager.Instance.rewardedVideoAd, new Dictionary<string, object>
            {
                { "ad_type", "second_chance" }
            });
        }
    }

    void HandleTimeUp()
    {
        secondChanceTimeBar.Stop();
        fade.Hide();
        summaryFade.Show();
    }

    public void Hide()
    {
        secondChanceTimeBar.Stop();
        fade.Hide();
    }
}