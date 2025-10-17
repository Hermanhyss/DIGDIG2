using UnityEngine;
using Unity.Cinemachine;
using System.Linq;

public class PositionTriggerScript : MonoBehaviour
{
    public Transform teleportDestination;
    public CinemachineCamera nextRoomCamera; // Updated to use CinemachineCamera

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Teleport player
        other.transform.SetPositionAndRotation(teleportDestination.position, teleportDestination.rotation);

        // Find active camera and lower its priority
        var activeCam = Object.FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None) 
            .OrderByDescending(c => c.Priority).FirstOrDefault();
        if (activeCam != null)
            activeCam.Priority = 5;

        // Raise next camera's priority
        if (nextRoomCamera != null)
            nextRoomCamera.Priority = 10;
    }
}

