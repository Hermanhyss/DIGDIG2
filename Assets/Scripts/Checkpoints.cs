using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Transform playerTransform;

    public List<Transform> checkpoints;

    private int highestCheckpointReached = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("Player collided with checkpoint!");

            for (int i = 0; i < checkpoints.Count; i++)
            {
                if (Vector3.Distance(other.transform.position, checkpoints[i].position) < 0.5f)
                {
                    GetCheckpointNumber(i + 1);
                    break;
                }
            }
        }
    }

    public void GetCheckpointNumber(int checkpoint)
    {
        if (checkpoint > highestCheckpointReached)
        {
            highestCheckpointReached = checkpoint;
            Debug.Log("Checkpoint " + checkpoint + " reached!");
        }
    }

    
            //playerTransform.position = checkpointTransform.position;
            //playerTransform.rotation = checkpointTransform.rotation;

    
}
