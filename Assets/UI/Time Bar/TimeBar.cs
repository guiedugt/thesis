using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TimeBar : MonoBehaviour
{
    [SerializeField] float duration = 3f;

    Slider slider;
    Coroutine tickCoroutine;
    bool isTickPaused = false;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void Tick(Action callback = null)
    {
        tickCoroutine = StartCoroutine(TickCoroutine(callback));
    }

    public void Pause()
    {
        isTickPaused = true;
    }

    public void Resume()
    {
        isTickPaused = false;
    }

    public void Stop()
    {
        isTickPaused = false;
        StopCoroutine(tickCoroutine);
    }

    IEnumerator TickCoroutine(Action callback)
    {
        slider.value = 1f;
        while (slider.value > 0f)
        {
            if (!isTickPaused) slider.value -= Time.deltaTime / duration;
            yield return null;
        }
        if (callback != null) callback();
    }
}