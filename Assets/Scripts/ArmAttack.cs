using UnityEngine;

public class ArmAttack : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 targetPosition;
    private bool isAttacking = false;

    // Call this to initialize the attack
    // spawnX: X position (left or right of player)
    // spawnY: Y position (usually same as player or ground)
    // spawnZ: Z position along the line where the arm can spawn
    // targetZ: The Z position to attack (usually the player's Z)
    public void Initialize(float spawnX, float spawnY, float spawnZ, float targetZ)
    {
        transform.position = new Vector3(spawnX, spawnY, spawnZ);
        targetPosition = new Vector3(spawnX, spawnY, targetZ);
        isAttacking = true;
    }

    void Update()
    {
        if (isAttacking)
        {
            // Move towards the target Z position in a straight line
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Optionally, destroy or deactivate when reached
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isAttacking = false;
                // Destroy(gameObject); // or deactivate, play animation, etc.
            }
        }
    }
}
