using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static new MainCamera camera;
    public static Car car;
    public static bool isGameRunning = false;
    public static bool isGameOver = false;
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
        OnGameStart.Invoke();
    }

    public void GameOver()
    {
        if (isGameOver) { return; }
        isGameRunning = false;
        isGameOver = true;
        car.enabled = false;

        camera.Shake();
        OnGameOver.Invoke();
    }

    public void RestartGame()
    {
        isGameRunning = false;
        isGameOver = false;
        car.enabled = true;

        OnGameRestart.Invoke();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        MemoryManager.Instance.Clear();
    }
}