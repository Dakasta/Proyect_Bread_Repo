using System;
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
    public GameObject Hit; // El collider del arma del enemigo
    public int damage = 1;
    public bool puedeHacerDano = true;

    [Header("Configuración Cooldown")]
    // Modificado de 3f a 0.8f
    public float tiempoEntreAtaques = 0f;
    private float proximoAtaque = 0f;    // Marca de tiempo para el siguiente ataque

    private float vida = 2;
   

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
        
    }

    void Update()
    {
        // Si está aturdido (por tu espada), no hace NADA
        if (isStunned) return;

        Comportamientos();
    }

    public void Comportamientos()
    {
        float distancia = Mathf.Abs(transform.position.x - target.transform.position.x);

        // --- LÓGICA DE PATRULLA ---
        if (distancia > rango_vision && !atacando)
        {
            ani.SetBool("AtackS", false);
            ani.SetBool("WalkS", false);

            cronometro += Time.deltaTime;
            if (cronometro >= 4) { rutina = 1; cronometro = 0; }
            switch (rutina)
            {
                case 0: ani.SetBool("WalkS", false); break;
                //case 1: direccion = Random.Range(0, 2); rutina = 2; break;
                //case 2:
                   // ani.SetBool("WalkS", true);
                    //transform.rotation = Quaternion.Euler(0, (direccion == 0) ? 0 : 180, 0);
                    //transform.Translate(Vector3.right * speed_walk * Time.deltaTime);
                    break;
            }
        }
        else
        {
            // --- LÓGICA DE PERSEGUIR ---
            if (distancia > rango_ataque && !atacando)
            {
                ani.SetBool("WalkS", true);
                ani.SetBool("AtackS", false);

                if (transform.position.x < target.transform.position.x)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                transform.Translate(Vector3.right * speed_run * Time.deltaTime);
            }
            // --- LÓGICA DE ATACAR (Con espera de 0.8 segundos) ---
            else if (!atacando)
            {
                // Solo ataca si ya pasó el tiempo de espera (0.8s)
                if (Time.time >= proximoAtaque)
                {
                    atacando = true;
                    ani.SetBool("WalkS", false);
                    ani.SetBool("AtackS", true);
                    proximoAtaque = Time.time + tiempoEntreAtaques; // Usa la variable pública (0.8f)
                }
                else
                {
                    ani.SetBool("AtackS", false);
                    ani.SetBool("WalkS", false); // Se queda quieto esperando el cooldown
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. EL ENEMIGO GOLPEA AL PLAYER (Solo si el enemigo NO está aturdido)
       // if (collision.CompareTag("Player") && puedeHacerDano && !isStunned)
        //{
       //     Player player = collision.GetComponent<Player>();
        //    if (player != null) player.Damage(transform.position, damage);
       // }

        // 2. CHOQUE DE ESPADAS (Parry)
        if (collision.CompareTag("Sword"))
        {

             vida --;
            Debug.Log( vida);
            if ( vida <= 0)
            {

                Destroy(gameObject);
            }
            // Si el collider del arma del enemigo está encendido, es un choque de ataques
            if (Hit.GetComponent<BoxCollider2D>().enabled == true)
            {
                StopEnemyStun(); // El enemigo se para 1 segundo
            }
        }
    }

    // Funcion que inicia la corrutina de parálisis/stun
    public void StopEnemyStun()
    {
        StopAllCoroutines(); // Detiene cualquier stun anterior
        StartCoroutine(PausarEnemigo());
    }

    // Corrutina para pausar al enemigo 1 segundo
    IEnumerator PausarEnemigo()
    {
        isStunned = true;
        puedeHacerDano = false;
        atacando = false;
        if (Hit != null) Hit.GetComponent<BoxCollider2D>().enabled = false;

        ani.SetBool("WalkS", false);
        ani.SetBool("AtackS", false);
        ani.SetBool("ParryedS", true);

        yield return new WaitForSeconds(1f); // Espera 1 segundo (el stun por parry sigue siendo 1s)

        isStunned = false;
        puedeHacerDano = true;
        ani.SetBool("ParryedS", false);
    }

    // EVENTOS DE ANIMACIÓN (Llamados desde la animación de ataque)
    public void Final_Ani()
    {
        // Al terminar la animación de ataque, permitimos que vuelva a moverse
        atacando = false;
        ani.SetBool("AtackS", false);
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
