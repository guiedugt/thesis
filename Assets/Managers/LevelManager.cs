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
    public float minSpeed = 5f;
    public float maxSpeed = 8f;
    public float minZigZagSpeed = 0f;
    public float maxZigZagSpeed = 1.5f;
    public float minZigZagDistance = 0f;
    public float maxZigZagDistance = 6f;


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
        this.minSpeed = multiplier / 4 + minSpeed;
        this.maxSpeed = multiplier / 4 + minSpeed;
        this.minZigZagSpeed = multiplier / 5;
        this.maxZigZagSpeed = multiplier / 4;
        this.minZigZagDistance = multiplier / 6;
        this.maxZigZagDistance = multiplier / 5;

        levelText.text = "Level " + nextLevel;
        level = nextLevel;
    }
}