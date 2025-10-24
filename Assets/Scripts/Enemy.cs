using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody enemyRb;
    [SerializeField] private Collider detectCollider;
    [SerializeField] private Collider collisionCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider attackCollider;
    private NavMeshAgent agent;
    private GameObject player;

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


    private void Update()
    {
        if (player != null && agent != null && !animator.GetBool("IsDead"))
        {
            if (!animator.GetBool("IsAttacking"))
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("IsMoving", true);
            }
        }
        

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



    //private void OnTriggerExit(Collider other)
    //{
    //    if (player != null && other == player.GetComponent<Collider>())
    //    {
    //        agent.isStopped = false;
    //        animator.SetBool("IsMoving", true);
    //        animator.SetBool("IsAttacking", false);
    //    }
    //}
    // Enemy takes damage
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
