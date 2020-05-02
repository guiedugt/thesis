using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UIFade))]
public class UIHider : MonoBehaviour
{
    [Header("Presentation")]
    [SerializeField] float fadeDuration = 0.4f;

    [Header("Screens to show UI")]
    [SerializeField] bool showInStartScreen = false;
    [SerializeField] bool showInGameScreen = false;
    [SerializeField] bool showInEndScreen = false;

    UIFade fade;

    void Start()
    {
        fade = GetComponent<UIFade>();
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        if (showInStartScreen) fade.Show();
    }

    void HandleGameStart()
    {
        if (showInStartScreen)  fade.Hide();
        if (showInGameScreen) fade.Show();
    }

    void HandleGameOver()
    {
        if (showInGameScreen)  fade.Hide();
        if (showInEndScreen) fade.Show();
    }

    void HandleGameRestart()
    {
        if (showInEndScreen)  fade.Hide();
        if (showInStartScreen) fade.Show();
    }
}