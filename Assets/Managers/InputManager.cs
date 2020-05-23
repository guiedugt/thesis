using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    [Header("Tap")]
    [SerializeField] float tapDistanceTolerance = 50f;

    [Header("Bomb")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float throwPower = 7f;
    [SerializeField] RectTransform bombThrowTouchArea;
    public Transform bombOrigin;
    public float bombDelay = 1f;
    public class BombThrowEvent : UnityEvent<Vector3> { }
    public BombThrowEvent OnBombThrow = new BombThrowEvent();
    public UnityEvent OnBombRecharge;

    [Header("Super Bomb")]
    [SerializeField] GameObject superBombPrefab;
    public float superBombDuration = 15f;
    [HideInInspector] public float superBombRemainingTime = 10f;

    float timeSinceLastBombThrow = 0f;
    bool canThrowBomb = true;
    bool isSwiping = false;
    public bool isSuperBombActive = false;
    Vector2 fingerDownPosition;
    Vector2 fingerUpPorision;
    Vector2 swipe;
    MainCamera mainCamera;
    Vector3 carPositionOnFingerDown;
    Vector3 moveDirection;
    Car car;

    void Start()
    {
        mainCamera = GameManager.mainCamera;
        car = GameManager.car;
        bombOrigin = GameObject.FindGameObjectWithTag("Bomb Origin").transform;
    }

    void Update()
    {
        HandleTouch();
        HandleKeyboard();
        HandleBombDelay();
        HandleBombThrow();
    }

    void HandleBombDelay()
    {
        timeSinceLastBombThrow += Time.deltaTime;
        if (timeSinceLastBombThrow >= bombDelay && !canThrowBomb)
        {
            canThrowBomb = true;
            if (OnBombRecharge != null) OnBombRecharge.Invoke();
        }
    }

    void HandleBombThrow()
    {
        if (!GameManager.isGameRunning || GameManager.isGameOver || Input.touches.Length <= 0) return;

        Touch touch = Input.GetTouch(0);
        bool isTap = touch.phase == TouchPhase.Ended && swipe.magnitude <= tapDistanceTolerance;
        if (!isTap) return;

        bool isInsideBounds = RectTransformUtility.RectangleContainsScreenPoint(bombThrowTouchArea, Input.mousePosition);
        if (!canThrowBomb || !isInsideBounds) return;

        Vector3 tapPosition = mainCamera.GetTapWorldPoint();

        GameObject bomb = Instantiate(isSuperBombActive ? superBombPrefab : bombPrefab, bombOrigin.position, Quaternion.identity, MemoryManager.Instance.transform);
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

        Vector3 throwDirection = Vector3.Normalize(tapPosition - bombOrigin.position);
        Vector3 bombTorque = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0f, 360f));

        bombRigidbody.velocity = throwDirection * throwPower;
        bombRigidbody.AddTorque(bombTorque);

        canThrowBomb = false;
        timeSinceLastBombThrow = 0f;
        OnBombThrow.Invoke(tapPosition);
    }

    void HandleKeyboard()
    {
        if (!GameManager.isGameRunning) return;

        if (Input.GetKeyDown(KeyCode.A)) car.Move(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) car.Move(Vector3.right);
    }

    void HandleTouch()
    {
        if (!GameManager.isGameRunning || Input.touches.Length <= 0) return;
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
        {
            isSwiping = false;
            moveDirection = Vector3.zero;
            fingerDownPosition = touch.position;
            carPositionOnFingerDown = car.transform.position;
        }

        swipe = touch.position - fingerDownPosition;

        if (touch.phase == TouchPhase.Moved)
        {
            bool isUnderTapTolerance = swipe.magnitude <= tapDistanceTolerance;
            if (!isUnderTapTolerance) isSwiping = true;
            if (!isSwiping) return;

            float xSwipeDir = swipe.normalized.x;
            // swiped left
            if (xSwipeDir <= -0.5f && moveDirection != Vector3.left)
            {
                moveDirection = Vector3.left;
                car.Move(moveDirection);
                return;
            }

            // moved right
            if (xSwipeDir >= 0.5f && moveDirection != Vector3.right)
            {
                moveDirection = Vector3.right;
                car.Move(moveDirection);
                return;
            }

            // returned to center
            if (xSwipeDir > -0.5f && xSwipeDir < 0.5f && moveDirection != Vector3.zero)
            {
                car.Move(moveDirection * -1);
                moveDirection = Vector3.zero;
                return;
            }
        }
    }

    public void ActivateSuperBomb()
    {
        isSuperBombActive = true;
        StartCoroutine(TickSuperBombCoroutine());
    }

    IEnumerator TickSuperBombCoroutine()
    {
        superBombRemainingTime = superBombDuration;
        while (superBombRemainingTime >= 0f)
        {
            superBombRemainingTime -= Time.deltaTime;
            yield return null;
        }
        superBombRemainingTime = 0f;
        isSuperBombActive = false;
    }
}