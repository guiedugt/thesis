using UnityEngine;

public class Track : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameOver()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat.SetFloat("_ScrollXSpeed", 0f);
        mat.SetFloat("_ScrollYSpeed", 0f);
    }
}