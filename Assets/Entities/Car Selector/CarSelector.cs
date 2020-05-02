using UnityEngine;

public class CarSelector : MonoBehaviour
{
    [SerializeField] GameObject[] showcaseCarPrefabs;

    int carIndex;
    GameObject showcasedCar;

    void Start()
    {
        if (showcaseCarPrefabs == null || showcaseCarPrefabs.Length <= 0) return;
        carIndex = PlayerPrefsManager.GetSelectedCarIndex();
        ShowCaseCar(carIndex);
    }

    public void ShowCaseCar(int i)
    {
        if (showcasedCar != null) Destroy(showcasedCar);
        showcasedCar = Instantiate(showcaseCarPrefabs[i], transform.position, transform.rotation, transform);
    }

    public void SelectCurrentShowcaseCar()
    {
        PlayerPrefsManager.SetSelectedCarIndex(carIndex);
    }
}