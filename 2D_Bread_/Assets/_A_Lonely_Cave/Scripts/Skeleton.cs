using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public int direccion;
    public float sped_walk;
    public float speed_run;
    public GameObject target;
    public bool atacando;





    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
