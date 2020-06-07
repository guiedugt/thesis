using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Reloadable))]
public class Obstacle : MonoBehaviour
{
    public float speed = 10f;
    public float zigZagSpeed = 1.5f;
    public float zigZagDistance = 3f;
    [SerializeField] GameObject scatters;
    [SerializeField] float scattersFadeDelay = 3f;

    RaycastHit hit;
    LayerMask ground;
    Vector3 initialPosition;
    Reloadable reloadable;

    void Start()
    {
        ground = LayerMask.GetMask("Ground");
        initialPosition = transform.position;
        reloadable = GetComponent<Reloadable>();
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
        Vector3 oldPosition = transform.position;
        transform.position = new Vector3(
            initialPosition.x + offset,
            transform.position.y,
            transform.position.z
        );

        scatters.transform.position = new Vector3(
            initialPosition.x,
            transform.position.y,
            transform.position.z
        );
    }

    public void AddToScatters(GameObject scatteredBrick)
    {
        scatteredBrick.transform.SetParent(scatters.transform);
        foreach (Transform child in scatteredBrick.transform)
            StartCoroutine(FadeScatterCoroutine(child.gameObject));
    }

    IEnumerator FadeScatterCoroutine(GameObject scatter)
    {
        Material mat = scatter.GetComponent<MeshRenderer>()?.material;
        if (mat == null) yield break;

        float a = 1f;
        while (mat.color.a > 0f)
        {
            Color color = mat.color;
            color.a = a;
            mat.color = color;
            a -= Time.deltaTime / scattersFadeDelay;
            yield return null;
        }

        Destroy(scatter);
    }   
}