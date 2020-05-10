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

    Position position = Position.Center;
    Vector3 velocity;
    Vector3 startPosition;
    Animator anim;
    Coroutine moveCoroutine;

    void Start()
    {
        startPosition = transform.position;
        anim = GetComponent<Animator>();
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
            yield return null;
        }

        transform.position = targetPosition;
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