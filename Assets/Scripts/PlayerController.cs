using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 7f;
    [SerializeField] float jumpStrength = 5.0f;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.82f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float peak;
    [SerializeField] float extraGravity;    


    [SerializeField] GameObject walkingEffect;
    ParticleSystem walkingParticleSystem;

    [SerializeField] float jumpBufferTime = 0.2f;
    float jumpBufferTimer;
    int jumpCount = 2;

    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask Ground;
    Rigidbody rb;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.2f;

    bool canMove = true;
    Animator animator;
    ChromaticPulse chromaticPulse;
    ChromaticVignettePulse chromaticVignettePulse;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        chromaticPulse = FindFirstObjectByType<ChromaticPulse>();
        chromaticVignettePulse = FindFirstObjectByType<ChromaticVignettePulse>();
        if (walkingEffect != null)
            walkingParticleSystem = walkingEffect.GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, Ground);

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpCount = 1;
            animator.SetBool("Land", true);
        }
        else
        {
            animator.SetBool("Land", false);
            animator.SetBool("Jump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferTime;
            animator.SetBool("Jump", true);
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("IsAttacking", true);
            animator.SetTrigger("Attack light");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetBool("IsAttacking", true);
            animator.SetTrigger("Heavy");
        }

        if (animator.GetBool("IsAttacking"))
        {
            canMove = false;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
        else
        {
            canMove = true;
        }

            Jump();
    }

    private void FixedUpdate()
    {
        //Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        //rb.AddForce(gravity, ForceMode.Acceleration);

        if (rb.linearVelocity.y >= peak || rb.linearVelocity.y <= -peak)
        {
            rb.AddForce(-transform.up * extraGravity);
        }

        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (canMove)
        {
            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);

                Vector3 moveVelocity = movement.normalized * playerSpeed;
                rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

                animator.SetBool("Walking", true);
            }
            else
            {

                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                animator.SetBool("Walking", false);
            }
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            animator.SetBool("Walking", false);
        }


        WalkingEffect();
    }

    void Jump()
    {
        if (jumpBufferTimer > 0f && jumpCount > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            jumpCount--;
            jumpBufferTimer = 0f;
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