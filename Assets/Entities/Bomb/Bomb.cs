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
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Gold") && !InputManager.Instance.isSuperBombActive) continue;
            Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();

            collider.isTrigger = true;
            collider.gameObject.transform.parent = MemoryManager.Instance.transform;

            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        ScoreItem scoreItem = new ScoreItem(colliders.Length, scorePerBrick * colliders.Length);
        ScoreManager.Instance.AddScore(ScoreType.Brick, scoreItem);

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