using UnityEngine;

[RequireComponent(typeof(Reloadable))]
public class Obstacle : MonoBehaviour
{
    public float speed = 10f;
    public float zigZagSpeed = 1.5f;
    public float zigZagDistance = 3f;

    [SerializeField] GameObject prefab;

    RaycastHit hit;
    LayerMask ground;
    Vector3 initialPosition;
    Reloadable reloadable;

    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        initialPosition = transform.position;
        reloadable = GetComponent<Reloadable>();

        reloadable.OnReload.AddListener(HandleReload);
    }

    void Update()
    {
        if (!GameManager.isGameRunning) { return; }
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

    void HandleReload()
    {
        Instantiate(prefab, reloadable.initialPosition, reloadable.initialRotation, transform.parent);
        Destroy(gameObject);
    }
}