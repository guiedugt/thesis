using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
  public static float score = 0f;

  [SerializeField] Slider scoreSlider;
  [SerializeField] float multiplier = 50f;

  void Start()
  {
    GameManager.Instance.OnGameStart.AddListener(HandleGameStartAndRestart);
    GameManager.Instance.OnGameRestart.AddListener(HandleGameStartAndRestart);
  }

  void HandleGameStartAndRestart()
  {
    scoreSlider.value = 0f;
    scoreSlider.maxValue = GetLevelMaxScore(1);
  }

  public void AddScore(float amount)
  {
    score += amount;
    scoreSlider.value = score;

    if (score >= GetLevelMaxScore(LevelManager.level))
    {
      int currentLevel = LevelManager.level;
      scoreSlider.minValue = GetLevelMaxScore(currentLevel);
      scoreSlider.maxValue = GetLevelMaxScore(currentLevel + 1);
      LevelManager.Instance.LoadLevel(currentLevel + 1);
    }
  }

  float GetLevelMaxScore(int? level)
  {
    return (level ?? LevelManager.level) * multiplier;
  }
}