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

    [SerializeField] bool noticePlayer = false;
    Coroutine patrolCoroutine;

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


        

        //if (noticePlayer)
        //{
        //    if (patrolCoroutine != null)
        //    {
        //         StopCoroutine(patrolCoroutine);
        //         patrolCoroutine = null;
        //    }
        //    agent.isStopped = false;
        //    agent.SetDestination(player.transform.position);
            


        //    Debug.Log("Chasing Player");
        //}
        //if (!noticePlayer)
        //{
        //    patrolCoroutine = StartCoroutine(Patrol());
        //    Debug.Log("Patrolling");
        //}


    private void Update()
    {
        if (noticePlayer)
        {
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
                patrolCoroutine = null;
            }

            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            Debug.Log("Chasing Player");
        }
        else
        {
            if (patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
                Debug.Log("Patrolling");
            }
        }

        if (player != null && agent != null && !animator.GetBool("IsDead"))
        {
            if (!animator.GetBool("IsAttacking"))
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("IsMoving", true);
            }
        }
    }


        

        //if (noticePlayer)
        //{
            
        //    if (patrolCoroutine != null)
        //    {
        //        StopCoroutine(patrolCoroutine);
        //        patrolCoroutine = null;
        //    }
            
        //    agent.isStopped = false;
        //    agent.SetDestination(player.transform.position);
        //}
        //else
        //{
        //    if (patrolCoroutine == null)
        //    {
        //        patrolCoroutine = StartCoroutine(Patrol());
        //    }
        //}
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (player != null && other == player.GetComponent<Collider>())
        {
            agent.isStopped = true;
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsMoving", false);
            StartCoroutine(AfterAttack());
            
        }
        
        // Enemy Damage
        if (other.CompareTag("Player"))
        {

            Debug.Log("Doing attack!");
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                //  playerController.TakeDamage(1);
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
    //Enemy takes damage
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 1.5f);
        }
    }

    IEnumerator Patrol()
    {
        while (!noticePlayer)
        {
            float rotationY = Random.Range(0, 360);
            EnemyTransform.Rotate(0, rotationY, 0);

            // Move forward a bit in the new direction
            Vector3 forwardMove = EnemyTransform.position + EnemyTransform.forward * 4f;
            agent.SetDestination(forwardMove);

            yield return new WaitForSeconds(5f);
        }
    }

    //IEnumerator Patrol()
    //{
    //    while (!noticePlayer && !animator.GetBool("IsDead"))
    //    {
    //        float rotationY = Random.Range(0f, 360f);
    //        EnemyTransform.Rotate(0, rotationY, 0);


    //        // Move forward a bit in the new direction
    //        Vector3 forwardMove = EnemyTransform.position + EnemyTransform.forward * 4f;
    //        agent.SetDestination(forwardMove);

    //        yield return new WaitForSeconds(5f);
    //    }
    //}


}
