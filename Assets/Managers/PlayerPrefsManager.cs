using UnityEngine;
 
public class PlayerPrefsManager : MonoBehaviour
{
    public static float GetTotalScore() {
        return PlayerPrefs.GetFloat("TotalScore");
    }
    
    public static void SetTotalScore(float value) {
        PlayerPrefs.SetFloat("TotalScore", value);
    }

    public static void AddToTotalScore(float value) {
        SetTotalScore(GetTotalScore() + value);
    }
}