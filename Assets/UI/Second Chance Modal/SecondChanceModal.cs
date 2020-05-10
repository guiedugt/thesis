using UnityEngine;

[RequireComponent(typeof(UIFade))]
public class SecondChanceModal : TimeBar
{
    [SerializeField] TimeBar secondChanceTimeBar;
    [SerializeField] UIFade summaryFade;

    UIFade fade;

    void Start()
    {
        fade = GetComponent<UIFade>();
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameOver()
    {
        secondChanceTimeBar.Tick(HandleTimeUp);
    }

    void HandleTimeUp()
    {
        secondChanceTimeBar.Stop();
        fade.Hide();
        summaryFade.Show();
    }
}