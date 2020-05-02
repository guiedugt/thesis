using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
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

    public static int GetSelectedCarIndex()
    {
        return PlayerPrefs.GetInt("SelectedCarIndex");
    }

    public static void SetSelectedCarIndex(int i)
    {
        PlayerPrefs.SetInt("SelectedCarIndex", i);
    }

    public static SelectableCar[] GetSelectableCars()
    {
        string json = PlayerPrefs.GetString("SelectableCars");
        if (json == "") return null;
        return JSON.ParseArray<SelectableCar>(json);
    }

    public static void SetSelectableCars (SelectableCar[] selectables)
    {
        PlayerPrefs.SetString("SelectableCars", JSON.SerializeArray(selectables));
    }
}