using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody enemyRb;
    [SerializeField] private Transform EnemyTransform;
    [SerializeField] private Collider collisionCollider; // Attack range
    [SerializeField] private Animator animator;
    [SerializeField] private Collider attackCollider;    // Deals damage
    private NavMeshAgent agent;
    private GameObject player;

    [Header("Settings")]
    [SerializeField] public int health = 3;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float playerDetectionRange = 10f;
    [SerializeField, Range(0, 360)] private float viewAngle = 90f;
    [SerializeField] private float viewDistance = 10f;

    private int currentPatrolIndex = 0;
    private bool canAttack = true;
    private bool isChasingPlayer = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        var playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.gameObject;
        }
        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    private void Update()
    {
        if (animator.GetBool("IsDead")) return;

        if (PlayerInSight())
        {
            isChasingPlayer = true;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            isChasingPlayer = false;
            Patrol();
        }

        bool shouldMove = !animator.GetBool("IsAttacking");
        if (animator.GetBool("IsMoving") != shouldMove)
        {
            animator.SetBool("IsMoving", shouldMove);
        }
    }

    private void Patrol()
    {
        // Patrol implementation here
        if (patrolPoints.Length == 0) return;

        // Set the destination to the current patrol point
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        // Check if the enemy has reached the patrol point
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Switch to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    public void OnAttackAnimationEvent()
    {
        Debug.Log("Enabling attack collider");
        if (attackCollider != null)
            attackCollider.enabled = true;
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log("Disabling attack collider");
        if (attackCollider != null)
            attackCollider.enabled = false;
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (collisionCollider != null && other == player.GetComponent<Collider>())
        {
            agent.isStopped = true;
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsMoving", false);
        }
        
        if (attackCollider != null && other != null && other.gameObject == player && attackCollider.enabled)
        {
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
               // playerController.TakeDamage(1); 
                Debug.Log("Player took damage from enemy attack!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            agent.isStopped = true;
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsMoving", false);
            canAttack = false;
            StartCoroutine(AttackCooldown());

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collisionCollider != null && other == player.GetComponent<Collider>())
        {
            StartCoroutine(BeforeMoving());     
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsAttacking", false);
        canAttack = true;
    }

    private IEnumerator BeforeMoving()
    {
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsAttacking", false);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 1.5f);
        }
    }

    private bool PlayerInSight()
{
    if (player == null) return false;

    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

    if (distanceToPlayer > viewDistance) return false;

    float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
    if (angleToPlayer > viewAngle * 0.5f) return false;

    // Optional: Raycast to check for obstacles between enemy and player
    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer, out RaycastHit hit, viewDistance))
    {
        if (hit.collider.gameObject != player)
            return false;
    }

    return true;
}
}
