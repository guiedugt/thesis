using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SelectableCar
{
    public int id;
    public GameObject prefab;
    public float cost;
    public bool isUnlocked;
}

public class CarSelector : MonoBehaviour
{
    [SerializeField] CarSelectButton carSelectButton;
    [SerializeField] SelectableCar[] selectableCars;

    SelectableCar selectedCar;
    GameObject selectedCarPrefab;

    void Start()
    {
        SelectableCar[] selectableCarsFromPrefs = PlayerPrefsManager.GetSelectableCars();
        int selectedCarId = PlayerPrefsManager.GetSelectedCarId();
        selectedCar = selectableCarsFromPrefs?.Single(t => t.id == selectedCarId) ?? selectableCars[0];
        ShowcaseCar(selectedCar);
    }

    public void ShowcaseCar(int id)
    {
        SelectableCar carToShowcase = selectableCars.Single(t => t.id == id);
        ShowcaseCar(carToShowcase);
    }

    public void ShowcaseCar(SelectableCar selectableCar)
    {
        selectedCar = selectableCar;
        if (selectedCarPrefab != null) Destroy(selectedCarPrefab);
        selectedCarPrefab = Instantiate(selectableCar.prefab, transform.position, transform.rotation, transform);
        carSelectButton.UpdateButton(selectedCar);
    }

    public void SelectCurrentShowcaseCar()
    {
        PlayerPrefsManager.SetSelectedCarId(selectedCar.id);
    }

    public bool IsSelectedCarUnlocked()
    {
        return selectedCar.isUnlocked;
    }
}