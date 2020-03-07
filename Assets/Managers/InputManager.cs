using UnityEngine;
using UnityEngine.Events;


public class InputManager : Singleton<InputManager>
{
    [Header("Bombs")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField][Range(1f, 10f)] float throwPower = 7f;
    [SerializeField][Range(1f, 50f)] float touchCameraDistance = 5f;
    [SerializeField] Transform bombOrigin;

    [Header("Camera")]
    [SerializeField] float maxTilt = 0.15f;
    [SerializeField] float cameraRotation = 10f;
    [SerializeField] float cameraDisplacement = 2f;
    [SerializeField] float cameraChangeSpeed = 0.4f;
    [SerializeField] float playerTiltDisplacement = 2f;
    [SerializeField] float playerChangeSpeed = 0.4f;
    [Tooltip("Used for debugging only")][Range(-1f, 1f)][SerializeField] float fakeTilt = 0f;

    public class BombThrowEvent : UnityEvent<GameObject, Vector3> { }
    public static BombThrowEvent OnBombThrow = new BombThrowEvent();

    new Camera camera;
    Vector3 initialCameraPosition;
    Quaternion initialCameraRotation;
    Vector3 previousCameraPosition;
    Quaternion previousCameraRotation;
    Vector3 cameraVelocity;
    float cameraAngularVelocity;

    GameObject player;
    Vector3 initialPlayerPosition;
    Vector3 playerVelocity;

    void Start()
    {
        camera = GameManager.camera;
        initialCameraPosition = camera.transform.position;
        initialCameraRotation = camera.transform.rotation;
        previousCameraPosition = initialCameraPosition;
        previousCameraRotation = initialCameraRotation;
        cameraVelocity = Vector3.zero;

        player = GameManager.player;
        initialPlayerPosition = player.transform.position;
    }

    void Update()
    {
        HandleBombThrow();
    }

    void FixedUpdate()
    {
        HandleTilt();
    }

    void HandleBombThrow()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 clickScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, touchCameraDistance);
        Vector3 clickPosition = GameManager.camera.ScreenToWorldPoint(clickScreenPosition);

        GameObject bomb = Instantiate(bombPrefab, bombOrigin.position, Quaternion.identity, MemoryManager.Instance.transform);
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

        Vector3 throwDirection = clickPosition - bombOrigin.position;
        Vector3 bombTorque = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0f, 360f));

        bombRigidbody.velocity = throwDirection * throwPower;
        bombRigidbody.AddTorque(bombTorque);

        OnBombThrow.Invoke(bomb, clickPosition);
    }

    void HandleTilt()
    {
        fakeTilt = Input.GetAxis("Horizontal");
        float tilt = (Mathf.Abs(fakeTilt) >= Mathf.Epsilon) ? fakeTilt : Input.acceleration.x;

        bool isLeft = tilt < -maxTilt;
        bool isRight = tilt > maxTilt;
        Vector3 positionOffsetDirection = isLeft ? Vector3.left : isRight ? Vector3.right : Vector3.zero;

        Vector3 cameraPositionOffset = positionOffsetDirection * cameraDisplacement;
        MoveCamera(cameraPositionOffset);

        float cameraRotationOffset = isLeft ? cameraRotation : isRight ? -cameraRotation : 0f;
        RotateCamera(cameraRotationOffset);

        // Move Player
        Vector3 playerPositionOffset = positionOffsetDirection * playerTiltDisplacement;
        MovePlayer(playerPositionOffset);
    }

    void MoveCamera(Vector3 positionOffset)
    {
        Vector3 targetPosition = initialCameraPosition + positionOffset;
        if (Vector3.Distance(camera.transform.position, targetPosition) > Mathf.Epsilon)
        {
            camera.transform.position = Vector3.SmoothDamp(
                camera.transform.position,
                initialCameraPosition + positionOffset,
                ref cameraVelocity,
                cameraChangeSpeed
            );
        }
    }

    void RotateCamera(float rotationOffset)
    {
        Quaternion targetRotation = initialCameraRotation * Quaternion.Euler(0f, rotationOffset, 0f);
        float rotationDelta = Quaternion.Angle(camera.transform.rotation, targetRotation);
        if (rotationDelta > Mathf.Epsilon)
        {
            float t = Mathf.SmoothDampAngle(rotationDelta, 0.0f, ref cameraAngularVelocity, cameraChangeSpeed);
            t = 1.0f - t / rotationDelta;
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, t);
        }
    }

    void MovePlayer(Vector3 positionOffset)
    {
        Vector3 targetPosition = initialPlayerPosition + positionOffset;
        if (Vector3.Distance(player.transform.position, targetPosition) > Mathf.Epsilon)
        {
            player.transform.position = Vector3.SmoothDamp(
                player.transform.position,
                initialPlayerPosition + positionOffset,
                ref playerVelocity,
                playerChangeSpeed
            );
        }
    }
}