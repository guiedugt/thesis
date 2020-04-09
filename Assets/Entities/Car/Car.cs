using System.Collections;
using UnityEngine;

enum Position
{
    Left,
    Center,
    Right
}

public class Car : MonoBehaviour
{
    [Header("Car")]
    [SerializeField] float swipeSpeed = 0.2f;
    [SerializeField] float swipeDisplacement = 3f;

    [Header("Cannon")]
    [SerializeField] GameObject cannonBase;
    [SerializeField] GameObject cannonPipe;
    [SerializeField][Range(0f, 1f)] float cannonPipeRecoilAmount = 0.5f;
    [SerializeField][Range(0f, 5f)] float cannonPipeRecoilDelay = 3f;
    [SerializeField] Color cannonPipeRecoilColor = Color.red;
    [SerializeField] ParticleSystem rechargeVfx;

    Position position = Position.Center;
    Vector3 velocity;
    Vector3 startPosition;
    InputManager inputManager;
    float cannonPipeVelocity;
    MeshRenderer cannonPipeMesh;
    Color initialCannonPipeColor;
    Animator anim;
    Coroutine moveCoroutine;

    void Start()
    {
        startPosition = transform.position;
        anim = GetComponent<Animator>();
        cannonPipeMesh = cannonPipe.GetComponent<MeshRenderer>();
        inputManager = InputManager.Instance;
        initialCannonPipeColor = cannonPipeMesh.material.color;
        inputManager.OnBombThrow.AddListener(OnBombThrow);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
    }

    public void Move(Vector3 direction)
    {
        moveCoroutine = StartCoroutine(MoveCoroutine(direction));
    }

    IEnumerator MoveCoroutine(Vector3 direction)
    {
        if (direction == Vector3.left)
        {
            if (position == Position.Left) { yield break; }
            if (position == Position.Center) { position = Position.Left; }
            if (position == Position.Right) { position = Position.Center; }
        }

        if (direction == Vector3.right)
        {
            if (position == Position.Right) { yield break; }
            if (position == Position.Center) { position = Position.Right; }
            if (position == Position.Left) { position = Position.Center; }
        }

        Vector3 targetPosition = startPosition;
        if (position == Position.Left) { targetPosition += Vector3.left * swipeDisplacement; }
        if (position == Position.Right) { targetPosition += Vector3.right * swipeDisplacement; }

        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                swipeSpeed
            );
            yield return null;
        }

        transform.position = targetPosition;
    }

    void HandleGameOver()
    {
        anim.SetTrigger("Crash");
    }

    void HandleGameRestart()
    {
        position = Position.Center;
    }

    void OnBombThrow(GameObject bomb, Vector3 clickPosition)
    {
        cannonBase.transform.LookAt(clickPosition);
        Vector3 recoilDirection = Vector3.Normalize(clickPosition - cannonBase.transform.position);
        StartCoroutine(CannonPipeRecoilCoroutine(recoilDirection));
        StartCoroutine(CannonPipeColorCoroutine());
    }

    IEnumerator CannonPipeRecoilCoroutine(Vector3 recoilDirection)
    {

        Vector3 initialCannonPipePosition = cannonPipe.transform.position;
        float currentOffset = cannonPipeRecoilAmount;
        float previousOffset = 0f;
        float deltaOffset = 0f;

        while (Mathf.Abs(currentOffset) > Mathf.Epsilon)
        {
            // Handle cannon pipe position offset
            currentOffset = Mathf.SmoothDamp(currentOffset, 0f, ref cannonPipeVelocity, cannonPipeRecoilDelay);

            deltaOffset = currentOffset - previousOffset;
            cannonPipe.transform.position = cannonPipe.transform.position - recoilDirection * deltaOffset;

            previousOffset = currentOffset;

            // Loop frame
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CannonPipeColorCoroutine()
    {
        float startTime = Time.timeSinceLevelLoad;
        float t = 0f;
        cannonPipeMesh.material.color = cannonPipeRecoilColor;
        while (!cannonPipeMesh.material.color.Equals(initialCannonPipeColor) && t < 1f)
        {
            // Handle cannon pipe color
            t = (Time.timeSinceLevelLoad - startTime) / inputManager.bombDelay;
            cannonPipeMesh.material.color = Color.Lerp(cannonPipeRecoilColor, initialCannonPipeColor, t);

            // Loop frame
            yield return new WaitForEndOfFrame();
        }

        Instantiate(rechargeVfx, inputManager.bombOrigin.position, Quaternion.identity, cannonPipe.transform);
    }
}