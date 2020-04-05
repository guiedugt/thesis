using UnityEngine;

public class UIHider : MonoBehaviour
{
    [Header("Screens to show UI")]
    [SerializeField] bool showInStartScreen = false;
    [SerializeField] bool showInGameScreen = false;
    [SerializeField] bool showInEndScreen = false;

    void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        gameObject.SetActive(showInStartScreen);
    }

    void HandleGameStart()
    {
        gameObject.SetActive(showInGameScreen);
    }

    void HandleGameOver()
    {
        gameObject.SetActive(showInEndScreen);
    }

    void HandleGameRestart()
    {
        gameObject.SetActive(showInStartScreen);
    }
}