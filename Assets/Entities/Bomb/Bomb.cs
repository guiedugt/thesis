using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionRadius = 7f;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
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
        int ignoredLayerMask = ~ layerMask;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, ignoredLayerMask);
        
        Destroy(gameObject);
    }
}