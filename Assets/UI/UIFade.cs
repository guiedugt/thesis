using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    [SerializeField] bool isInitiallyVisible = false;
    [SerializeField] float duration = 0.4f;
    CanvasGroup canvasGroup;
    Coroutine fadeCoroutine;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SetVisibility(isInitiallyVisible);
    }

    void SetVisibility(bool isVisible)
    {
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
        canvasGroup.alpha = isVisible ? 1f : 0f;
    }

    public void Show(Action callback = null, bool instantly = false)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        if (instantly) {
            SetVisibility(true);
            return;
        }

        fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true, callback));
    }

    public void Hide(Action callback = null, bool instantly = false)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        if (instantly) {
            SetVisibility(false);
            return;
        }

        fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: false, callback));
    }

    IEnumerator FadeCoroutine(bool isFadeIn, Action callback)
    {
        SetVisibility(isFadeIn);

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