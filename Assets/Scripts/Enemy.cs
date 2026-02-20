using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        #region Patrol Settings
        public Vector3 areaCenter = Vector3.zero;
        public Vector3 areaSize = new Vector3(10, 0, 10);
        public float waitTime = 2f;
        #endregion

        #region Vision Settings
        public float visionDistance = 15f;
        public float viewAngle = 90f;
        public LayerMask visionMask;
        #endregion

        #region Attack Settings
        [Header("Attack Settings")]
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public float alertDuration = 1.0f;
        public Collider attackCollider;
        #endregion

        #region Private Fields
        private NavMeshAgent agent;
        private Animator animator;
        private float timer;
        private Transform player;
        private bool hasSeenPlayer = false;
        private bool isAlerting = false;
        private float alertTimer = 0f;

        #region Health Settings
        [Header("Health Settings")]
        public float maxHealth = 100f;
        private float currentHealth;
        private bool isDead = false;
        #endregion

        #endregion

        #region Audio/Animation Fields

        public AudioSource walkingAudioSource;
        public AudioClip walkingClip;

        public AudioSource alertAudioSource;
        public AudioClip alertClip;

        #endregion

        #region Unity Events

        /// <summary>
        /// Initializes the enemy and sets up references.
        /// </summary>
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timer = waitTime;
            MoveToNewPoint();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;

            currentHealth = maxHealth;
        }

        /// <summary>
        /// Handles enemy state updates each frame.
        /// </summary>
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
                    EnterAlertState();
                    return;
                }

                if (isAlerting)
                {
                    UpdateAlertState();
                    return;
                }

                if (hasSeenPlayer)
                {
                    HandleChaseOrAttack();
                    return;
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

        /// <summary>
        /// Visualizes patrol area and vision in the editor.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(areaCenter, areaSize);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, visionDistance);

            // Draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * visionDistance);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * visionDistance);

            if (player != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, player.position);

                if (Application.isPlaying && CanSeePlayer())
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, player.position);
                }
            }
        }

        /// <summary>
        /// Handles attack collider trigger events.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (attackCollider != null && attackCollider.enabled && other.CompareTag("Player"))
            {
                Debug.Log("Enemy deals damage to the player!");
            }
        }

        #endregion

        #region State Methods

        /// <summary>
        /// Checks if the enemy can see the player.
        /// </summary>
        bool CanSeePlayer()
        {
            Vector3 origin = transform.position + Vector3.up * 1.0f;
            Vector3 directionToPlayer = (player.position - origin).normalized;
            float distanceToPlayer = Vector3.Distance(origin, player.position);

            if (distanceToPlayer > visionDistance)
                return false;

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer > viewAngle / 2f)
                return false;

            RaycastHit hit;
            Debug.DrawRay(origin, directionToPlayer * visionDistance, Color.red);

            if (Physics.Raycast(origin, directionToPlayer, out hit, visionDistance, visionMask))
            {
                if (hit.transform.CompareTag("Player"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the enemy to a new random point within the patrol area.
        /// </summary>
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

        /// <summary>
        /// Handles entering the alert state when the player is first seen.
        /// </summary>
        void EnterAlertState()
        {
            isAlerting = true;
            alertTimer = alertDuration;
            animator.SetBool("IsAlert", true);
            agent.ResetPath();
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttacking", false);
        }

        /// <summary>
        /// Updates the alert state timer and transitions out of alert.
        /// </summary>
        void UpdateAlertState()
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0f)
            {
                isAlerting = false;
                hasSeenPlayer = true;
                animator.SetBool("IsAlert", false);
            }
        }

        /// <summary>
        /// Handles chasing or attacking the player.
        /// </summary>
        void HandleChaseOrAttack()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRange)
            {
                agent.SetDestination(player.position);
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                agent.ResetPath();
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsAttacking", true);
            }
        }

        #endregion

        #region Animation Event Methods

        /// <summary>
        /// Called at the start of the attack animation.
        /// </summary>
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

        /// <summary>
        /// Called at the end of the attack animation.
        /// </summary>
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

        /// <summary>
        /// Called by the WalkingSound animation event to play the walking sound.
        /// </summary>
        public void WalkingSound()
        {
            if (walkingAudioSource != null && walkingClip != null)
            {
                walkingAudioSource.PlayOneShot(walkingClip);
            }
            else
            {
                Debug.LogWarning("Walking audio source or clip not assigned!");
            }
        }

        /// <summary>
        /// Called by the SoundAlert animation event to play the alert sound.
        /// </summary>
        public void SoundAlert()
        {
            if (alertAudioSource != null && alertClip != null)
            {
                alertAudioSource.PlayOneShot(alertClip);
            }
            else
            {
                Debug.LogWarning("Alert audio source or clip not assigned!");
            }
        }

        #endregion

        /// <summary>
        /// Call this method to deal damage to the enemy.
        /// </summary>
        public void TakeDamage(float amount)
        {
            if (isDead)
                return;

            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles enemy death.
        /// </summary>
        private void Die()
        {
            isDead = true;
            animator.SetBool("IsDead", true);

            // Stop movement
            agent.isStopped = true;
            agent.enabled = false;

            // Disable attack collider
            if (attackCollider != null)
                attackCollider.enabled = false;

            // Disable all other colliders on this GameObject
            foreach (var col in GetComponents<Collider>())
                col.enabled = false;

            // Disable this script to stop Update and other logic
            enabled = false;
        }
    }
 }

