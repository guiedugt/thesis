using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Collider ground;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        bool hitSomething = Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hitSomething) {
            Debug.Log(hit.ToString());
        } else {
            Debug.Log("Not hitting anything");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
