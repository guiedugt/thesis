using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    [Header("Bombs")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float throwPower = 7f;
    [SerializeField] RectTransform bombThrowTouchArea;
    public Transform bombOrigin;
    public float bombDelay = 1f;

    float timeSinceLastBombThrow = 0f;
    public class BombThrowEvent : UnityEvent<GameObject, Vector3> { }
    public BombThrowEvent OnBombThrow = new BombThrowEvent();

    Vector2 fingerDownPosition;
    Vector2 fingerUpPosition;
    Vector2 swipeDirection;
    new MainCamera camera;
    Car car;

    void Start()
    {
        camera = GameManager.camera;
        car = GameManager.car;
    }

    void Update()
    {
        HandleTouch();
        HandleGameStart();
        HandleBombThrow();
    }

    void HandleGameStart()
    {
        if (!Input.GetMouseButtonDown(0) || GameManager.isGameRunning || GameManager.isGameOver) { return; }

        GameManager.Instance.StartGame();
    }

    void HandleBombThrow()
    {
        timeSinceLastBombThrow += Time.deltaTime;
        if (GameManager.isGameOver || !Input.GetMouseButtonDown(0)) { return; }

        bool isInsideBounds = RectTransformUtility.RectangleContainsScreenPoint(bombThrowTouchArea, Input.mousePosition);
        if (timeSinceLastBombThrow < bombDelay || !isInsideBounds) return;

        Vector3 tapPosition = camera.GetTapWorldPoint();

        GameObject bomb = Instantiate(bombPrefab, bombOrigin.position, Quaternion.identity, MemoryManager.Instance.transform);
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

        Vector3 throwDirection = Vector3.Normalize(tapPosition - bombOrigin.position);
        Vector3 bombTorque = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0f, 360f));

        bombRigidbody.velocity = throwDirection * throwPower;
        bombRigidbody.AddTorque(bombTorque);

        timeSinceLastBombThrow = 0f;

        OnBombThrow.Invoke(bomb, tapPosition);
    }

    void HandleTouch()
    {
        if (!GameManager.isGameRunning) { return; }
 
        if (Input.GetKeyDown(KeyCode.A)) { car.Move(Vector3.left); }
        if (Input.GetKeyDown(KeyCode.D)) { car.Move(Vector3.right); }

        if (Input.touches.Length <= 0) { return; }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            fingerDownPosition = new Vector2(touch.position.x, touch.position.y);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            fingerUpPosition = new Vector2(touch.position.x, touch.position.y);
            swipeDirection = (fingerUpPosition - fingerDownPosition).normalized;

            bool swipedSideways = Mathf.Abs(swipeDirection.y) < 0.5f && swipeDirection.x != 0;
            if (!swipedSideways) return;

            bool swipedLeft = swipeDirection.x < 0f;
            bool swipedRight = swipeDirection.x > 0f;
            Vector3 positionOffsetDirection = swipedLeft ? Vector3.left : swipedRight ? Vector3.right : Vector3.zero;
            car.Move(positionOffsetDirection);
        }
    }
}