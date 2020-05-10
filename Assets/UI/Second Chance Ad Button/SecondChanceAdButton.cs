using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class SecondChanceAdButton : MonoBehaviour
{
    [SerializeField] Alert alert;
    [SerializeField] TimeBar secondChanceTimeBar;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        secondChanceTimeBar.Pause();
        AdManager.Instance.PlayRewardedVideoAd(AdCallback);
    }

    void AdCallback(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Handle second chance");
        }
        else
        {
            alert.Show("Sorry, something went wrong");
            secondChanceTimeBar.Resume();
        }
    }
}