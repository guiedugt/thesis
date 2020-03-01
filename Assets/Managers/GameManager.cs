using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static new Camera camera;
    public static GameObject player;

    void Awake()
    {
        camera = FindObjectOfType<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
}