using System.Collections;
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
    [SerializeField] bool Dano;
    [SerializeField] int Vida = 3;
    [SerializeField] bool muelto = false;
    [Header("GroundCheck Configuration")]
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Respawn Configuration")]
    [SerializeField] Transform respawnPoint;

    Rigidbody2D rb;
    Vector2 moveInput;
    Vector2 attackDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        sword.SetActive(false);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        Flip();
        UpdateAttackDirection();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // 🔹 Determina la dirección del ataque
    void UpdateAttackDirection()
    {
        if (moveInput != Vector2.zero)
        {
            attackDirection = moveInput.normalized;
        }
        else
        {
            attackDirection = isFacingRight ? Vector2.right : Vector2.left;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        Vector3 swordPosition = Vector3.zero;

        if (attackDirection.y > 0.5f) // Arriba
        {
            swordPosition = new Vector3(0f, 0.6f, 0);
        }
        else if (attackDirection.y < -0.5f) // Abajo
        {
            swordPosition = new Vector3(0f, -0.6f, 0);
        }
        else if (attackDirection.x > 0) // Derecha
        {
            swordPosition = new Vector3(0.6f, 0.2f, 0);
        }
        else if (attackDirection.x < 0) // Izquierda
        {
            swordPosition = new Vector3(0.6f, 0.2f, 0); 
        }

        sword.transform.localPosition = swordPosition;
        sword.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        sword.SetActive(false);
    }

    void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemmyAtack"))
        {
            Vector2 direccion = collision.transform.position;
            Damage(direccion, 1);
        }
    }
    public void Damage(Vector2 direccion, int cantDano)
    {

        Debug.Log("Vida actual: " + Vida);
        if (!Dano)
        {
            Dano = true;
            Vida -= cantDano;
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
        rb.AddForce(rebote, ForceMode2D.Impulse);
        }

        if(Vida <= 0)
        {

            muelto = true;
            Respawn();
        }
        StartCoroutine(Invulnerabilidad());
    }
    IEnumerator Invulnerabilidad()
    {
        yield return new WaitForSeconds(0.5f);
        Dano = false;
    }




    public void Respawn()
    {
        transform.position = respawnPoint.position;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
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
        else if (moveInput.x < 0 && isFacingRight)
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
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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