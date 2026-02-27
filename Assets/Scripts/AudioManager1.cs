using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundId
{
    BackgroundMusic,
    GameOver,

    Attack1,
    Attack2,
    Attack3,

    EnemyTakeDamage,
    PlayerTakeDamage,

    Buttons,
    EnemyAttack,

    SpaceStationBeep,
    ElectricityCable
}

public enum SoundBus
{
    Music,
    SFX,
    UI,
}

[Serializable]
public class SoundDef
{
    public SoundId id;
    public SoundBus bus = SoundBus.SFX;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;

    public bool loop = false;

    [Tooltip("If true, pitch is randomized around Pitch value.")]
    public bool randomizePitch = false;

    [Tooltip("Random pitch range around Pitch (e.g. 0.1 means +-0.1).")]
    [Range(0f, 0.5f)] public float pitchJitter = 0.1f;
}

public class AudioManager1 : MonoBehaviour
{
    public static AudioManager1 I { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    [Header("Sounds Library")]
    [SerializeField] private List<SoundDef> sounds = new();

    private Dictionary<SoundId, SoundDef> _map;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        _map = new Dictionary<SoundId, SoundDef>();
        foreach (var s in sounds)
        {
            if (s == null) continue;
            _map[s.id] = s;
        }
    }

    public float GetSavedVolume(string exposedParam, float defaultValue = 1f)
    {
        return PlayerPrefs.GetFloat("vol_" + exposedParam, defaultValue);
    }

    public AudioSource src;

    void Update()
    {
        if (src == null) return;

        Debug.Log($"isPlaying={src.isPlaying}, time={src.time:0.00}, clip={(src.clip ? src.clip.name : "NULL")}");
    }

    //detta är för att debugga varför ljudet inte funkar för tillfället
public void Play(SoundId id)
    {
        if (_map == null)
        {
            Debug.LogError("Sound map is null (did Awake run?)");
            return;
        }

        if (!_map.TryGetValue(id, out var s))
        {
            Debug.LogError($"SoundId not found in library: {id}");
            return;
        }

        if (s.clip == null)
        {
            Debug.LogError($"No clip assigned for: {id}");
            return;
        }

        var src = GetSourceForBus(s.bus);
        if (src == null)
        {
            Debug.LogError($"No AudioSource assigned for bus: {s.bus}");
            return;
        }

        Debug.Log($"Playing {id} on bus {s.bus} clip={s.clip.name} vol={s.volume} pitch={s.pitch} loop={s.loop}");

        
    }

    private void Start()
    {
        Play(SoundId.BackgroundMusic);
    }

    public void StopBus(SoundBus bus)
    {
        var src = GetSourceForBus(bus);
        if (src == null) return;

        src.Stop();
            src.clip = null;
    }


    public void PlayBackgroundMusic() => Play(SoundId.BackgroundMusic);
    public void PlayGameOver() => Play(SoundId.GameOver);

    public void PlayAttackRandom()
    {
        
        var choices = new List<SoundId>();
        if (HasClip(SoundId.Attack1)) choices.Add(SoundId.Attack1);
        if (HasClip(SoundId.Attack2)) choices.Add(SoundId.Attack2);
        if (HasClip(SoundId.Attack3)) choices.Add(SoundId.Attack3);

        if (choices.Count == 0) return;
        Play(choices[UnityEngine.Random.Range(0, choices.Count)]);
    }



    public void PlayButton() => Play(SoundId.Buttons);

 

    private const string MASTER = "MasterVol";
    private const string MUSIC = "MusicVol";
    private const string SFX = "SFXVol";
    private const string UI = "UIVol";

    public void SetMasterVolume(float v) => SetMix(MASTER, v);
    public void SetMusicVolume(float v) => SetMix(MUSIC, v);
    public void SetSFXVolume(float v) => SetMix(SFX, v);
    public void SetUIVolume(float v) => SetMix(UI, v);
  

    private void SetMix(string param, float sliderValue)
    {
        if (mixer == null) return;
        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        mixer.SetFloat(param, Mathf.Log10(sliderValue) * 20f);
    }

    private AudioSource GetSourceForBus(SoundBus bus)
    {
        return bus switch
        {
            SoundBus.Music => musicSource,
            SoundBus.UI => uiSource != null ? uiSource : sfxSource,
            _ => sfxSource
        };
    }

    private float GetPitch(SoundDef s)
    {
        if (!s.randomizePitch) return s.pitch;
        float jitter = UnityEngine.Random.Range(-s.pitchJitter, s.pitchJitter);
        return Mathf.Clamp(s.pitch + jitter, 0.5f, 2f);
    }

    private bool HasClip(SoundId id)
    {
        return _map.TryGetValue(id, out var s) && s.clip != null;
    }
}