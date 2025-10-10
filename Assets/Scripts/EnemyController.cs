using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] Rigidbody Enemyrb;
    float enemymoveSpeed = 5f;
    [SerializeField] int enemyHealth = 3;
    [SerializeField] Collider detectCollider;
    [SerializeField] Collider collisionCollider;
    NavMeshAgent agent;


    PlayerController playerController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = FindFirstObjectByType<PlayerController>().gameObject;
    }


    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(Player.transform.position);

        //if (Player != null)
        //{
        //    Vector3 direction = (Player.transform.position - transform.position).normalized;
        //    Enemyrb.MovePosition(transform.position + direction * enemymoveSpeed * Time.deltaTime);
        //}
    }

    private void TriggerEnter2D(Collider other)
    {
        // Fix nav mesh agent so it stops when detecting player
        if (Player != null && other == Player.GetComponent<Collider>())
        {
            agent.isStopped = true;
        }
    }
    private void TriggerExit2D(Collider other)
    {
        if (Player != null && other == Player.GetComponent<Collider>())
        {
            agent.isStopped = false;
        }
    }




    // CONTINUE WORK ON THIS: DETECT WHEN PLAYER IS IN CERTAIN RADIUS OF ENEMY
    //Physics.Spherecast(GameObject.transform.position, 0.5f, Vector3.forward, out RaycastHit hitInfo, 1f);
    
    
    //public void TakeDamage()
    //{

    //}
}