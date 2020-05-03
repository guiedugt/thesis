using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CarSelectorButton : MonoBehaviour
{
    public int id;
    [SerializeField] CarSelector carSelector;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        carSelector.ShowcaseCar(id);
    }
}