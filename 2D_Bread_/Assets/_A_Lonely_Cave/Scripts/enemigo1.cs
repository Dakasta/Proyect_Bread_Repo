using UnityEngine;

public class enemigo1 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 1.5f;
    public Transform firePoint;
    private Rigidbody2D rb;
    void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    void Shoot()
    {
        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

       BalaEnemigo projectile = proj.GetComponent<BalaEnemigo>();

        // Dirección fija hacia la IZQUIERDA
        Vector2 direction = Vector2.left;

        projectile.SetTarget(transform);
        projectile.SetDirection(direction);
    }
}
