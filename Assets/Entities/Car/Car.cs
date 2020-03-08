using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Cannon")]
    [SerializeField] GameObject cannonBase;
    [SerializeField] GameObject cannonPipe;
    [SerializeField][Range(0f, 1f)] float recoilAmount = 0.5f;
    [SerializeField][Range(0f, 5f)] float pipeRecoilDelay = 3f;

    float pipeVelocity;

    void Start()
    {
        InputManager.OnBombThrow.AddListener(OnBombThrow);
    }

    void OnBombThrow(GameObject bomb, Vector3 clickPosition)
    {
        cannonBase.transform.LookAt(clickPosition);
        Vector3 recoilDirection = Vector3.Normalize(clickPosition - cannonBase.transform.position);
        StartCoroutine(PipeRecoilCoroutine(recoilDirection));
    }

    IEnumerator PipeRecoilCoroutine(Vector3 recoilDirection)
    {

        Vector3 initialCannonPipePosition = cannonPipe.transform.position;
        float previousOffset = 0f;
        float currentOffset = recoilAmount;
        float deltaOffset;
        while (Mathf.Abs(currentOffset) > Mathf.Epsilon)
        {
            currentOffset = Mathf.SmoothDamp(currentOffset, 0f, ref pipeVelocity, pipeRecoilDelay);

            deltaOffset = currentOffset - previousOffset;
            cannonPipe.transform.position = cannonPipe.transform.position - recoilDirection * deltaOffset;

            previousOffset = currentOffset;
            yield return new WaitForEndOfFrame();
        }
    }
}