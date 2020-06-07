using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Singleton<LevelManager>
{
    public static int level = 1;

    [Header("Level Properties")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] float levelUpMultiplier = 3f;
    public UnityEvent OnLevelUp;

    [Header("Spawner Properties")]
    public bool hasInitialDelay = false;
    public float delay = 3f;

    [Header("Obstacle Properties")]
    public float minSpeed;
    public float maxSpeed;
    public float minZigZagSpeed;
    public float maxZigZagSpeed;
    public float minZigZagDistance;
    public float maxZigZagDistance;


    void Awake()
    {
        LoadLevel(1);
    }

    public void LevelUp()
    {
        int nextLevel = LevelManager.level + 1;
        LoadLevel(nextLevel);
        ScoreItem scoreItem = new ScoreItem(1f, nextLevel * levelUpMultiplier);
        ScoreManager.Instance.AddScore(ScoreType.Level, scoreItem);
        OnLevelUp.Invoke();
    }

    void LoadLevel(int nextLevel)
    {
        float multiplier = GetDifficultyProgressionCurveY(nextLevel);

        minSpeed = GetDifficultyProgressionCurveY(nextLevel, 10f, 15f);
        maxSpeed = GetDifficultyProgressionCurveY(nextLevel, 12f, 18f);
        minZigZagSpeed = GetDifficultyProgressionCurveY(nextLevel, 0f, 0.5f, 0.5f);
        maxZigZagSpeed = GetDifficultyProgressionCurveY(nextLevel, 0f, 2f, 0.5f);
        minZigZagDistance = GetDifficultyProgressionCurveY(nextLevel, 0f, 1f, 0.5f);
        maxZigZagDistance = GetDifficultyProgressionCurveY(nextLevel, 0f, 2f, 0.5f);

        levelText.text = "Level " + nextLevel;
        level = nextLevel;
    }

  // Curve Preview https://www.desmos.com/calculator/kokoyb75ge
  float GetDifficultyProgressionCurveY(float x, float min = 0.5f, float max = 8f, float intensity = 1f, float offset = 100f)
  {
    float rangeFactor = max - min;
    float numerator = rangeFactor * x;
    float growthFactor = intensity * x / rangeFactor;
    float denominator = x + Mathf.Pow(rangeFactor + offset, 1 - growthFactor);

    return min + (numerator / denominator);
  }
}