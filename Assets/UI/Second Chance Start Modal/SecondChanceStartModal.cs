using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(UIFade))]
public class SecondChanceStartModal : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float delay = 3f;
    [SerializeField] TextMeshProUGUI counterText;

    UIFade fade;
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
    bool alreadyShowedAlert = false;

    void Start()
    {
        fade = GetComponent<UIFade>();
        SecondChanceManager.Instance.OnTrigger.AddListener(HandleSecondChance);
    }

    void HandleSecondChance()
    {
        if (!alreadyShowedAlert) StartCoroutine(CounterCoroutine());
        alreadyShowedAlert = true;
    }

    IEnumerator CounterCoroutine()
    {
        Time.timeScale = 0f;
        float t = delay;
        while (t > Mathf.Epsilon)
        {
            counterText.text = t.ToString("0");
            t -= 1f;
            yield return wait;
        }
        counterText.text = "Go!!";
        Time.timeScale = 1f;
        fade.Hide();
        GameManager.isGameRunning = true;
    }
}