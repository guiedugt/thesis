using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PauseButton : MonoBehaviour
{
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    
    Toggle toggle;
    Image image;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(Toggle);
        image = toggle.targetGraphic.GetComponent<Image>();

        bool isPaused = Time.timeScale <= Mathf.Epsilon;
        Sprite sprite = isPaused ? playSprite : pauseSprite;
        toggle.isOn = isPaused;
        image.sprite = sprite;
    }

    public void Toggle(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        image.sprite = isPaused ? playSprite : pauseSprite;
    }
}