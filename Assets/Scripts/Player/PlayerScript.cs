using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Quaternion newRotation;
    public Transform cameraRot;
    [HideInInspector] public Vector3 input;
    public Vector3 destination;
    public float force = 200f;
    public float maxForce = 200f;
    public float movementSpeed = 4.5f;
    private float cooldown = 0.36f;
    private Animator animator;

    public ParticleSystem particleDash;
    public ParticleSystem particleRunning;
    public bool particleOn;

    private bool cooldownActive = false;
    private bool ableToDash = true;
    private bool buildUp;

    public Rigidbody rb;
    RaycastHit hit;
    public bool isGrounded;
    private bool prevGrounded;
    public float lookSpeed = 0.15f;
    public CapsuleCollider playerCollider;
    public float distanceToGround;
    public LayerMask layerMask;
    public bool allowedControl = true;
    public bool testBool;
    private DashCooldown cooldownScript;

    public float inpVelocity;
    public float inpVelocityZ;
    public float inpVelocityX;
    void Start()
    {
        animator = GetComponent<Animator>();
        particleRunning.Stop();
        particleDash.Stop();
        cooldownScript = GetComponent<DashCooldown>();
        // Gets collider component
        playerCollider = GetComponent<CapsuleCollider>();
        // Gets rigidbody component
        rb = GetComponent<Rigidbody>();
        distanceToGround = playerCollider.bounds.extents.y + 0.00001f;

    }
    void Update()
    {
        animator.SetBool("testBool", testBool);
        Debug.Log(animator.speed);
        if (allowedControl)// If allowed control
        {
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); // Gets input on X and Z axis
            animator.SetFloat("speed",input.magnitude);
            if (Input.GetButtonDown("Dash") && input != Vector3.zero)
            {
                Dash();
                Debug.Log("You have dashed");
            }
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, layerMask)) // Sending raycast below players feet
            {
                prevGrounded = isGrounded;
                isGrounded = true;
            }
            else
            {
                prevGrounded = isGrounded;
                isGrounded = false;
            }
            // Activate if player input != null
            if (input == Vector3.zero)
            {
                particleOn = false;
            }
            else
                particleOn = true;

            if (particleOn)
            {
                CreateRunningParticles();
            }
            else if (!particleOn)
            {
                if (particleRunning.isPlaying)
                {
                    particleRunning.Stop();
                }

            }
        }



        if (!allowedControl)
        {
            movementSpeed = 0f; // Paralyzes player
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
        else
        {
            movementSpeed = 4.5f; // Resumes 
            //Debug.Log(cooldownActive);
        }
    }
    private void FixedUpdate()
    {
        Move(input);
        //Here


        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -5f, 5f), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -5f, 5f));

        //if (rb.velocity.y > jumpForce)
        //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);


        inpVelocity = rb.velocity.y;
        inpVelocityZ = rb.velocity.z;
        inpVelocityX = rb.velocity.x;
    }
    // Make player move with inputs
    public void Move(Vector3 input)
    {
        //destination = rb.position + transform.TransformDirection(input * movementSpeed * Time.deltaTime);
        //rb.MovePosition(destination);

        rb.MovePosition(rb.position + input * movementSpeed * Time.deltaTime);
    }
    private void Dash() // Adds force towards the input direction
    {
        if (!cooldownActive && cooldownScript.dashesLeft > 0 )
        {
            rb.AddForce(transform.forward + Vector3.ClampMagnitude(new Vector3(force * Mathf.Round(input.x), 0f, force * Mathf.Round(input.z)), maxForce));
            
            //if (particleDash.isStopped && particleRunning.isPlaying)
            
            particleRunning.Stop();
            particleDash.Play();
            particleRunning.Play();
            cooldownScript.ResetDash();

            if (!buildUp)
            {
                StartCoroutine(DashCoolDown(cooldown));
            }
        }
    }
    private IEnumerator DashCoolDown(float duration)
    {
            ableToDash = false;
            cooldownActive = true;
            yield return new WaitForSeconds(duration);
            cooldownActive = false;
            ableToDash = true;
    }
    void CreateRunningParticles()
    {
        if (particleRunning.isStopped)
        {
            particleRunning.Play();
        }
        //Debug.Log("running");
    }
    private void Jump()
    {
        //if (rb.velocity.y < jumpForce)
        //rb.AddForce(0f, jumpForce + 1, 0f, ForceMode.VelocityChange);
    }
}
