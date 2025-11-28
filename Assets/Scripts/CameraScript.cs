using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player; 
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float zOffset = 2f;
    [SerializeField] private float minZ = -10f; 
    [SerializeField] private float maxZ = 10f;  

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (player == null) return;

       
        float targetZ = player.position.z + zOffset;

       
        targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, targetZ);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        
        Debug.DrawLine(
            new Vector3(transform.position.x - 10, transform.position.y, minZ),
            new Vector3(transform.position.x + 10, transform.position.y, minZ),
            Color.red
        );
        Debug.DrawLine(
            new Vector3(transform.position.x - 10, transform.position.y, maxZ),
            new Vector3(transform.position.x + 10, transform.position.y, maxZ),
            Color.green
        );
    }
}
