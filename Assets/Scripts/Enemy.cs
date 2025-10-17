using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody enemyRb;
    [SerializeField] private Collider detectCollider;
    [SerializeField] private Collider collisionCollider;
    [SerializeField] private Animator animator; // Add this line
    private NavMeshAgent agent;
    private GameObject player;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
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

    private void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.transform.position);
            animator.SetBool("IsMoving", true); // Play movement animation
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null && other == player.GetComponent<Collider>())
        {
            agent.isStopped = true;
            animator.SetTrigger("Attack"); // Play attack animation
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null && other == player.GetComponent<Collider>())
        {
            agent.isStopped = false;
            animator.SetBool("IsMoving", true); // Resume movement animation
        }
    }

    // Example: Detect player within a certain radius
    private void DetectPlayer()
    {
        float detectionRadius = 5f;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject == player)
            {
                // Player detected
            }
        }
    }

    // Example: Take damage
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            animator.SetTrigger("Death"); // Play death animation
            Destroy(gameObject, 1.5f); // Delay destroy to allow animation to play
        }
    }
}
