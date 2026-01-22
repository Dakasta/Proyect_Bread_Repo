using UnityEngine;

public class BalaEnemigo : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;

    private Rigidbody2D rb;
    private Transform cubeTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, 4f);
    }

    public void SetTarget(Transform target)
    {
        cubeTarget = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🔴 GOLPEA AL PLAYER
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                Vector2 direccion = transform.position;
               
            }

            Destroy(gameObject);
        }

       

        // 🟢 REBOTAR CON LA ESPADA
        if (collision.CompareTag("Sword"))
        {
            if (cubeTarget != null)
            {
                Vector2 directionToCube =
                    (cubeTarget.position - transform.position).normalized;

                SetDirection(directionToCube);
            }
        }
    }
}
