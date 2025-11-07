using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] Transform EnemyTransform;
    [SerializeField] Rigidbody Enemyrb;
    float enemymoveSpeed = 5f;
    [SerializeField] int enemyHealth = 3;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] Collider detectCollider;
    [SerializeField] Collider collisionCollider;
    NavMeshAgent agent;
    bool noticePlayer = false;
    Coroutine patrolCoroutine;


    PlayerController playerController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = FindFirstObjectByType<PlayerController>().gameObject;
    }


    void Start()
    {
        patrolCoroutine = StartCoroutine(Patrol());
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        
        if(Player != null) 
        {

            if (noticePlayer)
            {
                agent.SetDestination(Player.transform.position);
                agent.isStopped = false;

                if (patrolCoroutine != null)
                {
                    StopCoroutine(patrolCoroutine);
                    patrolCoroutine = null;
                }
            }
            else if (!noticePlayer)
            {
                if (patrolCoroutine == null)
                {
                    patrolCoroutine = StartCoroutine(Patrol());
                }
            }

            return;
        }



        //if (Player != null)
        //{
        //    Vector3 direction = (Player.transform.position - transform.position).normalized;
        //    Enemyrb.MovePosition(transform.position + direction * enemymoveSpeed * Time.deltaTime);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        // Fix nav mesh agent so it stops when detecting player
        if (Player != null && other == Player.GetComponent<Collider>())
        {
            agent.isStopped = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (Player != null && other == Player.GetComponent<Collider>())
        {
            agent.isStopped = false;
        }
    }

    void CheckForPlayer()
    {
        //Check if enemy notices player
        

        if (Physics.SphereCast(EnemyTransform.position, 1, EnemyTransform.forward, out RaycastHit hitInfo, detectionRadius, LayerMask.GetMask("Player")))
        {
            noticePlayer = true;
        }
        else
        {
            noticePlayer = false;
        }
    }

    IEnumerator Patrol()
    {
        //Check if enemy notices player
        float rotationY = Random.Range(-100, 100);
        EnemyTransform.Rotate(0, rotationY, 0);

        Vector3 forwardMove = EnemyTransform.forward * 4;
        agent.SetDestination(EnemyTransform.position + forwardMove);

        yield return new WaitForSeconds(5f);
        
    }


    // CONTINUE WORK ON THIS: DETECT WHEN PLAYER IS IN CERTAIN RADIUS OF ENEMY
    


    //public void TakeDamage()
    //{

    //}
}