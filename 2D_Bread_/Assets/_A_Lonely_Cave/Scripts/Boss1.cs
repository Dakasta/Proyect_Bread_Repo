using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb2D;
    public GameObject jugador;
    private bool mirandoDerecha = true;

    [Header("Vida")]
    [SerializeField] public float vida = 6;

    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 2f; // Nueva variable para controlar la rapidez

    [Header("Ataque")]
    [SerializeField] private Transform AtaqueJefe;
    [SerializeField] private float radioAtaque;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        jugador = GameObject.Find("Player");
    }

    private void Update()
    {
        // 1. Calculamos la distancia correctamente
        float Distancia = Vector2.Distance(transform.position, jugador.transform.position);
        animator.SetFloat("Distancia", Distancia);

        // 2. Lógica de Seguimiento y Giro
        MirarJugador();

        // 3. Movimiento físico hacia el jugador
        // Solo se mueve si está fuera del rango de ataque
        if (Distancia > radioAtaque)
        {
            // Determinamos la dirección (1 para derecha, -1 para izquierda)
            float direccion = (jugador.transform.position.x > transform.position.x) ? 1 : -1;

            // Aplicamos la velocidad al Rigidbody2D
            rb2D.linearVelocity = new Vector2(direccion * velocidadMovimiento, rb2D.linearVelocity.y);
        }
        else
        {
            // Se detiene cuando llega al jugador para atacar
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
        }
    }

    public void TomarDano()
    {
        vida--;
        if (vida <= 0)
        {
            animator.SetTrigger("Muerte");
        }
    }

    private void Muerte()
    {
        Destroy(gameObject);
    }

    public void MirarJugador()
    {
        if ((jugador.transform.position.x > transform.position.x && !mirandoDerecha) ||
            (jugador.transform.position.x < transform.position.x && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    public void Ataque()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(AtaqueJefe.position, radioAtaque);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (AtaqueJefe != null)
            Gizmos.DrawWireSphere(AtaqueJefe.position, radioAtaque);
    }
}