using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionForce = 2000f;
    [SerializeField] float explosionRadius = 3f;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 2f);
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

    void Explode(int layerMask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << layerMask);
        Rigidbody[] rigidBodies = hitColliders.Select(t => t.gameObject.GetComponent<Rigidbody>()).ToArray();
        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        Destroy(gameObject);
    }
}