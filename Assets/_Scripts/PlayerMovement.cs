using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeSpeed = 10f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private float dodgeCooldown = 1f;


    [Header("Look Settings")]
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator playerAnimator;
    
    private CharacterController characterController;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isCrouching = false;
    private bool isDodging = false;
    private float lastDodgeTime = -999f;

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            enabled = false;
            return;
        }
    }
    
    private void Update()
    {
        HandleRotation();

        if (!isDodging) 
        {
            HandleMovement();  
            HandleDodge();     
        }
    }
    private void HandleMovement()
    {
        bool isGrounded = IsGrounded();
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            playerAnimator.SetBool("Crouch", isCrouching);

            characterController.height = isCrouching ? 1f : 2f;
            characterController.center = isCrouching ? new Vector3(0, 0.5f, 0) : new Vector3(0, 1f, 0);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        playerAnimator.SetFloat("Forward", vertical);
        playerAnimator.SetFloat("Right", horizontal);
        playerAnimator.SetBool("Jump", !isGrounded);
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.03f, Vector3.down, groundCheckDistance);
    }

    private void HandleDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && Time.time >= lastDodgeTime + dodgeCooldown)
        {
            StartCoroutine(PerformDodge());
        }
    }

    private IEnumerator PerformDodge()
    {
        isDodging = true;
        lastDodgeTime = Time.time;

        playerAnimator.SetTrigger("Dodge");

        yield return new WaitForSeconds(.6f);

        Vector3 dodgeDirection = transform.forward;
        float startTime = Time.time;

        while (Time.time < startTime + dodgeDuration)
        {
            characterController.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);
            yield return null; 
        }

        isDodging = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.03f, Vector3.down * groundCheckDistance);
    }
#endif
}