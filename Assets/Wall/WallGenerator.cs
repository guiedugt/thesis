using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] GameObject brick;
    [SerializeField] int columns = 10;
    [SerializeField] int rows = 5;

    void OnValidate()
    {
        if (brick == null)
        {
            Debug.LogError($"{name} script should have a brick prefab assigned to it");
            return;
        }

        if (brick.GetComponent<MeshRenderer>() == null)
        {
            Debug.LogError($"{name} script should have a brick prefab with a mesh renderer assigned to it");
        }
    }

    void Start()
    {
        GenerateBricks();
    }

    void GenerateBricks()
    {
        Vector3 size = brick.GetComponent<MeshRenderer>().bounds.size;

        // For each column
        for (int i = 0; i < columns; i++)
        {
        //     // of each row
            for (int j = 0; j < rows; j++)
            {
                // create brick
                Vector3 position = new Vector3(
                    size.x * i,
                    size.y * j,
                    transform.position.z
                );

                Instantiate(brick, position, Quaternion.identity, transform);
            }
        }

    }

}