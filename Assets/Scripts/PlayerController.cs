using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float PlayerSpeed = 7f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = transform.right * PlayerSpeed;
            Debug.Log("Moving Right");
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = -transform.right * PlayerSpeed;
            Debug.Log("Moving Left");
        }

    }
}
