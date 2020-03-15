using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public static new Camera camera;
    public static GameObject player;
    public static bool isGameOver = false;
    public UnityEvent OnGameOver;

    void Awake()
    {
        camera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameOver()
    {
        if (isGameOver) { return; }
        print("Game Over");
        isGameOver = true;
        camera.GetComponent<MainCamera>().Shake();
        InputManager.Instance.enabled = false;
        player.GetComponent<Car>().enabled = false;
    }

}