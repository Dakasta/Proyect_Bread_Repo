
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// Se eliminó NUnit para evitar errores en el Build
public class Player : MonoBehaviour
{
    [SerializeField] public GameObject sword;
    [SerializeField] private float attackDuration = 0.2f;

    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight;

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
    public bool atak = false;

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
            // Corregido: Ahora la espada sale a la izquierda (-0.6f)
            swordPosition = new Vector3(+0.6f, 0.2f, 0);
            //swordRotation = Quaternion.Euler(0, 0, 180);
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
                //muelto = false;
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
