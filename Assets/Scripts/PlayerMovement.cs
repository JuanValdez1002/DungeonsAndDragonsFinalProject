using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Reference to the CharacterController component (handles collisions & movement)
    public CharacterController controller;

    // Walking and crouching speeds
    public float speed = 12f;
    public float crouchSpeed = 6f; // move slower when crouching

    // Gravity strength (multiplied for stronger fall effect)
    public float gravity = -9.81f * 2;

    // How high the player can jump
    public float jumpHeight = 3f;

    // Used for checking if the player is standing on the ground
    public Transform groundCheck;      // empty object under player (usually at feet)
    public float groundDistance = 0.4f; // size of sphere used to detect ground
    public LayerMask groundMask;        // defines which layers count as "ground"

    // Internal variables to track movement and jumping
    private Vector3 velocity;  // current player velocity (for gravity)
    private bool isGrounded;   // true if player is on the ground
    private bool isCrouching = false; // true when crouching

    // Crouching settings
    private float originalHeight;   // saves the standing height
    public float crouchHeight = 1f; // height when crouching

    void Start()
    {
        // Save the default height of the CharacterController when game starts
        originalHeight = controller.height;
    }

    void Update()
    {
        if (GameManager.IsGameOver) return;

        // ----------- GROUND CHECK -----------
        // Create an invisible sphere below the player to check if they are standing on ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);
        

        // If the player is touching the ground and falling, reset the fall velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // small downward push keeps player grounded
        }

        // ----------- MOVEMENT INPUT -----------
        // "Horizontal" = A/D or Left/Right
        // "Vertical" = W/S or Up/Down
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create a movement direction relative to where the player is facing
        // transform.right = left/right direction
        // transform.forward = forward/backward direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Use slower speed if crouching
        float currentSpeed = isCrouching ? crouchSpeed : speed;

        // Apply the movement to the CharacterController (multiplied by Time.deltaTime for smooth movement)
        controller.Move(move * currentSpeed * Time.deltaTime);

        // ----------- JUMPING -----------
        // Allow jumping only when player is grounded and not crouching
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            // Calculate jump velocity using physics formula
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ----------- GRAVITY -----------
        // Continuously apply gravity every frame (so player falls)
        velocity.y += gravity * Time.deltaTime;

        // Apply gravity to movement
        controller.Move(velocity * Time.deltaTime);

        // ----------- CROUCH INPUT -----------
        // Press Left Ctrl to crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        // Release Left Ctrl to stand back up
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            UnCrouch();
        }
    }

    // Called when crouching starts
    void Crouch()
    {
        isCrouching = true;
        controller.height = crouchHeight; // reduce height of the character
    }

    // Called when crouching ends
    void UnCrouch()
    {
        isCrouching = false;
        controller.height = originalHeight; // restore original height
    }
}
