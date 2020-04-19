using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    public void SpawnCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnCoin();
        }
    }

    public void SpawnCoin()
    {
        Vector3 position = new Vector3(
            Random.Range(col.bounds.min.x, col.bounds.max.x),
            Random.Range(col.bounds.min.y, col.bounds.max.y),
            Random.Range(col.bounds.min.z, col.bounds.max.z)
        );

        Instantiate(coinPrefab, position, Random.rotation);
    }
}
