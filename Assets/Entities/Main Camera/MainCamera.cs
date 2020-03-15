using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeMagnitude = 0.5f;

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 initPos = transform.position;
        float t = 0f;

        while (t < shakeDuration)
        {
            float x = initPos.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = initPos.y + Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(x, y, initPos.z);

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = initPos;
    }
}