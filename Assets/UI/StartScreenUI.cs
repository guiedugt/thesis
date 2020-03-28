using UnityEngine;

public class StartScreenUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
    }

    void HandleGameStart()
    {
        gameObject.SetActive(false);
    }
}