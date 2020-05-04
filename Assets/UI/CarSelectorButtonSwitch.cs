using UnityEngine;

public class CarSelectorButtonSwitch : MonoBehaviour
{
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject purchaseButton;

    CarPurchaseButton purchaseButtonComponent;

    void Start()
    {
        purchaseButtonComponent = purchaseButton.GetComponent<CarPurchaseButton>();
    }

    public void Switch(bool isUnlocked, float cost = 0f)
    {
        selectButton.SetActive(isUnlocked);
        purchaseButton.SetActive(!isUnlocked);
        if (!isUnlocked) purchaseButtonComponent.UpdateAmount(cost);
    }

    public void Switch(SelectableCar selectedCar)
    {
        selectButton.SetActive(selectedCar.isUnlocked);
        purchaseButton.SetActive(!selectedCar.isUnlocked);
        if (!selectedCar.isUnlocked) purchaseButtonComponent.UpdateAmount(selectedCar.cost);
    }
}