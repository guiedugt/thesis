using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, mainCamera.transform.position, volume);
    }
}