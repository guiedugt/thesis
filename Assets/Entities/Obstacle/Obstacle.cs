using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    RaycastHit hit;
    LayerMask ground;

    void Start()
    {
        ground = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        Ground();
        Move();
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

    void Move()
    {
        Vector3 offset = transform.forward * Time.deltaTime * speed;
        transform.Translate(offset, Space.World);
    }
}