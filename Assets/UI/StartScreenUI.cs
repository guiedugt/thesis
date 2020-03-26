using UnityEngine;

public class StartScreenUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameStart()
    {
        gameObject.SetActive(false);
    }

    void HandleGameOver()
    {
        gameObject.SetActive(true);
    }
}