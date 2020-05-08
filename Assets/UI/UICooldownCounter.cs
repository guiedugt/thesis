using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Image fillImage;

    void Start()
    {
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Radial360;
        countText.text = "";
    }

    public void ShowCooldown(float duration)
    {
        StartCoroutine(ShowCooldownCoroutine(duration));
    }

    IEnumerator ShowCooldownCoroutine(float duration)
    {
        float t = duration;
        while (t > 0f) {
            countText.text = Mathf.Ceil(t).ToString();
            fillImage.fillAmount = t / duration;
            t -= Time.deltaTime;
            yield return null;
        }

        countText.text = "0";
        fillImage.fillAmount = 0f;
    }
}