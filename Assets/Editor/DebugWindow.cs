using System.Linq;
using UnityEngine;
using UnityEditor;

public class DebugWindow : EditorWindow
{
    [MenuItem("Window/Debug Helpers")]
    public static void ShowWindow()
    {
        GetWindow<DebugWindow>("Debug Helpers");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Clear Player Prefs")) ClearPlayerPrefs();
        if (GUILayout.Button("Add 500 coins")) AddCoins(500);
        if (GUILayout.Button("Unlock All Cars")) ChangeAllCarsLockedState(isUnlocked: true);
        if (GUILayout.Button("Lock All Cars")) ChangeAllCarsLockedState(isUnlocked: false);
        if (GUILayout.Button("(Un)FreezeTime")) UnFreezeTime();
        if (GUILayout.Button("Spawn Coins")) SpawnCoins(5);
    }

    void ClearPlayerPrefs()
    {
        PlayerPrefsManager.Clear();
    }

    void AddCoins(float amount)
    {
        PlayerPrefsManager.AddToTotalScore(amount);
    }

    void ChangeAllCarsLockedState(bool isUnlocked)
    {
        SelectableCar[] selectableCars = PlayerPrefsManager.GetSelectableCars();
        selectableCars = selectableCars.Select(t => {
            t.isUnlocked = isUnlocked;
            return t;
        }).ToArray();
        PlayerPrefsManager.SetSelectableCars(selectableCars);
    }

    void UnFreezeTime()
    {
        if (Time.timeScale <= Mathf.Epsilon) Time.timeScale = 1f;
        else Time.timeScale = 0f;
    }

    void SpawnCoins(int amount)
    {
        FindObjectOfType<CoinSpawner>().SpawnCoins(amount);
    }
}