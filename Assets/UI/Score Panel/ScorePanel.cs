using System.Collections.Generic;
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
        ClearScore();
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameOver()
    {
        CalculateScore();
    }

    public void ClearScore()
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

    public void CalculateScore()
    {
        Dictionary<ScoreType, ScoreItem> scoresByType = ScoreManager.Instance.scoresByType;
        levelAmountText.text = "x " + scoresByType[ScoreType.Level].Amount.ToString("#,##0");
        levelCoinsText.text = scoresByType[ScoreType.Level].Score.ToString("#,##0");
        brickAmountText.text = "x " + scoresByType[ScoreType.Brick].Amount.ToString("#,##0");
        brickCoinsText.text = scoresByType[ScoreType.Brick].Score.ToString("#,##0");
        timeAmountText.text = "x " + scoresByType[ScoreType.Time].Amount.ToString("#,##0");
        timeCoinsText.text = scoresByType[ScoreType.Time].Score.ToString("#,##0");
        gameCoins.text = "+ " + ScoreManager.Instance.Score.ToString("#,##0");
        totalCoins.text = ScoreManager.Instance.TotalScore.ToString("#,##0");
    }
}
