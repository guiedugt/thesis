using System.Linq;
using UnityEngine;

public class CarSelector : MonoBehaviour
{
    [SerializeField] SelectableCarsData selectableCarsData;
    [SerializeField] CarSelectorButtonSwitch carSelectorButtonSwitch;
    [HideInInspector] public SelectableCar selectedCar;

    GameObject selectedCarPrefab;
    SelectableCar[] selectableCars;

    void Start()
    {
        selectableCars = selectableCarsData.data;
        SelectableCar[] selectableCarsFromPrefs = PlayerPrefsManager.GetSelectableCars();
        int selectedCarId = PlayerPrefsManager.GetSelectedCarId();
        if (selectableCarsFromPrefs != null) selectableCars = selectableCarsFromPrefs;
        selectedCar = selectableCars.Single(t => t.id == selectedCarId);
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
        selectedCarPrefab = Instantiate(selectableCar.showcasePrefab, transform.position, transform.rotation, transform);
        carSelectorButtonSwitch.Switch(selectedCar);
    }

    public void UnlockSelectedCar()
    {
        SelectableCar[] newSelectableCars = selectableCars.Select(t => {
            if (t.id == selectedCar.id) t.isUnlocked = true;
            return t;
        }).ToArray();

        selectableCars = newSelectableCars;
        PlayerPrefsManager.SetSelectableCars(newSelectableCars);
        PlayerPrefsManager.SubToTotalScore(selectedCar.cost);
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