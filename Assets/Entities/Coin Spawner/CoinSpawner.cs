using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int maxSpawnAmount = 10;
    [SerializeField] AudioClip coinsSFX;
    [SerializeField] float parentToMemoryManagerDelayInSeconds = 5f;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        LevelManager.Instance.OnLevelUp.AddListener(HandleLevelUp);
    }

    void HandleLevelUp()
    {
        int spawnAmount = LevelManager.level - 1;
        int clampedSpawnAmount = Mathf.Clamp(spawnAmount, 1, maxSpawnAmount);
        SpawnCoins(clampedSpawnAmount);
    }

    public void SpawnCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(col.bounds.min.x, col.bounds.max.x),
                Random.Range(col.bounds.min.y, col.bounds.max.y),
                Random.Range(col.bounds.min.z, col.bounds.max.z)
            );

            GameObject coin = Instantiate(coinPrefab, position, Random.rotation);
            ParentToMemoryManagerAfterDelay(coin);
        }

        AudioManager.Instance.Play(coinsSFX);
    }

    void ParentToMemoryManagerAfterDelay(GameObject coin)
    {
        StartCoroutine(ParentToMemoryManagerAfterDelayCoroutine(coin));
    }

    IEnumerator ParentToMemoryManagerAfterDelayCoroutine(GameObject coin)
    {
        yield return new WaitForSeconds(parentToMemoryManagerDelayInSeconds);
        coin.transform.SetParent(MemoryManager.Instance.transform);
    }
}