eusing System;
using UnityEngine;

public class BalaEnemigo2 : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;
    private Transform cubeTarget;
    public Transform Torreta;
    public Boolean Parry = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, 4f);
    }


    void Update()
    {
        if (Parry == true)
        {

            Parryada();
            

        }
    }

    public void SetTarget(Transform target)
    {
        cubeTarget = target;
    }



    private void Parryada()
    {
        float offsetX = 0.1f;


        Vector2 objetivo = (Vector2)Torreta.position + Vector2.right * offsetX;


        float distancia = Vector2.Distance(transform.position, objetivo);


        transform.position = Vector2.MoveTowards(transform.position, objetivo, speed * Time.deltaTime);


     
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Torreta1"))
        {
            Destroy(gameObject);
        }
        // Si la espada golpea la bola
        if (collision.CompareTag("Sword"))
        {
            speed = 15f;

            Parry = true;

                // Un método personalizado

               
            // Torreta = GameObject.FindGameObjectWithTag("Torreta1").transform;

            // float distancia = Vector2.Distance(transform.position, Torreta.position);
            // transform.position = Vector2.MoveTowards(transform.position, Torreta.position, speed * Time.deltaTime);


            // Vector2 directionToCube =
            //  (cubeTarget.position - transform.position).normalized;

            //SetDirection(directionToCube);

            Destroy(gameObject, 5f);


            


        }
    }
}