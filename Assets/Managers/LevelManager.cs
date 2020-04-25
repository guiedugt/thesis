using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Singleton<LevelManager>
{
    public static int level = 1;

    [Header("Level Properties")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] float levelUpMultiplier = 10f;
    public UnityEvent OnLevelUp;

    [Header("Spawner Properties")]
    public bool hasInitialDelay = false;
    public float delay = 3f;

    [Header("Obstacle Properties")]
    public float minSpeed = 8f;
    public float maxSpeed = 12f;
    public float minZigZagSpeed = 1f;
    public float maxZigZagSpeed = 5f;
    public float minZigZagDistance = 0f;
    public float maxZigZagDistance = 10f;


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
        float multiplier = (float) nextLevel;
        this.minSpeed = multiplier / 3 + 6f;
        this.maxSpeed = multiplier / 3 + 6f;
        this.minZigZagSpeed = multiplier / 4;
        this.maxZigZagSpeed = multiplier / 3;
        this.minZigZagDistance = multiplier / 5;
        this.maxZigZagDistance = multiplier / 4;

        levelText.text = "Level " + nextLevel;
        level = nextLevel;
    }
}