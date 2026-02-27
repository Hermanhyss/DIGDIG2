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
}
