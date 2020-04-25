using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreType {
  Brick,
  Level,
  Time
}

public class ScoreItem {
  public float Amount { get; set; }
  public float Score { get; set; }

  public ScoreItem(float amount, float score) => (Amount, Score) = (amount, score);

  public static ScoreItem operator + (ScoreItem a, ScoreItem b) {
    ScoreItem scoreItem = new ScoreItem(a.Amount, a.Score);
    scoreItem.Amount += b.Amount;
    scoreItem.Score += b.Score;
    return scoreItem;
  }
}

public class ScoreManager : Singleton<ScoreManager>
{
  public float TotalScore {
    get {
      return PlayerPrefsManager.GetTotalScore();
    }
  }

  public float Score {
    get {
      float sum = 0;
      foreach (KeyValuePair<ScoreType, ScoreItem> scoreType in scoresByType) {
        sum += scoreType.Value.Score;
      }
      return sum;
    }
  }
  public Dictionary<ScoreType, ScoreItem> scoresByType;
  [SerializeField] Slider scoreSlider;
  [SerializeField] float scorePerSecond = 5f;
  [SerializeField] float multiplier = 20f;

  void Start()
  {
    GameManager.Instance.OnGameStart.AddListener(HandleGameStartAndRestart);
    GameManager.Instance.OnGameRestart.AddListener(HandleGameStartAndRestart);
    GameManager.instance.OnGameOver.AddListener(HandleGameOver);
    InitializeScoreByType();
  }

  void Update()
  {
    ScoreItem scoreItem = new ScoreItem(Time.deltaTime, scorePerSecond * Time.deltaTime);
    if (GameManager.isGameRunning) { AddScore(ScoreType.Time, scoreItem); }
  }

  void HandleGameStartAndRestart()
  {
    scoreSlider.value = 0f;
    scoreSlider.maxValue = GetLevelMaxScore(1);
    InitializeScoreByType();
  }

  void HandleGameOver()
  {
    PlayerPrefsManager.AddToTotalScore(Score);
  }

  public void AddScore(ScoreType type, ScoreItem item)
  {
    scoresByType[type] += item;

    float newScore = Score;
    scoreSlider.value = newScore;

    int currentLevel = LevelManager.level;
    if (newScore >= GetLevelMaxScore(currentLevel))
    {
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

  void InitializeScoreByType()
  {
    scoresByType = new Dictionary<ScoreType, ScoreItem>() {
      { ScoreType.Brick, new ScoreItem(0, 0f) },
      { ScoreType.Level, new ScoreItem(0, 0f) },
      { ScoreType.Time, new ScoreItem(0, 0f) }
    };
  }
}