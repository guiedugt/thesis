using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [Header("Bombs")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField][Range(1f, 10f)] float throwPower = 7f;
    [SerializeField][Range(1f, 20f)] float touchCameraDistance = 5f;

    [Header("Camera")]
    [SerializeField] float maxTilt = 0.15f;
    [SerializeField] float cameraRotation = 10f;
    [SerializeField] float cameraDisplacement = 2f;
    [SerializeField] float cameraChangeSpeed = 0.4f;

    new Camera camera;
    Vector3 initialCameraPosition;
    Quaternion initialCameraRotation;
    Vector3 previousCameraPosition;
    Quaternion previousCameraRotation;
    Vector3 cameraVelocity;
    float cameraAngularVelocity;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        initialCameraPosition = camera.transform.position;
        initialCameraRotation = camera.transform.rotation;
        previousCameraPosition = initialCameraPosition;
        previousCameraRotation = initialCameraRotation;
        cameraVelocity = Vector3.zero;
    }

    void Update()
    {
        HandleBombThrow();
    }

    void FixedUpdate()
    {
        HandleCamera();
    }

    void HandleBombThrow()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 clickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, touchCameraDistance);
        Vector3 bombPosition = GameManager.camera.ScreenToWorldPoint(clickPosition);

        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity, MemoryManager.Instance.transform);
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

        Vector3 throwDirection = bombPosition - GameManager.camera.transform.position;
        Vector3 bombTorque = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0f, 360f));

        bombRigidbody.velocity = throwDirection * throwPower;
        bombRigidbody.AddTorque(bombTorque);
    }

    void HandleCamera()
    {
        float tilt = Input.acceleration.x;

        bool isLeft = tilt < -maxTilt;
        bool isRight = tilt > maxTilt;

        Vector3 positionOffset = isLeft ?
            Vector3.left * cameraDisplacement :
            isRight ?
            Vector3.right * cameraDisplacement :
            Vector3.zero;

        Vector3 targetPosition = initialCameraPosition + positionOffset;

        if (Vector3.Distance(camera.transform.position, targetPosition) > Mathf.Epsilon)
            camera.transform.position = Vector3.SmoothDamp(
                camera.transform.position,
                initialCameraPosition + positionOffset,
                ref cameraVelocity,
                cameraChangeSpeed
            );

        float rotationOffset = isLeft ? cameraRotation : isRight ? -cameraRotation : 0f;
        Quaternion targetRotation = initialCameraRotation * Quaternion.Euler(0f, rotationOffset, 0f);
        float rotationDelta = Quaternion.Angle(camera.transform.rotation, targetRotation);
        if (rotationDelta > float.Epsilon)
        {
            float t = Mathf.SmoothDampAngle(rotationDelta, 0.0f, ref cameraAngularVelocity, cameraChangeSpeed);
            t = 1.0f - t / rotationDelta;
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, t);
        }

    }
}