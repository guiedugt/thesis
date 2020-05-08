using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(UIFade))]
[RequireComponent(typeof(UICooldownCounter))]
public class SuperBombButton : MonoBehaviour
{
    Button button;
    UIFade fade;
    UICooldownCounter cooldownCounter;

    void Start()
    {
        button = GetComponent<Button>();
        fade = GetComponent<UIFade>();
        cooldownCounter = GetComponent<UICooldownCounter>();
        
        button.onClick.AddListener(ActivateSuperBomb);
    }

    public void Show() => fade.Show();
    public void Hide() => fade.Hide();

    void ActivateSuperBomb()
    {
        InputManager.Instance.ActivateSuperBomb();
        cooldownCounter.ShowCooldown(InputManager.Instance.superBombDuration);
        InvokeAfterSeconds(Hide, InputManager.Instance.superBombDuration);
    }

    void InvokeAfterSeconds(Action callback, float delay) => StartCoroutine(InvokeAfterSecondsCoroutine(callback, delay));

    IEnumerator InvokeAfterSecondsCoroutine(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}