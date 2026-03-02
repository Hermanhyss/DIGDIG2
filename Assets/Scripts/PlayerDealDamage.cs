using Enemies;
using UnityEngine;

public class PlayerDealDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Collider playerweaponCollider; 

    private void Start()
    {
        if (playerweaponCollider != null)
            playerweaponCollider.enabled = false;
        else
            Debug.LogWarning("Player weapon collider is not assigned!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerweaponCollider != null && playerweaponCollider.enabled)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Enemy take damage");
                enemy.EnemyTakeDamage(damage);
            }
        }
    }

    
    public void EnableWeaponCollider()
    {
        if (playerweaponCollider != null)
        {
            playerweaponCollider.enabled = true;
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
            Debug.Log("Weapon collider disabled");
        }
        else
        {
            Debug.LogWarning("Player weapon collider is not assigned!");
        }
    }
}

