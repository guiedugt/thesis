using System.Collections;
using UnityEngine;

enum Position
{
    Left,
    Center,
    Right
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Reloadable))]
public class Car : MonoBehaviour
{
    [SerializeField] float swipeSpeed = 0.1f;
    [SerializeField] float swipeDisplacement = 2.1f;
    [SerializeField] float CollisionUpForce = 200f;
    [SerializeField] float CollisionTorqueForce = 300f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip[] driftSFXs;
    [SerializeField] ParticleSystem crashVFX;

    Position position = Position.Center;
    Vector3 velocity;
    Vector3 startPosition;
    Quaternion startRotation;
    Animator anim;
    Coroutine moveCoroutine;
    Rigidbody rb;
    Reloadable reloadable;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        reloadable = GetComponent<Reloadable>();
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        SecondChanceManager.Instance.OnTrigger.AddListener(HandleSecondChance);
        reloadable.OnReload.AddListener(HandleReload);
    }

    public void Move(Vector3 direction)
    {
        Coroutine newMoveCoroutine = StartCoroutine(MoveCoroutine(direction));
        if (newMoveCoroutine != null) { moveCoroutine = newMoveCoroutine; }
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

        if (moveCoroutine != null) { StopCoroutine(moveCoroutine); }

        Vector3 targetPosition = startPosition;
        if (position == Position.Left) { targetPosition += Vector3.left * swipeDisplacement; }
        if (position == Position.Right) { targetPosition += Vector3.right * swipeDisplacement; }

        PlayRandomDriftSound();
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                swipeSpeed
            );
            transform.rotation = startRotation;

            float scaleOffset = Mathf.Abs(velocity.x / 150f);
            transform.localScale = new Vector3(1f + scaleOffset, 1f - scaleOffset, 1f + scaleOffset);

            yield return null;
        }

        transform.position = targetPosition;
    }

    void PlayRandomDriftSound()
    {
        AudioManager.Instance.Play(driftSFXs[Random.Range(0,driftSFXs.Length)], 0.15f);
    }

    void OnCollisionEnter(Collision collision)
    {
        int bricksLayer = LayerMask.NameToLayer("Bricks");
        if (collision.gameObject.layer == bricksLayer) {
            if (GameManager.isGameOver) return;
            rb.AddForce(Vector3.up * CollisionUpForce);
            rb.AddTorque(Vector3.down * CollisionTorqueForce);
            AudioManager.Instance.Play(crashSFX);
            if (crashVFX)
            {
                ParticleSystem explosionVFXInstance = Instantiate(crashVFX, transform.position, Quaternion.identity, MemoryManager.Instance.transform);
                explosionVFXInstance.gameObject.transform.LookAt(GameManager.mainCamera.transform);
            }
            GameManager.Instance.GameOver();
        }
    }

    void HandleGameStart()
    {
        anim.SetTrigger("Move");
    }

    void HandleGameOver()
    {
        anim.SetTrigger("Crash");
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        position = Position.Center;
    }

    void HandleGameRestart()
    {
        position = Position.Center;
        anim.SetTrigger("Move");
    }

    void HandleSecondChance()
    {
        position = Position.Center;
        anim.SetTrigger("Move");
    }

    void HandleReload()
    {
        position = Position.Center;
    }
}