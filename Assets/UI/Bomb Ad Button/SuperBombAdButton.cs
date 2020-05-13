using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(UIFade))]
[RequireComponent(typeof(Button))]
public class SuperBombAdButton : MonoBehaviour
{
    [SerializeField] SuperBombButton superBombButton;
    [SerializeField] Alert alert;

    UIFade fade;
    Button button;

    void Start()
    {
        fade = GetComponent<UIFade>();
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);

        if (PlayerPrefsManager.IsSuperBombAvailable()) fade.Hide(instantly: true);
    }

    void HandleClick()
    {
        AdManager.Instance.PlayRewardedVideoAd(AdCallback, AdManager.AdType.SuperBomb);
    }

    void AdCallback(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            fade.Hide();
            superBombButton.Show();
            alert.Show("Awesome! Super Bomb is now available!");
        }
        else
        {
            superBombButton.Hide();
            alert.Show("Oh no... something went wrong. Maybe try again?");
        }
    }
}