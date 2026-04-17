using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    [SerializeField] private GameObject enemyFlyingPrefab;
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyFlyingPrefab != null)
            {
                Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
                Instantiate(enemyFlyingPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("FlyingEnemy prefab not assigned in EnemyRange.");
            }
        }
    }
}

// Should only spwans enemy when animtion state "projectile go" has fully play or has a animation event calling out 
// need to fix how many flyingenemy can spwans before the EnemyRange die and gone
// also need to fix a amonut of range I can control in inspector and make sure the player is in that range before animation can start playing
// also need to make sure the animation can only play once and not keep playing when the player is in range
