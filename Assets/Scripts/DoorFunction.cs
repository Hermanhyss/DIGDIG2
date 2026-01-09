using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    private Animator animator;
    private bool DoorOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.name + ", Tag: " + other.tag);
        if (other.CompareTag("Player") && !DoorOpen)
        {
            Debug.Log("Player entered and door is closed. Opening door.");
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: " + other.name + ", Tag: " + other.tag);
        if (other.CompareTag("Player") && DoorOpen)
        {
            Debug.Log("Player exited and door is open. Closing door.");
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        animator.SetBool("DoorOpen", true);
        DoorOpen = true;
    }

    private void CloseDoor()
    {
        animator.SetBool("DoorOpen", false);
        DoorOpen = false;
    }
}
