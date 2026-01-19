using UnityEngine;

public class Torreta1 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;

    [Header("Patrol Points")]
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;

    private Vector3 targetPosition;



    public GameObject ProjectilePrefab;
    public float shootInterval = 1.5f;
    public Transform firePoint;
    private Rigidbody2D rb;




    void Start()
    {
        targetPosition = topPoint.position;
        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }


    void Shoot()
    {
        GameObject proj = Instantiate(
            ProjectilePrefab,   
            firePoint.position,
            Quaternion.identity
        );

        BalaEnemigo2 projectile = proj.GetComponent<BalaEnemigo2>();

        Vector2 direction = Vector2.left;

        projectile.SetTarget(transform);
        projectile.SetDirection(direction);

    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            targetPosition = targetPosition == topPoint.position
                ? bottomPoint.position
                : topPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bala"))
        {
            Destroy(gameObject);
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (topPoint == null || bottomPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(topPoint.position, bottomPoint.position);

        Gizmos.DrawSphere(topPoint.position, 0.1f);
        Gizmos.DrawSphere(bottomPoint.position, 0.1f);
    }
#endif
}