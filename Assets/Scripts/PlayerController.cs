using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float playerSpeed = 7f;
    [SerializeField] float jumpStrength = 5.0f;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.82f;
    [SerializeField] float coyoteTime = 0.2f;
    float coyoteTimer;
    int jumpCount;
    int maxJumpCount = 2;

    [SerializeField] GameObject walkingEffect;
    ParticleSystem walkingParticleSystem;

    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask Ground;
    Rigidbody rb;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.02f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (walkingEffect != null)
            walkingParticleSystem = walkingEffect.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, Ground);

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        Vector3 vel = rb.linearVelocity;

        if (Input.GetKey(KeyCode.D))
        {
            vel.x = transform.right.x * playerSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            vel.x = -transform.right.x * playerSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            vel.z = transform.forward.z * playerSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            vel.z = -transform.forward.z * playerSpeed;
        }

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        WalkingEffect();
    }

    void Jump()
    {
        if(isGrounded || !isGrounded && jumpCount == maxJumpCount && coyoteTimer > 0)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            coyoteTimer = 0f;
            jumpCount--;
            isGrounded = false;
        } else if(!isGrounded && jumpCount == 1 || !isGrounded && jumpCount == maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            jumpCount--;
        }
    }

    void WalkingEffect()
    {
        bool isMoving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                         Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));

        if (walkingParticleSystem != null)
        {
            if (isGrounded && isMoving)
            {
                if (!walkingParticleSystem.isPlaying)
                    walkingParticleSystem.Play();
            }
            else
            {
                if (walkingParticleSystem.isPlaying)
                    walkingParticleSystem.Stop();   
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.layer == 6) & isGrounded) 
        {
            jumpCount = maxJumpCount;
        }
    }
}
