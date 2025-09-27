using UnityEngine;
using UnityEngine.InputSystem;


/// Handles mouse look functionality for first-person camera control.
/// This script should be attached to the camera and controls both vertical (pitch) and horizontal (yaw) rotation.

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    [Tooltip("Reference to the player's body transform for horizontal rotation")]
    public Transform playerBody;
    
    [Tooltip("Mouse sensitivity multiplier - higher values = faster camera movement")]
    public float mouseSensitivity = 100f;
    
    // Tracks the current vertical rotation (pitch) to prevent over-rotation
    float xRotation = 0f;

    
    /// Initialize mouse look system - locks cursor to center of screen for FPS gameplay
    
    void Start()
    {
        // Lock cursor to center of screen and make it invisible for FPS experience
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    /// Updates camera rotation based on mouse input every frame
    
    void Update()
    {
        // Get mouse movement delta using new Input System
        // Multiply by sensitivity and deltaTime for consistent movement regardless of framerate
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        // Handle vertical rotation (looking up/down) - applied to camera
        // Subtract mouseY because mouse Y-axis is typically inverted for camera control
        xRotation -= mouseY;
        // Clamp vertical rotation to prevent camera from flipping over (90° up, 90° down)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply vertical rotation to the camera (this transform)
        // Only rotate around X-axis for pitch, keep Y and Z at 0
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Handle horizontal rotation (looking left/right) - applied to player body
        // This rotates the entire player so movement direction follows camera direction
        playerBody.Rotate(Vector3.up * mouseX);
    }
}