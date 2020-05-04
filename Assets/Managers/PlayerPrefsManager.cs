using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }

    public static float GetTotalScore()
    {
        return PlayerPrefs.GetFloat("TotalScore");
    }

    public static void SetTotalScore(float value)
    {
        PlayerPrefs.SetFloat("TotalScore", value);
    }

    public static void AddToTotalScore(float value)
    {
        SetTotalScore(GetTotalScore() + value);
    }

    public static void SubToTotalScore(float value)
    {
        SetTotalScore(GetTotalScore() - value);
    }

    public static int GetSelectedCarId()
    {
        int val = PlayerPrefs.GetInt("SelectedCarId");
        if (val == 0) return 1;
        return val;
    }

    public static void SetSelectedCarId(int id)
    {
        PlayerPrefs.SetInt("SelectedCarId", id);
    }

    public static SelectableCar[] GetSelectableCars()
    {
        string json = PlayerPrefs.GetString("SelectableCars");
        if (json.Length == 0) return null;
        return JSON.ParseArray<SelectableCar>(json);
    }

    public static void SetSelectableCars (SelectableCar[] selectables)
    {
        PlayerPrefs.SetString("SelectableCars", JSON.SerializeArray(selectables));
    }
}