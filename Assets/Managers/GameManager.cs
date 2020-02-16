using UnityEngine;
 
public class GameManager : Singleton<GameManager>
{
    public static new Camera camera;

    void Awake()
    {
        camera = FindObjectOfType<Camera>();
    }
}