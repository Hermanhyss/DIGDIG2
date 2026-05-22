using UnityEngine;
using System.Collections.Generic;

public class AudioManagerNew : MonoBehaviour
{
    [SerializeField] List<AudioSource> MusicSources;
    [SerializeField] List<AudioSource> SFXSources;
    [SerializeField] List<AudioSource> UISources;
    
    [SerializeField] List<AudioClip> musicClips;
    [SerializeField] List<AudioClip> SFXClips;
    [SerializeField] List<AudioClip> UIClips;

    public void PlayMusic(int clipNumber)
    {
        PlaySoundFromPool(MusicSources, musicClips[clipNumber]);

    }

    

    public void PlaySFX(int clipNumber)
    {
        PlaySoundFromPool(SFXSources, SFXClips[clipNumber]);

    }

    public void PlayUI(int clipNumber)
    {
        PlaySoundFromPool(UISources, UIClips[clipNumber]);

    }

    private void PlaySoundFromPool(List<AudioSource> audioSources, AudioClip clip) // väljer en random AudioSource från listan och spelar det valda klippet, om den inte redan spelar något ljud. Om alla AudioSources i poolen är upptagna, kommer inget ljud att spelas.
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();

                return; 
            }
        }
    }

    // Example usage:
    // Lägg under en annan script där du vill spela ljudet, t.ex. i en PlayerController eller GameManager script, och anropa PlaySound metoden med indexet för det ljudklipp du vill spela.
    // FindAnyObjectByType<AudioManagerNew>().PlaySound(0); // Plays the first clip in the list (change number to play different clips)
}
