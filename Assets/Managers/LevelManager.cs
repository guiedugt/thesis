using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
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

    public void LoadLevel(int level)
    {
        float multiplier = (float) level;
        this.minSpeed = multiplier + 4f;
        this.maxSpeed = multiplier + 4f;
        this.minZigZagSpeed = multiplier / 3;
        this.maxZigZagSpeed = multiplier / 2;
        this.minZigZagDistance = multiplier / 3;
        this.maxZigZagDistance = multiplier / 2;
    }
}