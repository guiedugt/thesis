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
    [SerializeField][Range(0f, 1f)] float gravityRatio = 0.5f;
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
        rigidbody.AddForce(-Physics.gravity * rigidbody.mass * gravityRatio);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    void OnCollisionEnter(Collision other)
    {
        bool hitBrick = LayerMask.LayerToName(other.gameObject.layer) == Constants.Layers.BRICKS;
        if (!hitBrick) return;

        Explode(other.gameObject.layer);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Explode(int layerMask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << layerMask);
        Rigidbody[] rigidBodies = hitColliders.Select(t => t.gameObject.GetComponent<Rigidbody>()).ToArray();
        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        if (explosionVFX)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}