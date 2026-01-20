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
    private bool puedeRecibirDano = true;

    [Header("Configuración Cooldown")]
    public float tiempoEntreAtaques = 0f;
    private float proximoAtaque = 0f;

    private float vida = 2;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        if (isStunned) return;
        Comportamientos();
    }

    public void Comportamientos()
    {
        float distancia = Mathf.Abs(transform.position.x - target.transform.position.x);

        if (distancia > rango_vision && !atacando)
        {
            ani.SetBool("AtackS", false);
            ani.SetBool("WalkS", false);

            cronometro += Time.deltaTime;
            if (cronometro >= 4) { rutina = 1; cronometro = 0; }
            switch (rutina)
            {
                case 0: ani.SetBool("WalkS", false); break;
            }
        }
        else
        {
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
            else if (!atacando)
            {
                if (Time.time >= proximoAtaque)
                {
                    atacando = true;
                    ani.SetBool("WalkS", false);
                    ani.SetBool("AtackS", true);
                    proximoAtaque = Time.time + tiempoEntreAtaques;
                }
                else
                {
                    ani.SetBool("AtackS", false);
                    ani.SetBool("WalkS", false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            // 1. LÓGICA DE DAÑO (Exacta: 2 golpes)
            if (puedeRecibirDano)
            {
                vida--;
                Debug.Log("Vida esqueleto: " + vida);

                if (vida <= 0)
                {
                    Destroy(gameObject);
                    return;
                }
                StartCoroutine(CooldownDano());
            }

            // 2. LÓGICA DE PARRY (Choque de armas)
            // Se activa solo si el arma del enemigo está encendida (está atacando)
            if (Hit != null && Hit.GetComponent<BoxCollider2D>().enabled)
            {
                StopEnemyStun();
            }
        }
    }

    IEnumerator CooldownDano()
    {
        puedeRecibirDano = false;
        // 0.25s es el tiempo ideal para evitar el multi-click pero permitir combos
        yield return new WaitForSeconds(0.25f);
        puedeRecibirDano = true;
    }

    public void StopEnemyStun()
    {
        // Importante: No usamos StopAllCoroutines para no cancelar el CooldownDano
        StartCoroutine(PausarEnemigo());
    }

    IEnumerator PausarEnemigo()
    {
        isStunned = true;
        puedeHacerDano = false;
        atacando = false;

        if (Hit != null) Hit.GetComponent<BoxCollider2D>().enabled = false;

        ani.SetBool("WalkS", false);
        ani.SetBool("AtackS", false);
        ani.SetBool("ParryedS", true);

        yield return new WaitForSeconds(1f);

        isStunned = false;
        puedeHacerDano = true;
        ani.SetBool("ParryedS", false);
    }

    public void Final_Ani()
    {
        atacando = false;
        ani.SetBool("AtackS", false);
    }

    public void ColliderWeaponTrue()
    {
        if (!isStunned && Hit != null)
            Hit.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        if (Hit != null)
            Hit.GetComponent<BoxCollider2D>().enabled = false;
    }
}
