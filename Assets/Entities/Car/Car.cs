using System.Collections;
using UnityEngine;

enum Position
{
    Left,
    Center,
    Right
}

[RequireComponent(typeof(Animator))]
public class Car : MonoBehaviour
{
    [SerializeField] float swipeSpeed = 0.2f;
    [SerializeField] float swipeDisplacement = 3f;
    [SerializeField] float CollisionUpForce = 200f;
    [SerializeField] float CollisionTorqueForce = 300f;

    Position position = Position.Center;
    Vector3 velocity;
    Vector3 startPosition;
    Quaternion startRotation;
    Animator anim;
    Coroutine moveCoroutine;
    Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HandleGameRestart);
        SecondChanceManager.Instance.OnTrigger.AddListener(HandleSecondChance);
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

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                swipeSpeed
            );
            transform.rotation = startRotation;
            yield return null;
        }

        transform.position = targetPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        int bricksLayer = LayerMask.NameToLayer("Bricks");
        if (collision.gameObject.layer == bricksLayer) {
            if (GameManager.isGameOver) return;
            rb.AddForce(Vector3.up * CollisionUpForce);
            rb.AddTorque(Vector3.down * CollisionTorqueForce);
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
}