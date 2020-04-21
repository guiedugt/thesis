using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionForce = 2000f;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] [Range(-10f, 10f)] float gravityRatio = 0.5f;
    [SerializeField] ParticleSystem explosionVFX;

    new Collider collider;
    new Rigidbody rigidbody;

    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * rigidbody.mass * gravityRatio);
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
            Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();

            collider.isTrigger = true;
            collider.gameObject.transform.parent = MemoryManager.Instance.transform;

            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        ScoreItem scoreItem = new ScoreItem(colliders.Length, Brick.pointsPerBrick * colliders.Length);
        ScoreManager.Instance.AddScore(ScoreType.Brick, scoreItem);

        if (explosionVFX)
        {
            ParticleSystem explosionVFXInstance = Instantiate(explosionVFX, transform.position, Quaternion.identity, MemoryManager.Instance.transform);
            explosionVFXInstance.gameObject.transform.LookAt(GameManager.camera.transform);
        }

        Destroy(gameObject);
    }
}