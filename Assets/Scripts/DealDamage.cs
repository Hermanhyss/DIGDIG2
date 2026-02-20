using Enemies;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    Collider weaponCollider;
    private void Start()
    {
        weaponCollider = GetComponentInChildren<Collider>();
    }

    // THIS GIVES A ERROR BECAUSE ENEMY DOESNT HAVE TAKEDAMAGE FUNCTION - ITS NOT MY FAULT BRUH
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (weaponCollider != null) { 
    //        if (other.gameObject.CompareTag("Enemy"))
    //        {
    //            var enemy = other.GetComponent<Enemy>();
    //            if (enemy != null)
    //            {
    //                enemy.TakeDamage(damage);
    //            }
    //        }
    //    }
        
    //}

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
