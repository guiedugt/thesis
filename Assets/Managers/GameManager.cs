using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static new MainCamera camera;
    public static Car car;
    public static bool isGameRunning = false;
    public static bool isGameOver = false;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public UnityEvent OnGameRestart;

    static float timeSinceSessionStart = 0f;
    float timeSinceGameStart = 0f;
    List<Reloadable> reloadables;

    void Awake()
    {
        camera = FindObjectOfType<MainCamera>();
        car = FindObjectOfType<Car>();
    }

    void Start()
    {
        if (SceneManager.Instance.IsGameScene())
        {
            AnalyticsEvent.AdOffer(true, null, AdManager.Instance.rewardedVideoAd, new Dictionary<string, object>
            {
                { "ad_type", "super_bomb" }
            });
        }
    }

    void Update()
    {
        timeSinceSessionStart += Time.deltaTime;
        if (isGameRunning) timeSinceGameStart += Time.deltaTime;
    }

    public void StartGame()
    {
        isGameRunning = true;
        isGameOver = false;
        car.enabled = true;
        timeSinceGameStart = 0f;

        AnalyticsEvent.GameStart();
        OnGameStart.Invoke();
    }

    public void GameOver()
    {
        if (isGameOver) { return; }
        isGameRunning = false;
        isGameOver = true;
        car.enabled = false;

        camera.Shake();
        AnalyticsEvent.GameOver("game_over", new Dictionary<string, object>
        {
            { "game_duration_in_seconds", timeSinceGameStart },
            { "level_reached", LevelManager.level }
        });

        AnalyticsEvent.LevelFail(LevelManager.level);

        OnGameOver.Invoke();
    }

    public void RestartGame(bool isSecondChance = false)
    {
        if (isSecondChance) {
            isGameRunning = false;
            isGameOver = false;
            car.enabled = true;

            reloadables = new List<Reloadable>(FindObjectsOfType<Reloadable>());
            reloadables.ForEach(t => t.Reload());

            MemoryManager.Instance.Clear();
            OnGameRestart.Invoke();
            isGameRunning = true;
        } else {
            isGameRunning = false;
            isGameOver = false;
            car.enabled = true;

            MemoryManager.Instance.Clear();
            OnGameRestart.Invoke();
            SceneManager.Instance.ReloadScene();
        }
    }

    public void HandleGarageBack()
    {
        isGameRunning = false;
        isGameOver = false;
    }

    void OnApplicationQuit()
    {
        AnalyticsEvent.Custom("game_quit", new Dictionary<string, object>
        {
            { "session_duration_in_seconds", timeSinceSessionStart }
        });
    }
}