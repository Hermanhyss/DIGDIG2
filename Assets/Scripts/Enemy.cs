using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody enemyRb;
    [SerializeField] private Transform EnemyTransform;
    [SerializeField] private Collider detectCollider;
    [SerializeField] private Collider collisionCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider attackCollider;
    private NavMeshAgent agent;
    private GameObject player;

    [Header("Patrol Settings")]
    public Transform[] checkpoints;
    public float checkpointThreshold = 0.5f;
    public float minWaitTime = 3f;
    public float maxWaitTime = 5f;
    private int currentCheckpointIndex = 0;
    private bool isWaiting = false;

    [Header("Vision Settings")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Settings")]
    [SerializeField] private int health = 3;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        var playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.gameObject;
        }
    }

    private void Start()
    {
        if (checkpoints != null && checkpoints.Length > 0)
        {
            agent.SetDestination(checkpoints[currentCheckpointIndex].position);
        }
    }

    private void Update()
    {
        if (animator.GetBool("IsDead")) return;

        // Vision check
        if (CanSeePlayer())
        {
            if (player != null)
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("IsMoving", true);
                Debug.Log("Player detected!");
            }
            return;
        }

        // Patrol logic
        if (checkpoints == null || checkpoints.Length == 0 || isWaiting) return;

        if (!agent.pathPending && agent.remainingDistance <= checkpointThreshold)
        {
            StartCoroutine(LookAroundAndMove());
        }
    }

    IEnumerator LookAroundAndMove()
    {
        isWaiting = true;
        agent.isStopped = true;

        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        float elapsed = 0f;

        while (elapsed < waitTime)
        {
            // Optional: Rotate to simulate looking around
            transform.Rotate(0, 60f * Time.deltaTime, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
        currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Length;
        agent.SetDestination(checkpoints[currentCheckpointIndex].position);
        isWaiting = false;
    }

    bool CanSeePlayer()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewDistance);

        foreach (var target in targetsInView)
        {
            if (target.CompareTag("Player"))
            {
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                if (angleToTarget < viewAngle / 2f)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftBoundary * viewDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * viewDistance);
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (var target in targetsInView)
        {
            if (target.CompareTag("Player"))
            {
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);
                float distToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (angleToTarget < viewAngle / 2f)
                {
                    bool blocked = Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask);
                    Gizmos.color = blocked ? Color.red : Color.green;
                    Gizmos.DrawLine(transform.position, target.transform.position);
                }
            }
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

    IEnumerator AfterAttack()
    {
        if (player != null)
        {
            Debug.Log("Enemy has finished attacking");
            yield return new WaitForSeconds(3.0f);
            agent.isStopped = false;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsMoving", true);
        }
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null && other == player.GetComponent<Collider>())
        {
            agent.isStopped = true;
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsMoving", false);
            StartCoroutine(AfterAttack());
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Doing attack!");
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // playerController.TakeDamage(1);
                Debug.Log("Doing attack!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null && other == player.GetComponent<Collider>())
        {
            agent.isStopped = false;
            animator.SetBool("IsMoving", true);
            animator.SetBool("IsAttacking", false);
        }
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
}
