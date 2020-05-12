using TMPro;
using UnityEngine;
 
public class CarPurchaseButton : MonoBehaviour
{
    [SerializeField] CarSelector carSelector;
    [SerializeField] CarSelectorButtonSwitch carSelectorButtonSwitch;
    [SerializeField] GameObject purchaseAlertInstance;
    [SerializeField] GameObject messageAlertInstance;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TotalCoins totalCoins;
    [SerializeField] AudioClip purchaseSFX;

    Alert purchaseAlert;
    Alert messageAlert;
    
    void Start()
    {
        purchaseAlert = purchaseAlertInstance.GetComponent<Alert>();
        purchaseAlert.OnConfirm.AddListener(HandlePurchaseConfirm);
        purchaseAlert.OnCancel.AddListener(HandlePurchaseCancel);
        messageAlert = messageAlertInstance.GetComponent<Alert>();
        messageAlert.OnConfirm.AddListener(HandleMessageConfirm);
        purchaseAlertInstance.SetActive(false);
        messageAlertInstance.SetActive(false);
    }

    public void UpdateAmount(float amount)
    {
        amountText.text = amount.ToString("#,##0");
    }

    public void OpenConfirmationModal()
    {
        purchaseAlertInstance.SetActive(true);
        purchaseAlert.Show("Confirm Purchase?");
    }

    void HandlePurchaseConfirm()
    {
        float totalScore = PlayerPrefsManager.GetTotalScore();
        if (totalScore < carSelector.selectedCar.cost) {
            messageAlertInstance.SetActive(true);
            messageAlert.Show("Not enough Coins");
            purchaseAlertInstance.SetActive(false);
            return;
        }
        
        carSelector.UnlockSelectedCar();
        totalCoins.UpdateAmount();
        carSelectorButtonSwitch.Switch(true);
        purchaseAlertInstance.SetActive(false);
        AudioManager.Instance.Play(purchaseSFX);
    }
    
    void HandlePurchaseCancel()
    {
        purchaseAlertInstance.SetActive(false);
    }

    void HandleMessageConfirm()
    {
        messageAlertInstance.SetActive(false);
    }
}