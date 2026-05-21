using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex;

    private Checkpoints checkpointManager; // Reference to parent manager

    void Start()
    {
        checkpointManager = GetComponentInParent<Checkpoints>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collided with checkpoint {checkpointIndex}!");
            checkpointManager.GetCheckpointNumber(checkpointIndex);
        }
    }
}
