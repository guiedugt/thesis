using UnityEngine;
 
public class DebugManager : Singleton<DebugManager>
{
    [SerializeField] bool clearPlayerPrefs;

    void Awake()
    {
        if (clearPlayerPrefs) PlayerPrefsManager.Clear();
    }
}