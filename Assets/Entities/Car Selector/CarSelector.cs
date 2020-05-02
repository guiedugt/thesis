using System;
using UnityEngine;

[Serializable]
public struct SelectableCar
{
    [SerializeField] GameObject prefab;
    [SerializeField] float cost;
    [SerializeField] bool isUnlocked;
}

public class CarSelector : MonoBehaviour
{
    [SerializeField] SelectableCar[] selectables;
    [SerializeField] GameObject[] showcaseCarPrefabs;

    int carIndex;
    GameObject showcasedCar;

    void Start()
    {
        selectables = PlayerPrefsManager.GetSelectableCars();
        carIndex = PlayerPrefsManager.GetSelectedCarIndex();
        ShowCaseCar(carIndex);
    }

    public void ShowCaseCar(int carToShowcaseIndex)
    {
        carIndex = carToShowcaseIndex;
        if (showcasedCar != null) Destroy(showcasedCar);
        showcasedCar = Instantiate(showcaseCarPrefabs[carIndex], transform.position, transform.rotation, transform);
    }

    public void SelectCurrentShowcaseCar()
    {
        PlayerPrefsManager.SetSelectedCarIndex(carIndex);
    }
}