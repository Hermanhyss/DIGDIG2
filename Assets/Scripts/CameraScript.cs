using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign in Inspector
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float zOffset = 2f;
    [SerializeField] private float minZ = -10f; // Minimum z value
    [SerializeField] private float maxZ = 10f;  // Maximum z value

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (player == null) return;

        // Camera follows player's z position with offset, both forward and backward
        float targetZ = player.position.z + zOffset;

        // Clamp the targetZ value
        targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, targetZ);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
