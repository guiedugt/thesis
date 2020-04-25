using UnityEngine.SceneManagement;

public class SceneManager : UnityEngine.SceneManagement.SceneManager
{
    public static void ReloadScene()
    {
        Scene currentScene = GetActiveScene();
        LoadScene(currentScene.name);
    }

    public static void LoadGameScene()
    {
        LoadScene("Game");
    }

    public static void LoadGarageScene()
    {
        LoadScene("Garage");
    }
}