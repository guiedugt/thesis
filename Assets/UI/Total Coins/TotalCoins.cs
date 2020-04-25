using TMPro;
using UnityEngine;
 
public class TotalCoins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountText;

    void Start()
    {
        UpdateAmount();
    }

    public void UpdateAmount()
    {
        float score = PlayerPrefsManager.GetTotalScore();
        amountText.text = score.ToString("#,##0");
    }
}
