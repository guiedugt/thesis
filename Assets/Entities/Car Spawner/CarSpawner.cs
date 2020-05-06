using System.Linq;
using UnityEngine;
 
public class CarSpawner : MonoBehaviour
{
    [SerializeField] SelectableCarsData selectableCarsData;
    SelectableCar[] selectableCars;

    void Awake()
    {
        int selectedCarId = PlayerPrefsManager.GetSelectedCarId();
        SelectableCar selectedCar = selectableCarsData.data.Single(t => t.id == selectedCarId);
        Instantiate(selectedCar.prefab, transform.position, transform.rotation, transform);
    }
}