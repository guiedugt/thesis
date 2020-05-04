using System;
using UnityEngine;

[Serializable]
public struct SelectableCar
{
    public int id;
    public GameObject prefab;
    public GameObject showcasePrefab;
    public float cost;
    public bool isUnlocked;
}

[CreateAssetMenu(fileName = "Selectable Cars", menuName = "SelectableCarsData", order = 1)]
public class SelectableCarsData : ScriptableObject
{
    public SelectableCar[] data;
}