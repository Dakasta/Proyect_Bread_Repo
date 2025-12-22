using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{


    [SerializeField] private GameObject sword;
    [SerializeField] private float attackDuration = 0.2f;

    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight;


    [Header("GroundCheck Configuration")]
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Respawn Configuration")]
    [SerializeField] Transform respawnPoint;
     Rigidbody2D rb;
    Vector2 moveInput;

    
   


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;

      



    }

     void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
       
        Flip();
    }

    private void FixedUpdate()
    {
        Movement();
    }
    private IEnumerator AttackCoroutine()
    {
        float direction = isFacingRight ? 1f : -1f;
        sword.transform.localPosition = new Vector3(0.6f * direction, 0.2f, 0);

        sword.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        sword.SetActive(false);
    }
    void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }
    void Respawn()
    {
        transform.position = respawnPoint.position;

    }
    void Movement()
    {
       rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

    }

   void Flip()
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }


    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Attack();
        }
    }

    #endregion

}