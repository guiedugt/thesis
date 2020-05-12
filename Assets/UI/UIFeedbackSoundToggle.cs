using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIFeedbackSoundToggle : MonoBehaviour
{
    [SerializeField] AudioClip feedbackSFX;

    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(PlayFeedbackSound);
    }

    void PlayFeedbackSound(bool isActive)
    {
        AudioManager.Instance.Play(feedbackSFX);
    }
}