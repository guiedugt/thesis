using UnityEngine;

[RequireComponent(typeof(UIFade))]
public class Alert : MonoBehaviour
{
    UIFade fade;

    public void Show(string message)
    {
        fade.Show();
    }

    public void Hide()
    {
        fade.Hide();
    }
}