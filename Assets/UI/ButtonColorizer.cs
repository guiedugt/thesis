using UnityEngine;
using UnityEngine.UI;
 
[RequireComponent(typeof(Button))]
public class ButtonColorizer : MonoBehaviour
{
    [SerializeField] Color color = Color.white;
    [SerializeField][Range(0f, 1f)] float highlightColorOffset = 0.04f;
    [SerializeField][Range(0f, 1f)] float highlightAlphaOffset = 0.04f;
    [SerializeField][Range(0f, 1f)] float pressedColorOffset = 0.2f;
    [SerializeField][Range(0f, 1f)] float pressedAlphaOffset = 0.2f;
    [SerializeField][Range(0f, 1f)] float selectedColorOffset = 0.05f;
    [SerializeField][Range(0f, 1f)] float selectedAlphaOffset = 0f;
    [SerializeField][Range(0f, 1f)] float disabledColorOffset = 0.2f;
    [SerializeField][Range(0f, 1f)] float disabledAlphaOffset = 0.5f;
    [SerializeField][Range(0f, 1f)] float colorMultiplier = 1f;
    [SerializeField] float colorFadeDuration = 0.1f;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        CalculateColors();
    }

    public void CalculateColors()
    {
        CalculateColors(color);
    }

    public void CalculateColors(Color newColor)
    {
        color = newColor;

        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        Color hsvColor = Color.HSVToRGB(h, s, v);
        Color tmpColor;

        ColorBlock colors = new ColorBlock();
        colors.normalColor = color;
        tmpColor = Color.HSVToRGB(h, s, v - highlightColorOffset);
        colors.highlightedColor = new Color(tmpColor.r, tmpColor.g, tmpColor.b, color.a - highlightAlphaOffset);
        tmpColor = Color.HSVToRGB(h, s, v - pressedColorOffset);
        colors.pressedColor = new Color(tmpColor.r, tmpColor.g, tmpColor.b, color.a - pressedAlphaOffset);
        tmpColor = Color.HSVToRGB(h, s, v - selectedColorOffset);
        colors.selectedColor = new Color(tmpColor.r, tmpColor.g, tmpColor.b, color.a - selectedAlphaOffset);
        tmpColor = Color.HSVToRGB(h, s, v - disabledColorOffset);
        colors.disabledColor = new Color(tmpColor.r, tmpColor.g, tmpColor.b, color.a - disabledAlphaOffset);

        colors.colorMultiplier = colorMultiplier;
        colors.fadeDuration = colorFadeDuration;

        button.colors = colors;
    }
}