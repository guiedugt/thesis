using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeMagnitude = 0.5f;
    [SerializeField] float pushDuration = 0.5f;
    [SerializeField] float pushMagnitude = 5f;
    [SerializeField] float tapDistance = 15f;
    [SerializeField] float swipeSpeed = 0.4f;

    new Camera camera;
    Vector3 initialPosition;
    Vector3 previousPosition;
    Quaternion initialRotation;
    Quaternion previousRotation;
    Vector3 velocity;
    float angularVelocity;

    void Start()
    {
        camera = GetComponent<Camera>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        previousPosition = initialPosition;
        previousRotation = initialRotation;
        velocity = Vector3.zero;
    }

    public Vector3 GetTapWorldPoint()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, tapDistance);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public void Move(Vector3 offset)
    {
        Vector3 targetPosition = initialPosition + offset;
        if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                initialPosition + offset,
                ref velocity,
                swipeSpeed
            );
        }
    }

    public void Rotate(float offset)
    {
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, offset, 0f);
        float rotationDelta = Quaternion.Angle(transform.rotation, targetRotation);
        if (rotationDelta > Mathf.Epsilon)
        {
            float t = Mathf.SmoothDampAngle(rotationDelta, 0.0f, ref angularVelocity, swipeSpeed);
            t = 1.0f - t / rotationDelta;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
        }
    }

    public void Shake(float? magnitude = null)
    {
        StartCoroutine(ShakeCoroutine(magnitude ?? shakeMagnitude));
    }

    IEnumerator ShakeCoroutine(float magnitude)
    {
        Vector3 initPos = transform.position;
        float t = 0f;

        while (t < shakeDuration)
        {
            float x = initPos.x + Random.Range(-magnitude, magnitude);
            float y = initPos.y + Random.Range(-magnitude, magnitude);

            transform.position = new Vector3(x, y, initPos.z);

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = initPos;
    }

    public void Push(float? magnitude = null)
    {
        StartCoroutine(PushCoroutine(magnitude ?? pushMagnitude));
    }

    IEnumerator PushCoroutine(float magnitude)
    {
        float velocity = 0f;
        float startingFOV = camera.fieldOfView;
        camera.fieldOfView += magnitude;
        while (camera.fieldOfView > startingFOV)
        {
            camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, startingFOV, ref velocity, pushDuration);
            yield return null;
        }

        camera.fieldOfView = startingFOV;
    }
}