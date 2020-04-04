using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] Transform[] defaultObstaclesLocations;

    WaitForSeconds wait;
    WaitForSeconds oneSec = new WaitForSeconds(1);

    void Start()
    {
        wait = new WaitForSeconds(LevelManager.Instance.delay);
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        SpawnDefaultObstacles();
    }

    void HandleGameStart()
    {
        StartCoroutine(SpawnCoroutine(LevelManager.Instance.hasInitialDelay));
    }

    void HandleGameOver()
    {
        StopAllCoroutines();
    }

    void HandleGameRestart()
    {
        SpawnDefaultObstacles();
    }

    void SpawnDefaultObstacles()
    {
        if (defaultObstaclesLocations != null && defaultObstaclesLocations.Length > 0)
        {
            foreach (Transform defaultObstacleTransform in defaultObstaclesLocations)
            {
                SpawnObstacle(defaultObstacleTransform.position, defaultObstacleTransform.rotation);
            };
        }
    }

    IEnumerator SpawnCoroutine(bool waitForDelay = true)
    {
        if (waitForDelay) { yield return wait; }
        SpawnObstacle(null, null);
        yield return SpawnCoroutine();
    }

    GameObject SpawnObstacle(Vector3? position, Quaternion? rotation)
    {
        Vector3 spawnPosition = position ?? transform.position;
        Quaternion spawnRotation = rotation ?? transform.rotation;

        GameObject obstacleGameObject = Instantiate(obstaclePrefab, spawnPosition, spawnRotation, MemoryManager.Instance.transform);
        Obstacle obstacle = obstacleGameObject.GetComponent<Obstacle>();
        obstacle.speed = Random.Range(LevelManager.Instance.minSpeed, LevelManager.Instance.maxSpeed);
        obstacle.zigZagSpeed = Random.Range(LevelManager.Instance.minZigZagSpeed, LevelManager.Instance.maxZigZagSpeed);
        obstacle.zigZagDistance = Random.Range(LevelManager.Instance.minZigZagDistance, LevelManager.Instance.maxZigZagDistance);
        return obstacleGameObject;
    }
}