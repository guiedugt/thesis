using UnityEngine;

public class Brick : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}