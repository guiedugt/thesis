using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionForce = 2000f;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float scorePerBrick = 0.2f;
    [SerializeField] [Range(-10f, 10f)] float gravityRatio = 0.5f;
    [SerializeField] ParticleSystem explosionVFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] float screenShakeMagnitude = 0.1f;
    [SerializeField] GameObject shatteredBrickPrefab;
    [SerializeField] GameObject shatteredGoldBrickPrefab;

    Collider col;
    Rigidbody rb;

    void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravityRatio);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    void OnCollisionEnter(Collision other)
    {
        Explode();
    }

    void Explode()
    {
        int bricksLayerMask = LayerMask.GetMask("Bricks");
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, bricksLayerMask);

        Obstacle obstacle = colliders.Length > 0 ? colliders[0].GetComponentInParent<Obstacle>() : null;

        if (obstacle != null)
        {
            foreach (Collider collider in colliders)
            {
                bool isGold = collider.CompareTag("Gold");
                if (isGold && !InputManager.Instance.isSuperBombActive) continue;


                GameObject shatteredPrefab = isGold ? shatteredGoldBrickPrefab : shatteredBrickPrefab;
                GameObject scatteredBrick = Instantiate(shatteredPrefab, collider.transform.position, collider.transform.rotation);
                obstacle.AddToScatters(scatteredBrick);

                Destroy(collider.gameObject);

                foreach (Rigidbody rb in scatteredBrick.GetComponentsInChildren<Rigidbody>())
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            ScoreItem scoreItem = new ScoreItem(colliders.Length, scorePerBrick * colliders.Length);
            ScoreManager.Instance.AddScore(ScoreType.Brick, scoreItem);
        }

        if (explosionVFX)
        {
            ParticleSystem explosionVFXInstance = Instantiate(explosionVFX, transform.position, Quaternion.identity, MemoryManager.Instance.transform);
            explosionVFXInstance.gameObject.transform.LookAt(GameManager.mainCamera.transform);
        }

        GameManager.mainCamera.Shake(screenShakeMagnitude);
        AudioManager.Instance.Play(explosionSFX);
        Destroy(gameObject);
    }
}