using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelAmountText;
    [SerializeField] TextMeshProUGUI levelCoinsText;
    [SerializeField] TextMeshProUGUI brickAmountText;
    [SerializeField] TextMeshProUGUI brickCoinsText;
    [SerializeField] TextMeshProUGUI timeAmountText;
    [SerializeField] TextMeshProUGUI timeCoinsText;
    [SerializeField] TextMeshProUGUI gameCoins;
    [SerializeField] TextMeshProUGUI totalCoins;

    void Start()
    {
        levelAmountText.text = "x 0";
        levelCoinsText.text = "0";
        brickAmountText.text = "x 0";
        brickCoinsText.text = "0";
        timeAmountText.text = "x 0";
        timeCoinsText.text = "0";
        gameCoins.text = "+ 0";
        totalCoins.text = "0";
    }
}
