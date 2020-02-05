using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField][Range(1f, 10f)] float throwPower = 7f;
    [SerializeField][Range(1f, 20f)] float touchCameraDistance = 5f;

    new Camera camera;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        HandleBombThrow();
    }

    void HandleBombThrow()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 clickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, touchCameraDistance);
        Vector3 bombPosition = camera.ScreenToWorldPoint(clickPosition);

        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

        Vector3 throwDirection = bombPosition - camera.transform.position;
        Vector3 bombTorque = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0f, 360f));

        bombRigidbody.velocity = throwDirection * throwPower;
        bombRigidbody.AddTorque(bombTorque);
    }
}