using UnityEngine;
using UnityEngine.Audio;

public class AudioManager1 : MonoBehaviour
{
    public static AudioManager1 I { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;


    private const string MASTER = "MasterVol";
    private const string MUSIC = "MusicVol";
    private const string SFX = "SFXVol";
    private const string UI = "UIVol";

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        ApplySavedVolume(MASTER, 1f);
        ApplySavedVolume(MUSIC, 1f);
        ApplySavedVolume(SFX, 1f);
        ApplySavedVolume(UI, 1f);
    }


    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
        musicSource.clip = null;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    public void PlayUI(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;


        AudioSource src = uiSource != null ? uiSource : sfxSource;
        if (src == null) return;

        src.pitch = pitch;
        src.PlayOneShot(clip, Mathf.Clamp01(volume));
    }


    public void SetMasterVolume(float sliderValue) => SetVolume(MASTER, sliderValue);
    public void SetMusicVolume(float sliderValue) => SetVolume(MUSIC, sliderValue);
    public void SetSFXVolume(float sliderValue) => SetVolume(SFX, sliderValue);
    public void SetUIVolume(float sliderValue) => SetVolume(UI, sliderValue);

    public float GetSavedVolume(string exposedParam, float defaultValue = 1f)
    {
        return PlayerPrefs.GetFloat(Key(exposedParam), defaultValue);
    }

    private void ApplySavedVolume(string exposedParam, float defaultValue)
    {
        float v = GetSavedVolume(exposedParam, defaultValue);
        SetVolume(exposedParam, v, save: false);
    }

    private void SetVolume(string exposedParam, float sliderValue, bool save = true)
    {
        if (mixer == null) return;


        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);

        float db = Mathf.Log10(sliderValue) * 20f;
        mixer.SetFloat(exposedParam, db);

        if (save)
        {
            PlayerPrefs.SetFloat(Key(exposedParam), sliderValue);
            PlayerPrefs.Save();
        }
    }

    private static string Key(string exposedParam) => $"vol_{exposedParam}";
}
