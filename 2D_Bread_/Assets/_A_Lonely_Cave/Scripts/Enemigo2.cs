using UnityEngine;

public class Enemigo2 : MonoBehaviour
{
    public float velocidad = 8f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Ground"))
        {
           
            Destroy(gameObject, 1f);
        }
    }
    void FixedUpdate()
    { 
        rb.linearVelocity = Vector2.down * velocidad;
    }





}