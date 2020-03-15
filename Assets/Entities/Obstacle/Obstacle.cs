using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 10f;
    public float zigZagSpeed = 1.5f;
    public float zigZagDistance = 3f;

    RaycastHit hit;
    LayerMask ground;
    Vector3 initialPosition;

    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        initialPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.isGameOver) { return; }
        Ground();
        MoveForward();
        ZigZag();
    }

    void Ground()
    {
        bool hitGround = Physics.Raycast(
            transform.position,
            transform.TransformDirection(Vector3.down),
            out hit,
            Mathf.Infinity,
            ground
        );

        if (hitGround)
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }

    void MoveForward()
    {
        Vector3 offset = transform.forward * Time.deltaTime * speed;
        transform.Translate(offset, Space.World);
    }

    void ZigZag()
    {
        float offset = Mathf.Sin(Mathf.PI * Time.time * zigZagSpeed) * zigZagDistance;
        transform.position = new Vector3(
            initialPosition.x + offset,
            transform.position.y,
            transform.position.z
        );
    }
}