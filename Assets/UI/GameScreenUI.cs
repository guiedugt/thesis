using UnityEngine;

public class GameScreenUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        gameObject.SetActive(false);
    }

    void HandleGameOver()
    {
        gameObject.SetActive(false);
    }

    void HandleGameStart()
    {
        gameObject.SetActive(true);
    }

    void HandleGameRestart()
    {
        gameObject.SetActive(false);
    }
}