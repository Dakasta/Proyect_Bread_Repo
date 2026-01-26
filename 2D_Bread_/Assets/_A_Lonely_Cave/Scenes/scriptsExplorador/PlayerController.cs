using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float linearDrag = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Better Jump")]
    [SerializeField] private float fallMultiplier = 3.5f;
    [SerializeField] private float lowJumpMultiplier = 2.2f;

    [Header("Jump Assist")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpHeld;

    private float coyoteCounter;
    private float jumpBufferCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();
        HandleTimers();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        ApplyLinearDrag();
        BetterJump();
        TryJump();
    }

    // -------- INPUT SYSTEM --------

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpHeld = true;
            jumpBufferCounter = jumpBufferTime;
        }

        if (context.canceled)
        {
            jumpHeld = false;
        }
    }

    // -------- MOVEMENT --------

    private void MoveCharacter()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(
                Mathf.Sign(rb.linearVelocity.x) * maxSpeed,
                rb.linearVelocity.y
            );
        }
    }

    private void ApplyLinearDrag()
    {
        rb.linearDamping = Mathf.Abs(moveInput.x) < 0.1f ? linearDrag : 0f;
    }

    // -------- JUMP LOGIC --------

    private void TryJump()
    {
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // -------- BETTER JUMP (GRAVEDAD) --------

    private void BetterJump()
    {
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                           * (fallMultiplier - 1f)
                           * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                           * (lowJumpMultiplier - 1f)
                           * Time.fixedDeltaTime;
        }
    }

    // -------- GROUND / TIMERS --------

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded)
            coyoteCounter = coyoteTime;
    }

    private void HandleTimers()
    {
        if (!isGrounded)
            coyoteCounter -= Time.deltaTime;

        jumpBufferCounter -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
