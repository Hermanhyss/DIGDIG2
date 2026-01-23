using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    /// <summary>
    /// Controls enemy behavior: moves randomly within a defined area.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        public Vector3 areaCenter = Vector3.zero;
        public Vector3 areaSize = new Vector3(10, 0, 10);
        public float waitTime = 2f;
        public float visionDistance = 15f; // How far the enemy can see
        public float viewAngle = 90f; // Field of view in degrees
        public LayerMask visionMask; // Set this in the inspector to include obstacles and player

        public float attackRange = 2f;
        public float attackCooldown = 1.5f;

        private NavMeshAgent agent;
        private Animator animator;
        private float timer;
        private Transform player;
        private float lastAttackTime = -Mathf.Infinity;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timer = waitTime;
            MoveToNewPoint();

            // Find player by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        void Update()
        {
            bool isAttacking = animator.GetBool("IsAttacking");

            if (!isAttacking)
            {
                bool isMoving = agent.velocity.magnitude > 0.1f;
                animator.SetBool("IsMoving", isMoving);
                animator.SetBool("IsIdle", !isMoving);
            }

            if (player != null && CanSeePlayer())
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer > attackRange)
                {
                    // Chase the player
                    agent.SetDestination(player.position);
                    animator.SetBool("IsAttacking", false);
                }
                else
                {
                    // Attack the player
                    agent.ResetPath();
                    animator.SetBool("IsAttacking", true);

                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        AttackPlayer();
                        lastAttackTime = Time.time;
                    }
                }
                return; // Skip random movement if chasing/attacking
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

        void AttackPlayer()
        {
            Debug.Log("Enemy attacks the player!");
            // TODO: Add damage logic here
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
    }
}
