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
    [SerializeField, Range(0, 360)] private float viewAngle = 90f;
    [SerializeField] private float viewDistance = 10f;

    private int currentPatrolIndex = 0;
    private bool canAttack = true;
    private bool isChasingPlayer = false;
    private bool hasSpottedPlayer = false;

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

        // If the player has ever been spotted, always chase
        if (hasSpottedPlayer || PlayerInSight())
        {
            hasSpottedPlayer = true;
            isChasingPlayer = true;
            agent.SetDestination(player.transform.position);
        }

        bool shouldMove = !animator.GetBool("IsAttacking");
        if (animator.GetBool("IsMoving") != shouldMove)
        {
            animator.SetBool("IsMoving", shouldMove);
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
        yield return new WaitForSeconds(3f);
        agent.isStopped = false;
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsAttacking", false);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            EnemyDie();      
        }
    }

    public void EnemyDie()
    {
        animator.SetBool("IsDead", true);

        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        enemyRb.linearVelocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;
        enemyRb.isKinematic = true;

        collisionCollider.enabled = false;
        attackCollider.enabled = false;
    }
  

    private bool PlayerInSight()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > viewDistance) return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > viewAngle * 0.5f) return false;

        // Raycast to check for clear line of sight between enemy and player
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
        {
            // If the first thing hit isn't the player, line of sight is blocked
            if (hit.collider.gameObject != player)
                return false;
        }
        else
        {
            // Nothing was hit, so no line of sight
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw view distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Draw view angle
        Vector3 forward = transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle * 0.5f, 0) * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);

        // Draw the raycast for line of sight (even in edit mode if player is assigned)
        if (player != null)
        {
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float distance = Mathf.Min(viewDistance, Vector3.Distance(transform.position, player.transform.position));

            // Perform the same raycast as in PlayerInSight
            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
            {
                Gizmos.color = hit.collider.gameObject == player ? Color.green : Color.magenta;
                Gizmos.DrawLine(origin, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.2f);
            }
            else
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(origin, origin + direction * viewDistance);
            }
        }

        // Optionally, draw a line to the player if in sight during play mode
        if (Application.isPlaying && player != null && PlayerInSight())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}
