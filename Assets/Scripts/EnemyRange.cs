using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    [SerializeField] private GameObject enemyFlyingPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private int maxSpawns = 5;
    [SerializeField] private Animator animator;

    private Transform player;
    private int spawnCount = 0;
    private bool isAttacking = false;

    private readonly string isIdleBool = "IsIdle";
    private readonly string windUpBool = "IsWindUp";
    private readonly string attackBool = "IsAttacking";

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("Player not found in scene!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in EnemyRange!");
        }
        else
        {
            Debug.Log("EnemyRange initialized successfully with Animator.");
        }
    }
    private void Update()
    {
        if (player == null || spawnCount >= maxSpawns || isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            StartAttackSequence();
        }
    }
    private void StartAttackSequence()
    {
        Debug.Log("StartAttackSequence called - starting WindUp animation");
        isAttacking = true;
        animator.SetBool(isIdleBool, false);
        animator.SetBool(windUpBool, true);
    }

    public void OnWindUpComplete()
    {
        Debug.Log("OnWindUpComplete called!");
        animator.SetBool(windUpBool, false);
        animator.SetBool(attackBool, true);
    }
    public void SpawnEnemy()
    {
        Debug.Log("SpawnEnemy called!");
        if (enemyFlyingPrefab != null)
        {
            Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
            Instantiate(enemyFlyingPrefab, spawnPosition, Quaternion.identity);
            spawnCount++;
        }
        else
        {
            Debug.LogWarning("FlyingEnemy prefab not assigned in EnemyRange.");
        }
    }
    public void OnAttackComplete()
    {
        Debug.Log("OnAttackComplete called!");
        animator.SetBool(attackBool, false);
        animator.SetBool(isIdleBool, true);
        isAttacking = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
