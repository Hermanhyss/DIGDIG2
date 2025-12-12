using UnityEngine;
using System;

public enum SoundType
{
    Jump,
    Attack,
    Damage,
    Buttons,
    UI,
    BackgroundMusic,
    BackgroundSounds,
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;

    private void Awake()
    {
        // Proper singleton guard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        if (Instance == null || Instance.soundList == null)
            return;

        int index = (int)sound;
        if (index < 0 || index >= Instance.soundList.Length)
            return;

        AudioClip clip = Instance.soundList[index];
        if (clip != null)
        {
            Instance.audioSource.PlayOneShot(clip, volume);
        }
    }
    public void PlayMusic(SoundType sound, float volume = 1f)
    {
        int index = (int)sound;
        AudioClip clip = soundList[index];
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }
    public static void SetVolume(float volume)
    {
        if (Instance != null)
            Instance.audioSource.volume = volume;
    }

}
