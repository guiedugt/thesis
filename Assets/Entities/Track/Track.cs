using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Track : MonoBehaviour
{
    [SerializeField][Range(0f, 10f)] float scrollXSpeed = 0f;
    [SerializeField][Range(0f, 10f)] float scrollYSpeed = 3f;

    Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        GameManager.Instance.OnGameStart.AddListener(HandleGameStart);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    void HandleGameStart()
    {
        material.SetFloat("_ScrollXSpeed", scrollXSpeed);
        material.SetFloat("_ScrollYSpeed", scrollYSpeed);
    }

    void HandleGameOver()
    {
        material.SetFloat("_ScrollXSpeed", 0f);
        material.SetFloat("_ScrollYSpeed", 0f);
    }
}