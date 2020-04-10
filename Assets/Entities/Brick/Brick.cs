using UnityEngine;

public class Brick : MonoBehaviour
{
  public static float pointsPerBrick = 2f;

  [SerializeField] float CollisionUpForce = 200f;
  [SerializeField] float CollisionTorqueForce = 300f;

  void OnCollisionEnter(Collision collision)
  {
    bool isPlayer = collision.gameObject.CompareTag("Player");
    if (isPlayer) { HandlePlayerCollision(collision.gameObject); return; }
  }

  void HandlePlayerCollision(GameObject player)
  {
    if (GameManager.isGameOver) { return; }
    Rigidbody rb = player.GetComponent<Rigidbody>();
    rb.AddForce(Vector3.up * CollisionUpForce);
    rb.AddTorque(Vector3.down * CollisionTorqueForce);
    GameManager.Instance.GameOver();
  }
}