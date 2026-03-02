using UnityEngine;

public class WeaponColliderRelay : MonoBehaviour
{
    public PlayerDealDamage playerDealDamage;

    public void EnableWeaponCollider()
    {
        if (playerDealDamage != null)
            playerDealDamage.EnableWeaponCollider();
    }

    public void DisableWeaponCollider()
    {
        if (playerDealDamage != null)
            playerDealDamage.DisableWeaponCollider();
    }
}