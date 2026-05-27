using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Animator animator;

    private Transform player;
    private bool isCharging = false;
    private bool isAttacking = false;
    private Vector3 targetPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", true);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isCharging && !isAttacking && distanceToPlayer <= detectionRange)
        {
            StartCharging();
        }

        if (isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    private void StartCharging()
    {
        isCharging = true;

        if (animator != null)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsCharging", true);
        }
    }

    // Denna metod kallas från Animation Event i Charging-animationen
    public void OnChargingComplete()
    {
        StartAttacking();
    }

    private void StartAttacking()
    {
        isCharging = false;
        isAttacking = true;

        // Spara spelarens nuvarande position
        if (player != null)
        {
            targetPosition = player.position;
        }

        if (animator != null)
        {
            animator.SetBool("IsCharging", false);
            animator.SetBool("IsAttacking", true);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Explodera när den träffar något
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
