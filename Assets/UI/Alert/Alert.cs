using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UIFade))]
public class Alert : MonoBehaviour
{
    public UnityEvent OnConfirm;
    public UnityEvent OnCancel;

    [SerializeField] TextMeshProUGUI messageText;
    UIFade fade;

    void Start()
    {
        fade = GetComponent<UIFade>();
    }

    public void Show(string message)
    {
        messageText.text = message;
        fade.Show();
    }

    public void Cancel()
    {
        fade.Hide(() => {
            if (OnCancel != null) OnCancel.Invoke();
        });
    }

    public void Confirm()
    {
        fade.Hide(() => {
            if (OnConfirm != null) OnConfirm.Invoke();
        });
    }
}