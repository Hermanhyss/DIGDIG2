using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private int damage;

    Collider weaponCollider;
    private void Start()
    {
        weaponCollider = GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
            enemy.TakeDamage(damage);

    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
