using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIFeedbackSoundButton : MonoBehaviour
{
    [SerializeField] AudioClip feedbackSFX;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayFeedbackSound);
    }

    void PlayFeedbackSound()
    {
        AudioManager.Instance.Play(feedbackSFX);
    }
}