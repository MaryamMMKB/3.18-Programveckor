using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float smoothing = 5f; // Smoothing for more cinematic camera feel
    
    [Header("Camera Limits")]
    [SerializeField] private float minVerticalAngle = -90f;
    [SerializeField] private float maxVerticalAngle = 90f;
    
    [Header("Options")]
    [SerializeField] private bool invertYAxis = false;
    
    [Header("References")]
    [SerializeField] private Transform playerBody;
    
    // Private variables
    private float xRotation = 0f;
    private Vector2 currentMouseDelta;
    private Vector2 targetMouseDelta;
    
    void Start()
    {
        // Lock and hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleCursorToggle();
    }
    
    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Apply Y-axis inversion if enabled
        if (invertYAxis)
        {
            mouseY = -mouseY;
        }
        
        // Store target mouse delta
        targetMouseDelta = new Vector2(mouseX, mouseY);
        
        // Smooth the mouse movement for more cinematic feel
        currentMouseDelta = Vector2.Lerp(currentMouseDelta, targetMouseDelta, smoothing * Time.deltaTime);
        
        // Calculate vertical rotation (X-axis rotation for camera)
        xRotation -= currentMouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        
        // Apply rotation to camera (vertical look)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Apply rotation to player body (horizontal look)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * currentMouseDelta.x);
        }
    }
    
    private void HandleCursorToggle()
    {
        // Toggle cursor lock with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    
    // Public methods for external control
    public void SetSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
    
    public void SetInvertY(bool invert)
    {
        invertYAxis = invert;
    }
    
    public void SetSmoothing(float smooth)
    {
        smoothing = smooth;
    }
}
