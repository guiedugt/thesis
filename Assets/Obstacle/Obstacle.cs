using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Collider body;
    RaycastHit hit;

    void OnValidate()
    {
        if (body == null)
        {
            Debug.LogWarning(transform.name + " must have a body (with a collider)");
        }
    }

    void Update()
    {
        Ground();
    }

    void Ground()
    {
        bool hitGround = Physics.Raycast(
            body.transform.position,
            body.transform.TransformDirection(Vector3.down),
            out hit
        );

        if (hitGround)
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(body.transform.up, hit.normal) * body.transform.rotation;
        }
        else
        {
            Debug.Log(transform.name + " is not finding its ground");
        }
    }
}