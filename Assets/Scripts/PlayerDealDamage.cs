using Enemies;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDealDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Collider playerweaponCollider;

/*    [Header("Audio")]
    public AudioSource hitAudioSource;
    public AudioClip hitClip;
    public AudioSource missAudioSource;
    public AudioClip missClip; */

    private bool hitThisSwing = false;
    private HashSet<Enemy> enemiesHitThisSwing = new HashSet<Enemy>();

    // Cached reference
    private AudioManagerNew audioManager;

    private void Start()
    {
        if (playerweaponCollider != null)
            playerweaponCollider.enabled = false;
        else
            Debug.LogWarning("Player weapon collider is not assigned!");

        // Cache AudioManager reference
        audioManager = FindAnyObjectByType<AudioManagerNew>();
        if (audioManager == null)
            Debug.LogWarning("AudioManagerNew not found in scene!");

        //if (hitAudioSource == null)
        //    Debug.LogWarning("Hit AudioSource is not assigned!");
        //if (missAudioSource == null)
            Debug.LogWarning("Miss AudioSource is not assigned!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerweaponCollider != null && playerweaponCollider.enabled)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Check if we already hit this enemy during this swing
                if (!enemiesHitThisSwing.Contains(enemy))
                {
                    enemiesHitThisSwing.Add(enemy);

                    Debug.Log("Enemy take damage");
                    enemy.EnemyTakeDamage(damage);

                    // Play hit sound (only once per enemy per swing)
                    if (audioManager != null)
                        audioManager.PlaySound(3);

                    hitThisSwing = true;
                }
            }
        }
    }

    public void EnableWeaponCollider()
    {
        if (playerweaponCollider != null)
        {
            playerweaponCollider.enabled = true;
            hitThisSwing = false;
            enemiesHitThisSwing.Clear(); 

            // Play weapon swing sound once per attack
            if (audioManager != null)
                audioManager.PlaySound(4);

            Debug.Log("Weapon collider enabled");
        }
        else
        {
            Debug.LogWarning("Player weapon collider is not assigned!");
        }
    }

    public void DisableWeaponCollider()
    {
        if (playerweaponCollider != null)
        {
            playerweaponCollider.enabled = false;
            if (!hitThisSwing)
            {
                Debug.Log("Missed attack");
                //PlayMissSound();
            }
            Debug.Log("Weapon collider disabled");
        }
        else
        {
            Debug.LogWarning("Player weapon collider is not assigned!");
        }
    }

    //public void PlayHitSound()
    //{
    //    if (hitAudioSource != null && hitClip != null)
    //        hitAudioSource.PlayOneShot(hitClip);
    //    else
    //        Debug.LogWarning("Hit audio source or clip not assigned!");
    //}

    //public void PlayMissSound()
    //{
    //    if (missAudioSource != null && missClip != null)
    //        missAudioSource.PlayOneShot(missClip);
    //}
}

