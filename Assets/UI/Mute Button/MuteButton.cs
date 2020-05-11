using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MuteButton : MonoBehaviour
{
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(Toggle);
        toggle.isOn = AudioListener.pause;
    }

    public void Toggle(bool isMuted)
    {
        AudioListener.pause = isMuted;
    }
}