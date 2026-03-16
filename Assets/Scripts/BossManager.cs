using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("References")]
    public GameObject armPrefab;
    public Transform player;

    [Header("Room Spawn Lines")]
    public float leftX = -10f;
    public float rightX = 10f;
    public float minY = 0f; // Adjust as needed
    public float minZ = -5f; // Set your room's Z range
    public float maxZ = 5f;

    // Call this to trigger an arm attack from both sides
    public void SpawnArmAttack()
    {
        float playerZ = player.position.z;
        float playerY = player.position.y;

        // Left arm
        GameObject leftArm = Instantiate(armPrefab);
        leftArm.GetComponent<ArmAttack>().Initialize(leftX, playerY, playerZ, playerZ);

        // Right arm
        GameObject rightArm = Instantiate(armPrefab);
        rightArm.GetComponent<ArmAttack>().Initialize(rightX, playerY, playerZ, playerZ);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnArmAttack();
        }
    }

    // Draw spawn lines in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Left line
        Gizmos.DrawLine(
            new Vector3(leftX, minY, minZ),
            new Vector3(leftX, minY, maxZ)
        );
        // Right line
        Gizmos.DrawLine(
            new Vector3(rightX, minY, minZ),
            new Vector3(rightX, minY, maxZ)
        );
    }
}
