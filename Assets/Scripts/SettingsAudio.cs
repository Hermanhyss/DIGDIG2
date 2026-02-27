using UnityEngine;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider ui;

    private void Start()
    {
        // Set slider positions from what AudioManager saves
        master.value = AudioManager1.I.GetSavedVolume("MasterVol", 1f);
        music.value = AudioManager1.I.GetSavedVolume("MusicVol", 1f);
        sfx.value = AudioManager1.I.GetSavedVolume("SFXVol", 1f);
        ui.value = AudioManager1.I.GetSavedVolume("UIVol", 1f);

        // Apply them once so mixer matches immediately
        AudioManager1.I.SetMasterVolume(master.value);
        AudioManager1.I.SetMusicVolume(music.value);
        AudioManager1.I.SetSFXVolume(sfx.value);
        AudioManager1.I.SetUIVolume(ui.value);
    }
}