using UnityEngine;

public class BalaEnemigo : MonoBehaviour
{
    public float speed = 8f;
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


    public int damage = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {

            Vector2 direction = (collision.transform.position - transform.position);
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, direction);
            Destroy(gameObject);
        }

        if (collision.CompareTag("enemigo"))
        {
            Destroy(gameObject);
        }
        // Si la espada golpea la bola
        if (collision.CompareTag("Sword"))
        {
            if (cubeTarget != null)
            {
                Vector2 directionToCube =
                    (cubeTarget.position - transform.position).normalized;

                SetDirection(directionToCube);

                Destroy(gameObject, 5f);


            }
           

        }
    }
}