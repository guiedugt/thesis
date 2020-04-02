using System.Collections.Generic;
using UnityEngine;

struct ObstacleLevel
{
    public float delay;
    public float minSpeed;
    public float maxSpeed;
    public float minZigZagSpeed;
    public float maxZigZagSpeed;
    public float minZigZagDistance;
    public float maxZigZagDistance;

    public ObstacleLevel(
        float delay,
        float minSpeed,
        float maxSpeed,
        float minZigZagSpeed,
        float maxZigZagSpeed,
        float minZigZagDistance,
        float maxZigZagDistance
    )
    {
        this.delay = delay;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.minZigZagSpeed = minZigZagSpeed;
        this.maxZigZagSpeed = maxZigZagSpeed;
        this.minZigZagDistance = minZigZagDistance;
        this.maxZigZagDistance = maxZigZagDistance;
    }
}

public class ObstacleManager : Singleton<ObstacleManager>
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

    Dictionary<int, ObstacleLevel> levels;

    void Awake()
    {
        levels = new Dictionary<int, ObstacleLevel>();
        levels.Add(1, new ObstacleLevel(
            delay: 4f,
            minSpeed: 6f,
            maxSpeed: 8f,
            minZigZagSpeed: 0f,
            maxZigZagSpeed: 2f,
            minZigZagDistance: 0f,
            maxZigZagDistance: 3f
        ));

        levels.Add(2, new ObstacleLevel(
            delay: 3.5f,
            minSpeed: 7f,
            maxSpeed: 10f,
            minZigZagSpeed: 1f,
            maxZigZagSpeed: 5f,
            minZigZagDistance: 2f,
            maxZigZagDistance: 4f
        ));


        levels.Add(3, new ObstacleLevel(
            delay: 3f,
            minSpeed: 8f,
            maxSpeed: 11f,
            minZigZagSpeed: 3f,
            maxZigZagSpeed: 7f,
            minZigZagDistance: 3f,
            maxZigZagDistance: 5f
        ));

        LoadLevel(1);
    }

    public void LoadLevel(int level)
    {
        this.minSpeed = levels[level].minSpeed;
        this.maxSpeed = levels[level].maxSpeed;
        this.minZigZagSpeed = levels[level].minZigZagSpeed;
        this.maxZigZagSpeed = levels[level].maxZigZagSpeed;
        this.minZigZagDistance = levels[level].minZigZagDistance;
        this.maxZigZagDistance = levels[level].maxZigZagDistance;
    }
}