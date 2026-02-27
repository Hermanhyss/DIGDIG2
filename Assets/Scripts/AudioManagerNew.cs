using UnityEngine;
using System.Collections.Generic;

public class AudioManagerNew : MonoBehaviour
{
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] List<AudioClip> audioClips;
   
    public void PlaySound(int clipNumber)
    {
        audioSources[0].clip = audioClips[clipNumber];
        audioSources[0].Play();
    }

    // Example usage:
    // Lägg under en annan script där du vill spela ljudet, t.ex. i en PlayerController eller GameManager script, och anropa PlaySound metoden med indexet för det ljudklipp du vill spela.
    // FindAnyObjectByType<AudioManagerNew>().PlaySound(0); // Plays the first clip in the list (change number to play different clips)
}
