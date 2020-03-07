using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] GameObject cannonBase;

    void Start()
    {
        InputManager.OnBombThrow.AddListener(OnBombThrow);
    }

    void OnBombThrow(GameObject bomb, Vector3 clickPosition)
    {
        var go = new GameObject();
        go.transform.position = clickPosition;
        cannonBase.transform.LookAt(clickPosition);
    }
}