using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car")]
    [SerializeField] float swipeSpeed = 0.2f;
    [SerializeField] float swipeDisplacement = 4f;

    [Header("Cannon")]
    [SerializeField] GameObject cannonBase;
    [SerializeField] GameObject cannonPipe;
    [SerializeField][Range(0f, 1f)] float cannonPipeRecoilAmount = 0.5f;
    [SerializeField][Range(0f, 5f)] float cannonPipeRecoilDelay = 3f;
    [SerializeField] Color cannonPipeRecoilColor = Color.red;
    [SerializeField] ParticleSystem rechargeVfx;

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
    }

    public void Move(Vector3 direction)
    {
        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }
        moveCoroutine = StartCoroutine(MoveCoroutine(direction));
    }

    IEnumerator MoveCoroutine(Vector3 direction)
    {
        Vector3 offset = swipeDisplacement * direction;
        Vector3 originPosition = transform.position;
        Vector3 targetPosition = originPosition + offset;
        if (Vector3.Distance(startPosition, targetPosition) >= swipeDisplacement) { yield break; }

        while (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                originPosition + offset,
                ref velocity,
                swipeSpeed
            );
            yield return null;
        }
    }

    void HandleGameOver()
    {
        anim.SetTrigger("Crash");
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