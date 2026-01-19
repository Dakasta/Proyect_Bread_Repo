using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Variables de Movimiento")]
    public int rutina;
    public float cronometro;
    public Animator ani;
    public int direccion;
    public float speed_walk;
    public float speed_run;

    [Header("Variables de Combate")]
    public GameObject target;
    public bool atacando;
    public float rango_vision;
    public float rango_ataque;
    public bool isStunned = false;
    public GameObject rango;
    public GameObject Hit; // El collider del arma
    public int damage = 1;
    public bool puedeHacerDano = true;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
        if (Hit != null) Hit.GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        // Si está aturdido, no procesa movimiento ni ataques
        if (isStunned)
        {
            return;
        }

        Comportamientos();
    }

    public void Comportamientos()
    {
        float distancia = Mathf.Abs(transform.position.x - target.transform.position.x);

        // PATRULLA
        if (distancia > rango_vision && !atacando)
        {
            ani.SetBool("AtackS", false);
            ani.SetBool("WalkS", false);
            ani.SetBool("ParryedS", false);

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
                    transform.rotation = Quaternion.Euler(0, (direccion == 0) ? 0 : 180, 0);
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
                ani.SetBool("ParryedS", false);

                if (transform.position.x < target.transform.position.x)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                transform.Translate(Vector3.right * speed_run * Time.deltaTime);
            }
            // ATACAR
            else if (!atacando)
            {
                atacando = true;
                ani.SetBool("WalkS", false);
                ani.SetBool("AtackS", true);
                ani.SetBool("ParryedS", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // EL ENEMIGO HACE DAÑO AL PLAYER
        // Corregido: usamos == para comparar, y revisamos que no esté aturdido
        if (collision.CompareTag("Player") && puedeHacerDano == true && isStunned == false)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(transform.position, damage);
            }
        }

        // EL PLAYER GOLPEA AL ENEMIGO CON LA ESPADA
        if (collision.CompareTag("Sword"))
        {
            Stop(); // Llama a la parálisis
        }
    }

    public void Stop()
    {
        // Detenemos cualquier parálisis previa para que el tiempo se reinicie si le sigues pegando
        StopAllCoroutines();
        StartCoroutine(PausarEnemigo());
    }

    IEnumerator PausarEnemigo()
    {
        isStunned = true;
        puedeHacerDano = false;
        atacando = false;

        // Desactivamos su arma inmediatamente para que no te dañe parado
        if (Hit != null) Hit.GetComponent<BoxCollider2D>().enabled = false;

        // Activamos animación de parálisis y apagamos las demás
        ani.SetBool("WalkS", false);
        ani.SetBool("AtackS", false);
        ani.SetBool("ParryedS", true);

        Debug.Log("Enemigo aturdido por 1 segundo");

        yield return new WaitForSeconds(1f); // Espera el segundo

        // Volver a la normalidad
        isStunned = false;
        puedeHacerDano = true;
        ani.SetBool("ParryedS", false);
        Debug.Log("Enemigo recuperado");
    }

    // EVENTOS DE ANIMACIÓN
    public void Final_Ani()
    {
        // Solo resetear ataque si no ha sido interrumpido por un stun
        if (!isStunned)
        {
            ani.SetBool("AtackS", false);
            atacando = false;
        }
    }

    public void ColliderWeaponTrue()
    {
        // Solo encender el arma si el enemigo no está en medio de un stun
        if (!isStunned && Hit != null)
            Hit.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        if (Hit != null)
            Hit.GetComponent<BoxCollider2D>().enabled = false;
    }
}
