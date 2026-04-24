using Enemies;
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

    private void Start()
    {
        if (playerweaponCollider != null)
            playerweaponCollider.enabled = false;
        else
            Debug.LogWarning("Player weapon collider is not assigned!");

        //if (hitAudioSource == null)
        //    Debug.LogWarning("Hit AudioSource is not assigned!");
        //if (missAudioSource == null)
            Debug.LogWarning("Miss AudioSource is not assigned!");
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (playerweaponCollider != null && playerweaponCollider.enabled)
        {
            FindAnyObjectByType<AudioManagerNew>().PlaySound(4);
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Enemy take damage");
                enemy.EnemyTakeDamage(damage);
                FindAnyObjectByType<AudioManagerNew>().PlaySound(3);
                hitThisSwing = true;
            }
        }
    }

    public void EnableWeaponCollider()
    {
        if (playerweaponCollider != null)
        {
            playerweaponCollider.enabled = true;
            hitThisSwing = false;
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

