using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(UIFade))]
public class UIHider : MonoBehaviour
{
    [SerializeField] bool showInStartScreen = false;
    [SerializeField] bool showInGameScreen = false;
    [SerializeField] bool showInEndScreen = false;
    [SerializeField] bool showInSecondChanceScreen = false;

    CanvasGroup canvasGroup;
    UIFade fade;

    void Start()
    {
        fade = GetComponent<UIFade>();
        canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        SecondChanceManager.Instance.OnTrigger.AddListener(HandleSecondChance);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        if (showInStartScreen) fade.Show();
    }

    void HandleGameStart()
    {
        if (showInStartScreen) fade.Hide();
        if (showInGameScreen) fade.Show();
    }

    void HandleGameOver()
    {
        if (showInGameScreen) fade.Hide();
        if (showInEndScreen) fade.Show();
    }

    void HandleGameRestart()
    {
        if (showInEndScreen) fade.Hide();
        if (showInStartScreen) fade.Show();
    }

    void HandleSecondChance()
    {
        if (showInSecondChanceScreen) fade.Show(instantly: true);
        else fade.Hide(instantly: true);
    }
}