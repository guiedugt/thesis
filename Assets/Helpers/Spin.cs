using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField][Range(0f, 100f)] float speed = 30f;

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }
}
