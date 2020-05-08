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

    public void Show(Action callback = null)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true, callback));
    }

    public void Hide(Action callback = null)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: false, callback));
    }

    IEnumerator FadeCoroutine(bool isFadeIn, Action callback)
    {
        canvasGroup.interactable = isFadeIn;
        canvasGroup.blocksRaycasts = isFadeIn;
        canvasGroup.alpha = isFadeIn ? 1f : 0f;

        float t = 0f;
        float startTime = Time.time;
        while (t < 1f)
        {
            if (!canvasGroup) yield return null;
            t = Mathf.Min((Time.time - startTime) / duration, 1f);
            canvasGroup.alpha = isFadeIn ? t : 1f - t;
            yield return null;
        }

        if (callback != null) callback();
    }
}