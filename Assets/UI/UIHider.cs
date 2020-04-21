using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIHider : MonoBehaviour
{
    [Header("Presentation")]
    [SerializeField] float fadeDuration = 0.4f;

    [Header("Screens to show UI")]
    [SerializeField] bool showInStartScreen = false;
    [SerializeField] bool showInGameScreen = false;
    [SerializeField] bool showInEndScreen = false;

    CanvasGroup canvasGroup;
    Coroutine fadeCoroutine;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        if (showInStartScreen) fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true));
    }

    void HandleGameStart()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (showInStartScreen)  fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: false));
        if (showInGameScreen) fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true));
    }

    void HandleGameOver()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (showInGameScreen)  fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: false));
        if (showInEndScreen) fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true));
    }

    void HandleGameRestart()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (showInEndScreen)  fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: false));
        if (showInStartScreen) fadeCoroutine = StartCoroutine(FadeCoroutine(isFadeIn: true));
    }

    IEnumerator FadeCoroutine(bool isFadeIn)
    {
        float t = 0f;
        float startTime = Time.time;
        while (t < 1f)
        {
            t = Mathf.Min((Time.time - startTime) / fadeDuration, 1f);
            canvasGroup.alpha = isFadeIn ? t : 1f - t;
            yield return null;
        }
    }
}