using UnityEngine;

public class DennisMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] float extraGravity;

    [SerializeField] float peak;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
       
        
        if(rb.linearVelocity.y < peak && rb.linearVelocity.y > -peak)
        {
            Debug.Log("PEAK OF JUMP");
        } else
        {
            rb.AddForce(-transform.up * extraGravity);
        }
    }
}
