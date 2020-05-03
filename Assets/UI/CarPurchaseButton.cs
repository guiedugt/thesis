using UnityEngine;
 
public class CarPurchaseButton : MonoBehaviour
{
    [SerializeField] GameObject alertInstance;
    Alert alert;
    
    void Start()
    {
        alert = alertInstance.GetComponent<Alert>();
        alertInstance.SetActive(false);
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