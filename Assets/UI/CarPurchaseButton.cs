using TMPro;
using UnityEngine;
 
public class CarPurchaseButton : MonoBehaviour
{
    [SerializeField] GameObject alertInstance;
    [SerializeField] TextMeshProUGUI amountText;

    Alert alert;
    
    void Start()
    {
        alert = alertInstance.GetComponent<Alert>();
        alertInstance.SetActive(false);
    }

    public void UpdateAmount(float amount)
    {
        amountText.text = amount.ToString("#,##0");
    }

    public void OpenConfirmationModal()
    {
        alertInstance.SetActive(true);
        alert.Show("Confirm Purchase?");
        alert.OnCancel.AddListener(() => {
            alertInstance.SetActive(false);
        });
    }
}