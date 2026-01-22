using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject sword;
    [SerializeField] private float attackDuration = 0.2f;

    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight;

 
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
    public bool atak = false;
    public bool estaDasheando;
    public float Vida = 3;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        sword.GetComponent<BoxCollider2D>().enabled = false;
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
        // CAMBIO AQUÍ: Si Dash es true, hace return y no ejecuta Movement
        if (Dash())
        {
            return;
        }

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
            swordPosition = new Vector3(+0.6f, 0.2f, 0);
        }

        sword.transform.localPosition = swordPosition;
        sword.GetComponent<BoxCollider2D>().enabled = true;
        sword.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        sword.GetComponent<BoxCollider2D>().enabled = false;
        sword.SetActive(false);
    }

    void Attack()
    {
        StartCoroutine(DesactivarAtaque(0.3f));
        StartCoroutine(AttackCoroutine());
        atak = true;
    }

    IEnumerator DesactivarAtaque(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        atak = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemmyAtack"))
        {
            if (!atak)
            {
                Vida -= 1;
                Debug.Log("Vida actual: " + Vida);
            }
            else
            {
                Debug.Log("MI BOMBOOOOO ");
            }
        }

        if (Vida <= 0)
        {
            muelto = true;
            if (muelto == true)
            {
                Respawn();
            }
        }
    }

    IEnumerator Invulnerabilidad()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void Respawn()
    {
        SceneManager.LoadScene("LVL_Hectorr");
    }

    void Movement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    // CAMBIO AQUÍ: Cambiado de 'void' a 'bool' para que el if de arriba funcione
    private bool Dash()
    {
        if (estaDasheando)
        {
            float dashSpeed = 15f;
          

            rb.linearVelocity = new Vector2(moveInput.x * dashSpeed, rb.linearVelocity.y );
            return true; // Indica que se está haciendo dash
        }

        return false; // Indica que no se está haciendo dash
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Para que esto funcione, en algún momento debes poner 
            // estaDasheando = true y luego false con una corrutina.
            estaDasheando = true;
            StartCoroutine(StopDash(0.2f)); // Ejemplo para que el dash termine
        }
    }

    // Corrutina auxiliar para que el dash no sea infinito
    IEnumerator StopDash(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        estaDasheando = false;
    }

    #endregion
}
