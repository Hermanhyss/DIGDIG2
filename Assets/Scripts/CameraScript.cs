using UnityEngine;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player; 
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float zOffset = 2f;
    [SerializeField] private float minZ = -10f; 
    [SerializeField] private float maxZ = 10f;  
    [SerializeField] private float minY = -5f;   // Added minY
    [SerializeField] private float maxY = 5f;    // Added maxY
    [SerializeField] private float offsetY = 0f; // Added offsetY
    [SerializeField] private float fadeAlpha = 0.3f; // Target alpha for faded objects

    private Vector3 velocity = Vector3.zero;
    private List<FadeObject> lastFadedObjects = new List<FadeObject>();

    void Update()
    {
        if (player == null) return;

        float targetZ = player.position.z + zOffset;
        targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

        float targetY = player.position.y + offsetY;
        targetY = Mathf.Clamp(targetY, minY, maxY);

        Vector3 targetPosition = new Vector3(player.position.x, targetY, targetZ);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Raycast from camera to player
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Fade logic
        FadeObjectsBetweenCameraAndPlayer(directionToPlayer, distanceToPlayer);

        // Debug lines for Z
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

        // Debug lines for Y
        Debug.DrawLine(
            new Vector3(transform.position.x - 10, minY, transform.position.z),
            new Vector3(transform.position.x + 10, minY, transform.position.z),
            Color.blue
        );
        Debug.DrawLine(
            new Vector3(transform.position.x - 10, maxY, transform.position.z),
            new Vector3(transform.position.x + 10, maxY, transform.position.z),
            Color.yellow
        );
    }

    private void FadeObjectsBetweenCameraAndPlayer(Vector3 directionToPlayer, float distanceToPlayer)
    {
        // Restore previous faded objects
        foreach (var faded in lastFadedObjects)
        {
            if (faded != null)
                faded.SetAlpha(1f);
        }
        lastFadedObjects.Clear();

        // RaycastAll to find all objects between camera and player
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToPlayer.normalized, distanceToPlayer);
        foreach (var hit in hits)
        {
            if (hit.transform == player) continue;

            FadeObject fadeObj = hit.transform.GetComponent<FadeObject>();
            if (fadeObj != null)
            {
                fadeObj.SetAlpha(fadeAlpha);
                lastFadedObjects.Add(fadeObj);
            }
        }
    }
}
