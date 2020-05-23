using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Reloadable))]
public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject[] advancedObstaclePrefabs;
    [SerializeField] Transform[] defaultObstaclesLocations;

    WaitForSeconds wait;
    WaitForSeconds oneSec = new WaitForSeconds(1);
    System.Random random = new System.Random();
    Reloadable reloadable;
    Stack<GameObject> advancedObstaclePrefabsStack;

    void Start()
    {
        wait = new WaitForSeconds(LevelManager.Instance.delay);
        reloadable = GetComponent<Reloadable>();
        reloadable.OnReload.AddListener(HandleReload);
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        RefillStack();
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
        
        GameObject prefabToInstantiate = GetObstaclePrefab();
        GameObject obstacleGameObject = Instantiate(prefabToInstantiate, spawnPosition, spawnRotation, MemoryManager.Instance.transform);
        Obstacle obstacle = obstacleGameObject.GetComponent<Obstacle>();
        obstacle.speed = Random.Range(LevelManager.Instance.minSpeed, LevelManager.Instance.maxSpeed);
        obstacle.zigZagSpeed = Random.Range(LevelManager.Instance.minZigZagSpeed, LevelManager.Instance.maxZigZagSpeed);
        obstacle.zigZagDistance = Random.Range(LevelManager.Instance.minZigZagDistance, LevelManager.Instance.maxZigZagDistance);
        return obstacleGameObject;
    }

    GameObject GetObstaclePrefab()
    {
        if (LevelManager.level <= 3 || advancedObstaclePrefabs.Length <= 0) return obstaclePrefab;
        if (advancedObstaclePrefabsStack.Count <= 0) RefillStack();
        return advancedObstaclePrefabsStack.Pop();
    }

    void RefillStack()
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        foreach (var value in advancedObstaclePrefabs.OrderBy(x => random.Next())) stack.Push(value);
        advancedObstaclePrefabsStack = stack;
    }

    void HandleReload()
    {
        StopAllCoroutines();
        SpawnDefaultObstacles();
        StartCoroutine(SpawnCoroutine(LevelManager.Instance.hasInitialDelay));
    }
}