using UnityEngine;
using UnityEngine.InputSystem; // ✅ new input system

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;

        // ✅ Replace old Input.GetAxis with Keyboard.current checks
        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move.y = 0; // Prevent vertical movement
        move = move.normalized;
        rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
    }
}
