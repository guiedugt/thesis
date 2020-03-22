using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Cannon")]
    [SerializeField] GameObject cannonBase;
    [SerializeField] GameObject cannonPipe;
    [SerializeField][Range(0f, 1f)] float cannonPipeRecoilAmount = 0.5f;
    [SerializeField][Range(0f, 5f)] float cannonPipeRecoilDelay = 3f;
    [SerializeField] Color cannonPipeRecoilColor = Color.red;
    [SerializeField] ParticleSystem rechargeVfx;

    InputManager inputManager;
    float cannonPipeVelocity;
    MeshRenderer cannonPipeMesh;
    Color initialCannonPipeColor;
    Animator anim;

    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.OnBombThrow.AddListener(OnBombThrow);
        cannonPipeMesh = cannonPipe.GetComponent<MeshRenderer>();
        initialCannonPipeColor = cannonPipeMesh.material.color;
        anim = GetComponent<Animator>();
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
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