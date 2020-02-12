using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float zigZagSpeed = 10f;
    [SerializeField] float zigZagDistance = 10f;

    RaycastHit hit;
    LayerMask ground;

    void Start()
    {
        ground = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        Ground();
        // MoveForward();
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
        else
        {
            Debug.Log(transform.name + " is not finding its ground");
        }
    }

    void MoveForward()
    {
        Vector3 offset = transform.forward * Time.deltaTime * speed;
        transform.Translate(offset, Space.World);
    }

    void ZigZag()
    {
        transform.position += Vector3.right * Mathf.Sin(Time.time * zigZagSpeed) * zigZagDistance;
    }
}