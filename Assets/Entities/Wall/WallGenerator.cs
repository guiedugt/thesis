using System;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] int rows = 5;
    [SerializeField] int columns = 10;
    [SerializeField] GameObject brick;

    void OnValidate()
    {
        if (brick == null)
        {
            throw new Exception($"{name} script should have a brick prefab assigned to it");
        }

        if (brick.GetComponent<MeshRenderer>() == null)
        {
            throw new Exception($"{name} script should have a brick prefab with a mesh renderer assigned to it");
        }
    }

    void Start()
    {
        GenerateBricks();
    }

    void GenerateBricks()
    {
        Vector3 size = brick.GetComponent<MeshRenderer>().bounds.size;

        // For each row
        for (int row = 0; row < rows; row++)
        {
            // of each column
            for (int column = 0; column < columns; column++)
            {
                // create brick
                Vector3 localPosition = new Vector3(
                    size.x * column,
                    size.y * row,
                    transform.position.z
                );
                Vector3 worldPosition = localPosition + transform.position;
                Instantiate(brick, worldPosition, Quaternion.identity, transform);
            }
        }
    }
}