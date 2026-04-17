using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    public Transform player; 
    public float speed = 5f;
    public float heightAdjustSpeed = 2f;
    public LayerMask obstacleMask;



    private void Update()
    {
        if (player == null)
            return;

        Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, heightAdjustSpeed * Time.deltaTime);

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
        {
            // No obstacle, move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
