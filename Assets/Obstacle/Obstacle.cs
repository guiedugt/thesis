using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] float speed = 10f;

    Collider col;
    RaycastHit hit;
    LayerMask ground;

    void Start()
    {
        col = body.GetComponent<Collider>();
        ground = LayerMask.GetMask("Ground");
    }

    void OnValidate()
    {
        if (col == null)
        {
            Debug.LogWarning(transform.name + " must have a body (with a collider)");
        }
    }

    void Update()
    {
        Ground();
        Move();
    }

    void Ground()
    {
        bool hitGround = Physics.Raycast(
            body.transform.position,
            body.transform.TransformDirection(Vector3.down),
            out hit,
            Mathf.Infinity,
            ground
        );

        if (hitGround)
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(body.transform.up, hit.normal) * body.transform.rotation;
        }
        else
        {
            Debug.Log(transform.name + " is not finding its ground");
        }
    }

    void Move()
    {
        Vector3 offset = body.transform.forward * Time.deltaTime * speed;
        transform.Translate(offset, Space.World);
    }
}