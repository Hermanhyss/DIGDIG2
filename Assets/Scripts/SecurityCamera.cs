using UnityEngine;
public class SecurityCamera : MonoBehaviour
{
    Transform playerTransform;
    CapsuleCollider playerCollider;
    public float radius = 5f;
    public float timeCount = 0.0f;
    public Vector3 From;
    public Vector3 To;
    public float returnSpeed = 90;
    public float rotSpeed;
    public float trackSpeed;
    //ADD A IDLE
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().gameObject.transform;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        bool foundPlayer = false;
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                foundPlayer = true;
                break;
            }
        }
        if (foundPlayer)
        {
            timeCount = 0f;
            rotateTowardsPlayer();
        }
        else
        {
            Quaternion fromRot = Quaternion.Euler(From);
            Quaternion toRot = Quaternion.Euler(To);

            float step = returnSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Slerp(fromRot, toRot, timeCount), step);

            if (Quaternion.Angle(transform.localRotation, Quaternion.Slerp(fromRot, toRot, timeCount)) < 1f)
            {
                timeCount += Time.deltaTime * rotSpeed;
                timeCount = Mathf.Clamp01(timeCount);
            }

            if (timeCount >= 1)
            {
                timeCount = 0f;
                (From, To) = (To, From);
            }
        }
    }
    void rotateTowardsPlayer()
    {
        Vector3 relativePos = playerTransform.position - transform.position;
        relativePos.y = 0f;
        Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, trackSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}