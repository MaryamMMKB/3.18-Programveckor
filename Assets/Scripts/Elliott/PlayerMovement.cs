using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    
    [Header("Sluggish Movement Feel")]
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float momentumDrag = 0.85f; // How much momentum carries over (lower = more sluggish)
    
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
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
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
            velocity.y = -2f; // Small negative value to keep grounded
        }
    }
    
    private void HandleMovement()
    {
        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // Calculate desired movement direction
        Vector3 moveDirection = transform.right * x + transform.forward * z;
        moveDirection.Normalize();
        
        // Calculate target movement with walk speed
        targetMovement = moveDirection * walkSpeed;
        
        // Sluggish acceleration/deceleration with momentum
        float accelerationRate = moveDirection.magnitude > 0.1f ? acceleration : deceleration;
        currentMovement = Vector3.Lerp(currentMovement, targetMovement, accelerationRate * Time.deltaTime);
        
        // Apply momentum drag for more sluggish feel
        if (moveDirection.magnitude < 0.1f)
        {
            currentMovement *= momentumDrag;
        }
        
        // Add vertical velocity for gravity
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
            // Smooth transition to target bob amount
            currentBobAmount = Mathf.Lerp(currentBobAmount, bobAmplitude, bobSmoothness * Time.deltaTime);
            
            // Update bob timer
            bobTimer += Time.deltaTime * bobFrequency * (horizontalSpeed / walkSpeed);
            
            // Calculate bob offset
            float bobOffset = Mathf.Sin(bobTimer) * currentBobAmount;
            
            // Apply bob to camera
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
            // Smoothly return to rest position when not moving
            currentBobAmount = Mathf.Lerp(currentBobAmount, 0, bobSmoothness * Time.deltaTime);
            bobTimer = 0;
            
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                cameraStartPos,
                bobSmoothness * Time.deltaTime
            );
        }
    }
    
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }
    
    // Public method for external access
    public float GetCurrentSpeed()
    {
        return new Vector3(currentMovement.x, 0, currentMovement.z).magnitude;
    }
}
