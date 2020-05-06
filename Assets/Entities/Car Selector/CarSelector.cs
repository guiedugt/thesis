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
        int selectedCarId = PlayerPrefsManager.GetSelectedCarId();
        selectableCars = PlayerPrefsManager.GetSelectableCars() ?? selectableCarsData.data;
        ShowcaseCar(selectedCarId);
    }

    public void ShowcaseCar(int id)
    {
        selectedCar = selectableCars.Single(t => t.id == id);
        GameObject showcasePrefab = selectableCarsData.data.Single(t => t.id == id).showcasePrefab;
        if (selectedCarPrefab != null) Destroy(selectedCarPrefab);
        selectedCarPrefab = Instantiate(showcasePrefab, transform.position, transform.rotation, transform);
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