using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
  void Start()
  {
    GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
    gameObject.SetActive(false);
  }

  void HandleGameOver()
  {
    gameObject.SetActive(true);
  }

  void HandleGameRestart()
  {
    gameObject.SetActive(false);
  }
}