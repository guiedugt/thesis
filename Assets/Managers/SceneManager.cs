using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    public void ReloadScene()
    {
        Scene currentScene = UnitySceneManager.GetActiveScene();
        UnitySceneManager.LoadScene(currentScene.name);
    }

    public void LoadGameScene()
    {
        UnitySceneManager.LoadScene("Game");
    }

    public void LoadGarageScene()
    {
        UnitySceneManager.LoadScene("Garage");
    }

    public bool IsGameScene()
    {
        return UnitySceneManager.GetActiveScene().name == "Game";
    }

    public bool IsGarageScene()
    {
        return UnitySceneManager.GetActiveScene().name == "Garage";
    }
}