using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField][Range(0f, 5f)] float fadeDuration = 1f;

    public static new Camera camera;
    public static GameObject player;
    public static bool isGameRunning = false;
    public static bool isGameOver = false;
    public static float tFade = 0f;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;

    Car car;

    void Awake()
    {
        camera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        car = player.GetComponent<Car>();
    }

    public void StartGame()
    {
        isGameRunning = true;
        isGameOver = false;
        StartCoroutine(TFadeCoroutine());
        Instance.OnGameStart.Invoke();
    }

    public void GameOver()
    {
        if (isGameOver) { return; }
        isGameRunning = false;
        isGameOver = true;
        camera.GetComponent<MainCamera>().Shake();
        InputManager.Instance.enabled = false;
        car.enabled = false;
        StartCoroutine(TFadeCoroutine());
        Instance.OnGameOver.Invoke();
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