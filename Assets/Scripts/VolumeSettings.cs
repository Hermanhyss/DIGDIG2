using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider UISlider;
    [SerializeField] private Slider masterSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVol"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
            SetUIVolume();
            SetMasterVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = Mathf.Clamp(masterSlider.value, 0.0001f, 1f);
        myMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("MusicVol", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVol", volume); //sparar inställningar för musiken
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SetUIVolume()
    {
        float volume = UISlider.value;
        myMixer.SetFloat("UIVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("UIVol", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol"); //laddar inställningar för musiken
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
        UISlider.value = PlayerPrefs.GetFloat("UIVol");
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");

        SetMusicVolume(); 
        SetSFXVolume();
        SetUIVolume();
        SetMasterVolume();
    }
}

