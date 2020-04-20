using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
  public static float score = 0f;

  [SerializeField] Slider scoreSlider;
  [SerializeField] float scorePerSecond = 5f;
  [SerializeField] float multiplier = 20f;

  void Start()
  {
    GameManager.Instance.OnGameStart.AddListener(HandleGameStartAndRestart);
    GameManager.Instance.OnGameRestart.AddListener(HandleGameStartAndRestart);
  }

  void Update()
  {
    if (GameManager.isGameRunning) { AddScore(scorePerSecond * Time.deltaTime); }
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
      scoreSlider.minValue = scoreSlider.maxValue;
      scoreSlider.maxValue = GetLevelMaxScore(currentLevel + 1);
      LevelManager.Instance.LevelUp();
    }
  }

  float GetLevelMaxScore(int? level)
  {
    if (level <= 0) { return 0; }
    return GetLevelMaxScore(level - 1) + (level ?? LevelManager.level) * multiplier;
  }
}