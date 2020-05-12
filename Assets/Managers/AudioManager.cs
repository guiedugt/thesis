using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    MainCamera mainCamera;

    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
    }

    public void Play(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, mainCamera.transform.position);
    }
}