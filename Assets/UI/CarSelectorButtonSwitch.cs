using UnityEngine;

public class CarSelectorButtonSwitch : MonoBehaviour
{
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject purchaseButton;

    public void Switch(SelectableCar selectedCar)
    {
        selectButton.SetActive(selectedCar.isUnlocked);
        purchaseButton.SetActive(!selectedCar.isUnlocked);
    }
}