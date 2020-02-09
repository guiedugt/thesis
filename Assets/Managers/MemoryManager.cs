using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MemoryManager : Singleton<MemoryManager>
{
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}