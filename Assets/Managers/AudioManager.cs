using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void Play(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, mainCamera.transform.position);
    }
}