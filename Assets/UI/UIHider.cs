using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(UIFade))]
public class UIHider : MonoBehaviour
{
    [SerializeField] bool showInStartScreen = false;
    [SerializeField] bool showInGameScreen = false;
    [SerializeField] bool showInEndScreen = false;

    CanvasGroup canvasGroup;
    UIFade fade;

    void Start()
    {
        fade = GetComponent<UIFade>();
        canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        if (showInStartScreen) Show();
    }

    void Show() => fade.Show(() => canvasGroup.interactable = true);
    void Hide() => fade.Hide(() => canvasGroup.interactable = false);

    void HandleGameStart()
    {
        if (showInStartScreen) Hide();
        if (showInGameScreen) Show();
    }

    void HandleGameOver()
    {
        if (showInGameScreen)  Hide();
        if (showInEndScreen) Show();
    }

    void HandleGameRestart()
    {
        if (showInEndScreen)  Hide();
        if (showInStartScreen) Show();
    }
}