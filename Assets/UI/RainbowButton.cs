using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ButtonColorizer))]
public class RainbowButton : MonoBehaviour
{
    [Header("Color")]
    [SerializeField][Range(0f, 1f)] float saturation = 0.5f;
    [SerializeField][Range(0f, 1f)] float value = 0.9f;
    [SerializeField][Range(0f, 1f)] float speed = 0.5f;

    ButtonColorizer colorizer;

    void Start()
    {
        colorizer = GetComponent<ButtonColorizer>();
        StartCoroutine(RainbowCoroutine());
    }

    IEnumerator RainbowCoroutine()
    {
        while(!GameManager.isGameRunning && !GameManager.isGameOver) {
            float hue = Time.time * speed % 1f;
            Color color = Color.HSVToRGB(hue, saturation, value);
            colorizer.CalculateColors(color);
            yield return null;
        }
    }
}