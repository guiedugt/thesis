using UnityEngine;
using UnityEngine.Events;

public class Reloadable : MonoBehaviour
{
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public UnityEvent OnReload;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Reload()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        OnReload.Invoke();
    }
}