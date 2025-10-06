using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    // Controls how sensitive the mouse movement feels
    public float mouseSensitivity = 100f;

    // Reference to the player's body (the parent object with the CharacterController)
    // This allows the mouse to rotate the entire player left/right
    public Transform playerBody;

    // Tracks the camera's up/down rotation (pitch)
    // We store it so we can clamp it later to prevent over-rotation
    float xRotation = 0f;

    void Start()
    {
        // Lock the mouse cursor to the center of the screen
        // This prevents the cursor from moving outside the game window
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- Get mouse input ---

        // Get horizontal mouse movement (X-axis) — used to rotate left/right
        // Get vertical mouse movement (Y-axis) — used to look up/down
        // Multiply by sensitivity and Time.deltaTime to make movement frame-rate independent
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // --- Adjust camera rotation (up/down) ---

        // Subtract mouseY because moving the mouse up should rotate the camera down (natural feel)
        xRotation -= mouseY;

        // Clamp the rotation so the player can't look too far up or down (prevents flipping)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the vertical rotation to the camera (this GameObject)
        // Only rotates the camera's local X-axis, not the entire player
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // --- Rotate player (left/right) ---

        // Rotate the player horizontally (around the Y-axis)
        // This affects movement direction since the player body actually rotates
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
