﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField][Range(0f, 5f)] float fadeDuration = 1f;

    public static new MainCamera camera;
    public static Car car;
    public static bool isGameRunning = false;
    public static bool isGameOver = false;
    public static float tFade = 0f;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public UnityEvent OnGameRestart;

    List<Reloadable> reloadables;

    void Awake()
    {
        camera = FindObjectOfType<MainCamera>();
        car = FindObjectOfType<Car>();
    }

    public void StartGame()
    {
        isGameRunning = true;
        isGameOver = false;
        car.enabled = true;

        reloadables = new List<Reloadable>(FindObjectsOfType<Reloadable>());
        reloadables.ForEach(t => t.Reload());
        StartCoroutine(TFadeCoroutine());
        OnGameStart.Invoke();
    }

    public void GameOver()
    {
        if (isGameOver) { return; }
        isGameRunning = false;
        isGameOver = true;
        car.enabled = false;

        camera.Shake();
        StartCoroutine(TFadeCoroutine());
        OnGameOver.Invoke();
    }

    public void RestartGame()
    {
        isGameRunning = false;
        isGameOver = false;
        car.enabled = true;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        MemoryManager.Instance.Clear();
        OnGameRestart.Invoke();
    }

    IEnumerator TFadeCoroutine()
    {
        tFade = 0f;
        float startTime = Time.time;
        while (tFade < 1f)
        {
            tFade = Mathf.Min((Time.time - startTime) / fadeDuration, 1f);
            yield return null;
        }
    }
}