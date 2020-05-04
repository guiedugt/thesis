using System.Linq;
using UnityEngine;
 
public class CarSpawner : MonoBehaviour
{
    [SerializeField] SelectableCarsData selectableCarsData;
    SelectableCar[] selectableCars;

    void Awake()
    {
        SelectableCar[] selectableCarsFromPrefs = PlayerPrefsManager.GetSelectableCars();
        selectableCars = selectableCarsFromPrefs ?? selectableCarsData.data;

        int selectedCarId = PlayerPrefsManager.GetSelectedCarId();
        SelectableCar selectedCar = selectableCars.Single(t => t.id == selectedCarId);
        Instantiate(selectedCar.prefab, transform.position, transform.rotation, transform);
    }
}