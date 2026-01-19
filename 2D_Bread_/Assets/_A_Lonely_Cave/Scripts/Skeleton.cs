using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public int direccion;

    public float speed_walk;
    public float speed_run;

    public GameObject target;
    public bool atacando;

    public float rango_vision;
    public float rango_ataque;

    public GameObject rango;
    public GameObject Hit;
    public int damage = 1;
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        Comportamientos();
    }

    public void Comportamientos()
    {
        float distancia = Mathf.Abs(transform.position.x - target.transform.position.x);

        // =========================
        // PATRULLA
        // =========================
        if (distancia > rango_vision && !atacando)
        {
            ani.SetBool("AtackS", false);
            ani.SetBool("WalkS", false);

            cronometro += Time.deltaTime;

            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    ani.SetBool("WalkS", false);
                    break;

                case 1:
                    direccion = Random.Range(0, 2);
                    rutina = 2;
                    break;

                case 2:
                    ani.SetBool("WalkS", true);

                    if (direccion == 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }

                    transform.Translate(Vector3.right * speed_walk * Time.deltaTime);
                    break;
            }
        }
      
      
        else
        {
            // PERSEGUIR
            if (distancia > rango_ataque && !atacando)
            {
                ani.SetBool("WalkS", true);
                ani.SetBool("AtackS", false);

                if (transform.position.x < target.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                transform.Translate(Vector3.right * speed_run * Time.deltaTime);
            }
            // ATACAR
            else
            {
                if (!atacando)
                {
                    atacando = true;
                    ani.SetBool("WalkS", false);
                    ani.SetBool("AtackS", true);


                }
            }
        }
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
                player.Damage(direccion, damage);
            }

          
        }
    }

    public void Final_Ani()
    {
        ani.SetBool("AtackS", false);
        atacando = false;
        rango.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponTrue()
    {
        Hit.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        Hit.GetComponent<BoxCollider2D>().enabled = false;
    }
}
