using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        public Vector3 areaCenter = Vector3.zero;
        public Vector3 areaSize = new Vector3(10, 0, 10);
        public float waitTime = 2f;
        public float visionDistance = 15f; 
        public float viewAngle = 90f; 
        public LayerMask visionMask; 

        [Header("Attack Settings")]
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public float alertDuration = 1.0f; 
        public Collider attackCollider;

        private NavMeshAgent agent;
        private Animator animator;
        private float timer;
        private Transform player;
        private float lastAttackTime = -Mathf.Infinity;
        private bool hasSeenPlayer = false; 
        private bool isAlerting = false;
        private float alertTimer = 0f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timer = waitTime;
            MoveToNewPoint();

            
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        void Update()
        {
            bool isAttacking = animator.GetBool("IsAttacking");

            if (!isAttacking && !isAlerting)
            {
                bool isMoving = agent.velocity.magnitude > 0.1f;
                animator.SetBool("IsMoving", isMoving);
                animator.SetBool("IsIdle", !isMoving);
            }

            if (player != null)
            {
                if (CanSeePlayer() && !hasSeenPlayer && !isAlerting)
                {
                    // First time seeing player: enter alert state
                    isAlerting = true;
                    alertTimer = alertDuration;
                    animator.SetBool("IsAlert", true);
                    agent.ResetPath();
                    animator.SetBool("IsMoving", false);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsAttacking", false);
                    return;
                }

                if (isAlerting)
                {
                    alertTimer -= Time.deltaTime;
                    if (alertTimer <= 0f)
                    {
                        isAlerting = false;
                        hasSeenPlayer = true;
                        animator.SetBool("IsAlert", false);
                    }
                    return; // Wait until alert is done
                }

                if (hasSeenPlayer)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                    if (distanceToPlayer > attackRange)
                    {
                        // Chase the player
                        agent.SetDestination(player.position);
                        animator.SetBool("IsAttacking", false);
                        animator.SetBool("IsMoving", true);
                    }
                    else
                    {
                        // Attack the player
                        agent.ResetPath();
                        animator.SetBool("IsMoving", false);
                        animator.SetBool("IsAttacking", true);
                    }
                    return; // Skip random movement if chasing/attacking
                }
            }

            animator.SetBool("IsAttacking", false);

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    MoveToNewPoint();
                    timer = waitTime;
                }
            }
        }

        bool CanSeePlayer()
        {
            Vector3 origin = transform.position + Vector3.up * 1.0f; // Raise the raycast origin
            Vector3 directionToPlayer = (player.position - origin).normalized;
            float distanceToPlayer = Vector3.Distance(origin, player.position);

            if (distanceToPlayer > visionDistance)
                return false;

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer > viewAngle / 2f)
                return false;

            RaycastHit hit;
            Debug.DrawRay(origin, directionToPlayer * visionDistance, Color.red); // Visualize the ray

            if (Physics.Raycast(origin, directionToPlayer, out hit, visionDistance, visionMask))
            {
                if (hit.transform.CompareTag("Player"))
                    return true;
            }
            return false;
        }

        void MoveToNewPoint()
        {
            Vector3 randomPoint = areaCenter + new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                0,
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }



        // Call this from animation event at the start of the attack animation
        public void AttackingAnimationStart()
        {
            if (attackCollider != null)
            {
                attackCollider.enabled = true;
                Debug.Log("Attack collider enabled");
            }
            else
            {
                Debug.LogWarning("Attack collider is not assigned!");
            }
        }

        // Call this from animation event at the end of the attack animation
        public void AttackingAnimationEnd()
        {
            if (attackCollider != null)
            {
                attackCollider.enabled = false;
                Debug.Log("Attack collider disabled");
            }
            else
            {
                Debug.LogWarning("Attack collider is not assigned!");
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(areaCenter, areaSize);

            // Draw vision range
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, visionDistance);

            // Draw vision angle
            Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * visionDistance);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * visionDistance);

            // Draw line to player
            if (player != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, player.position);

                // Draw a blue line if the enemy can see the player
                if (Application.isPlaying && CanSeePlayer())
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, player.position);
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (attackCollider != null && attackCollider.enabled && other.CompareTag("Player"))
            {
           
                Debug.Log("Enemy deals damage to the player!");

              
            }
        }
    }
}
