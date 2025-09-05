using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float playerSpeed = 7f;
    [SerializeField] float jumpStrength = 5.0f;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.82f;
    [SerializeField] float coyoteTime = 0.2f;
    float coyoteTimer;

    [SerializeField] float jumpBufferTime = 0.2f;
    float jumpBufferTimer;

    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask Ground;
    Rigidbody rb;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, Ground);

        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.fixedDeltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.fixedDeltaTime;

        Vector3 vel = rb.linearVelocity;

        if (Input.GetKey(KeyCode.D))
            vel.x = transform.right.x * playerSpeed;

        if (Input.GetKey(KeyCode.A))
            vel.x = -transform.right.x * playerSpeed;

        if (Input.GetKey(KeyCode.W))
            vel.z = transform.forward.z * playerSpeed;

        if (Input.GetKey(KeyCode.S))
            vel.z = -transform.forward.z * playerSpeed;

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        Jump();
    }

    void Jump()
    {
        if (coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
            Debug.Log("Jumped with coyoteTimer = " + coyoteTimer);
        }
    }
}
