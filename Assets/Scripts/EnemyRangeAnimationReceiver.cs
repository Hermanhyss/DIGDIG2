using UnityEngine;
/// <summary>
/// This script must be on the SAME GameObject as the Animator component.
/// It forwards animation events to the EnemyRange script.
/// </summary>
public class EnemyRangeAnimationReceiver : MonoBehaviour
{
    public EnemyRange enemyRange;

    private void Start()
    {
       
        if (enemyRange == null)
        {
            enemyRange = GetComponentInParent<EnemyRange>();
            if (enemyRange == null)
            {
                enemyRange = Object.FindAnyObjectByType<EnemyRange>();
            }
        }

        if (enemyRange == null)
        {
            Debug.LogError("EnemyRangeAnimationReceiver could not find EnemyRange script!");
        }
    }

   
    public void OnWindUpComplete()
    {
        if (enemyRange != null)
            enemyRange.OnWindUpComplete();
    }

    public void SpawnEnemy()
    {  
        if (enemyRange != null)
            enemyRange.SpawnEnemy();
    }

    public void OnAttackComplete()
    {
        if (enemyRange != null)
            enemyRange.OnAttackComplete();
    }
}
