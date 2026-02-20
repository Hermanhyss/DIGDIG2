using System.Collections;
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
    bool isRunning;

    [SerializeField] GameObject walkingEffect;
    ParticleSystem walkingParticleSystem;

    [SerializeField] float jumpBufferTime = 0.2f;
    float jumpBufferTimer;

    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask Ground;
    Rigidbody rb;
    public float maxDistance = 2;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.2f;

    bool canMove = true;
    Animator animator;
    ChromaticPulse chromaticPulse;
    ChromaticVignettePulse chromaticVignettePulse;

    [SerializeField] float comboTimeFrame;
    int numberOfAttack;
    bool ActivateComboTimer;


    bool canAttack;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        chromaticPulse = FindFirstObjectByType<ChromaticPulse>();
        chromaticVignettePulse = FindFirstObjectByType<ChromaticVignettePulse>();
        if (walkingEffect != null)
            walkingParticleSystem = walkingEffect.GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        canAttack = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, Ground);

        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0,-1,0), transform.TransformDirection(Vector3.down), out hit, maxDistance, Ground))
        {
            Debug.DrawRay(transform.position + new Vector3(0,-1,0), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            animator.SetBool("Landing", true);
        }
        else
        {
            animator.SetBool("Landing", false);
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
        Running();
        AttackCombo();
    }

    private void FixedUpdate()
    {
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
        if (jumpBufferTimer > 0f && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            jumpBufferTimer = 0f;
        }
    }

    void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 4f;
            animator.speed = 3.5f;
            isRunning = true;

            if (!isGrounded)
            {
                animator.speed = 1f;
            }
        }
        else
        {
            playerSpeed = 1.5f;
            animator.speed = 1f;
            isRunning = false;
        }
    }

    void AttackCombo()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StopCoroutine(AttackComboCor());
            StartCoroutine(AttackComboCor());
        }
    }

    IEnumerator AttackComboCor()
    {
        ActivateComboTimer = true;
       
        numberOfAttack += 1;
        

        if (numberOfAttack == 1 && canAttack)
        {
            animator.Play("Attack light");
        }
        
        if(numberOfAttack == 2 && canAttack){
            animator.Play("Attack light 2");
        }

        canAttack = false;
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
        yield return new WaitForSeconds(0.5f);

        numberOfAttack = 0;
        ActivateComboTimer = false;
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