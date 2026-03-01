using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    private Animator animator;
    private bool DoorOpen = false;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        PlaySound();
    }

    private void CloseDoor()
    {
        animator.SetBool("DoorOpen", false);
        DoorOpen = false;
        PlaySound();
    }

    private void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
