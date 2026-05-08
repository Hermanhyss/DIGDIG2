using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    Transform playerTransform;
    CapsuleCollider playerCollider;

    public float radius = 5f;

    //ADD A IDLE
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().gameObject.transform;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 relativePos = playerTransform.position - transform.position;

                // the second argument, upwards, defaults to Vector3.up
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                transform.rotation = rotation;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
