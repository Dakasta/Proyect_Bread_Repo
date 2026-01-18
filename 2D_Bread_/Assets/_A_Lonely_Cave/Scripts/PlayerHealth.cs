using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 6f;

    private Rigidbody2D rb;
    private Player player;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("No se encontró el script Player en este GameObject");
        }
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;

        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        Debug.Log("Vida actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jugador muerto");

        currentHealth = maxHealth;

        player.Respawn();
    }
}
