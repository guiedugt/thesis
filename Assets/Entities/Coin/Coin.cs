using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    [SerializeField] float jumpForce = 1500f;
    [SerializeField] float rotationForce = 2500f;
    [SerializeField] float gravityModifier = 75f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * jumpForce);

        rb.AddTorque(new Vector3(
            Random.Range(0f, rotationForce),
            Random.Range(0f, rotationForce),
            Random.Range(0f, rotationForce)
        ));
    }
    
    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravityModifier);
    }
}