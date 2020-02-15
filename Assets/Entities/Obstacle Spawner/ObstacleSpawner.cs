using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float delay = 3f;

    WaitForSeconds wait;

    void Start()
    {
        wait = new WaitForSeconds(delay);
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine() {
        yield return wait;
        Instantiate(prefab, transform.position, transform.rotation, MemoryManager.Instance.transform);
        yield return SpawnCoroutine();
    }
}