using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float playerSpeed = 7f;
    [SerializeField] float jumpStrength = 5.0f;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.82f;
    [SerializeField] float coyoteTime = 0.2f;
    float coyoteTimer;

    [SerializeField] GameObject walkingEffect;
    ParticleSystem walkingParticleSystem;

    [SerializeField] float jumpBufferTime = 0.2f;
    float jumpBufferTimer;

    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask Ground;
    Rigidbody rb;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.2f;

    ChromaticPulse chromaticPulse;
    ChromaticVignettePulse chromaticVignettePulse;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        chromaticPulse = FindFirstObjectByType<ChromaticPulse>();
        chromaticVignettePulse = FindFirstObjectByType<ChromaticVignettePulse>();
        if (walkingEffect != null)
            walkingParticleSystem = walkingEffect.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Jump();
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

        //NOT DONE
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = transform.forward * playerSpeed;
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = transform.forward * playerSpeed;
            gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity = transform.forward * playerSpeed;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = transform.forward * playerSpeed;
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        WalkingEffect();
    }

    void Jump()
    {
        if (coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            //StartCoroutine(chromaticPulse.Pulse());
            //StartCoroutine(chromaticVignettePulse.Pulse());

            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
            Debug.Log("Jumped with coyoteTimer = " + coyoteTimer);
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
}