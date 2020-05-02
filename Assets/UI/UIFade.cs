using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    [SerializeField] float duration = 0.4f;
    CanvasGroup canvasGroup;
    Coroutine fadeCoroutine;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true));
    }

    public void Hide()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: false));
    }

    IEnumerator FadeCoroutine(bool isFadeIn)
    {
        float t = 0f;
        float startTime = Time.time;
        while (t < 1f)
        {
            t = Mathf.Min((Time.time - startTime) / duration, 1f);
            canvasGroup.alpha = isFadeIn ? t : 1f - t;
            yield return null;
        }
    }
}