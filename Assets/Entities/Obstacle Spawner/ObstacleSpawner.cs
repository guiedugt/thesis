using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;

    WaitForSeconds wait;
    WaitForSeconds oneSec = new WaitForSeconds(1);

    void Start()
    {
        wait = new WaitForSeconds(ObstacleManager.Instance.delay);
        GameManager.Instance.OnGameStart.AddListener(() => StartCoroutine(SpawnCoroutine(ObstacleManager.Instance.hasInitialDelay)));
        GameManager.Instance.OnGameOver.AddListener(() => StopAllCoroutines());
    }

    IEnumerator SpawnCoroutine(bool waitForDelay = true)
    {
        if (waitForDelay) { yield return wait; }
        GameObject obstacleGameObject = Instantiate(obstaclePrefab, transform.position, transform.rotation, MemoryManager.Instance.transform);
        Obstacle obstacle = obstacleGameObject.GetComponent<Obstacle>();
        obstacle.speed = Random.Range(ObstacleManager.Instance.minSpeed, ObstacleManager.Instance.maxSpeed);
        obstacle.zigZagSpeed = Random.Range(ObstacleManager.Instance.minZigZagSpeed, ObstacleManager.Instance.maxZigZagSpeed);
        obstacle.zigZagDistance = Random.Range(ObstacleManager.Instance.minZigZagDistance, ObstacleManager.Instance.maxZigZagDistance);
        yield return SpawnCoroutine();
    }
}