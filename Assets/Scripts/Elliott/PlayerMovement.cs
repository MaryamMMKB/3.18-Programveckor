using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;

    [Header("Sluggish Movement Feel")]
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float momentumDrag = 0.85f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Head Bob Settings")]
    [SerializeField] private bool enableHeadBob = true;
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float bobSmoothness = 10f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Footstep Audio")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.5f; // Time between steps at walkSpeed

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentMovement;
    private Vector3 targetMovement;
    private bool isGrounded;

    // Head bob variables
    private float bobTimer;
    private Vector3 cameraStartPos;
    private float currentBobAmount;

    // Footstep variables
    private AudioSource audioSource;
    private float footstepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (cameraTransform != null)
        {
            cameraStartPos = cameraTransform.localPosition;
        }
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        HandleHeadBob();
        HandleFootsteps();
        ApplyGravity();

        // Apply final movement
        controller.Move(currentMovement * Time.deltaTime);
    }

    private void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            isGrounded = controller.isGrounded;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        moveDirection.Normalize();

        targetMovement = moveDirection * walkSpeed;

        float accelerationRate = moveDirection.magnitude > 0.1f ? acceleration : deceleration;
        currentMovement = Vector3.Lerp(currentMovement, targetMovement, accelerationRate * Time.deltaTime);

        if (moveDirection.magnitude < 0.1f)
        {
            currentMovement *= momentumDrag;
        }

        currentMovement = new Vector3(currentMovement.x, velocity.y, currentMovement.z);
    }

    private void HandleHeadBob()
    {
        if (!enableHeadBob || cameraTransform == null || !isGrounded)
        {
            return;
        }

        float horizontalSpeed = new Vector3(currentMovement.x, 0, currentMovement.z).magnitude;

        if (horizontalSpeed > 0.1f)
        {
            currentBobAmount = Mathf.Lerp(currentBobAmount, bobAmplitude, bobSmoothness * Time.deltaTime);
            bobTimer += Time.deltaTime * bobFrequency * (horizontalSpeed / walkSpeed);
            float bobOffset = Mathf.Sin(bobTimer) * currentBobAmount;
            Vector3 targetPos = cameraStartPos;
            targetPos.y += bobOffset;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                targetPos,
                bobSmoothness * Time.deltaTime
            );
        }
        else
        {
            currentBobAmount = Mathf.Lerp(currentBobAmount, 0, bobSmoothness * Time.deltaTime);
            bobTimer = 0;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                cameraStartPos,
                bobSmoothness * Time.deltaTime
            );
        }
    }

    private void HandleFootsteps()
    {
        float horizontalSpeed = new Vector3(currentMovement.x, 0, currentMovement.z).magnitude;

        if (isGrounded && horizontalSpeed > 0.1f)
        {
            footstepTimer -= Time.deltaTime * (horizontalSpeed / walkSpeed); // Faster when running
            if (footstepTimer <= 0f && footstepClips.Length > 0)
            {
                AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
                audioSource.PlayOneShot(clip);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    public float GetCurrentSpeed()
    {
        return new Vector3(currentMovement.x, 0, currentMovement.z).magnitude;
    }
}
