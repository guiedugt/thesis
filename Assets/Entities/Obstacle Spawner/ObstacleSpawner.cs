﻿using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawner Properties")]
    [SerializeField] GameObject prefab;
    [SerializeField] bool hasInitialDelay = false;
    [SerializeField] float delay = 3f;

    [Header("Obstacle Properties")]
    [SerializeField] float minSpeed = 8f;
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float minZigZagSpeed = 1f;
    [SerializeField] float maxZigZagSpeed = 5f;
    [SerializeField] float minZigZagDistance = 0f;
    [SerializeField] float maxZigZagDistance = 10f;

    WaitForSeconds wait;

    void Start()
    {
        wait = new WaitForSeconds(delay);
        StartCoroutine(SpawnCoroutine(hasInitialDelay));
    }

    IEnumerator SpawnCoroutine(bool waitForDelay = true)
    {
        if (waitForDelay) { yield return wait; }
        GameObject obstacleGameObject = Instantiate(prefab, transform.position, transform.rotation, MemoryManager.Instance.transform);
        Obstacle obstacle = obstacleGameObject.GetComponent<Obstacle>();
        obstacle.speed = Random.Range(minSpeed, maxSpeed);
        obstacle.zigZagSpeed = Random.Range(minZigZagSpeed, maxZigZagSpeed);
        obstacle.zigZagDistance = Random.Range(minZigZagDistance, maxZigZagDistance);
        yield return SpawnCoroutine();
    }
}