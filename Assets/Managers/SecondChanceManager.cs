using UnityEngine.Events;

public class SecondChanceManager : Singleton<SecondChanceManager>
{
    public static bool isSecondChance = false;
    public UnityEvent OnTrigger;

    void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
    }

    void HandleGameStart()
    {
        isSecondChance = false;
    }

    public void Trigger()
    {
        isSecondChance = true;
        GameManager.Instance.RestartGame(isSecondChance: true);
        if (OnTrigger != null) OnTrigger.Invoke();
    }
}