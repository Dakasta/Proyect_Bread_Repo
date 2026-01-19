using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RangoEnemy : MonoBehaviour
{
    public Animator ani;
    public Skeleton enemigo;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            ani.SetBool("WalkS", false);
            
            ani.SetBool("AtackS", true);
            enemigo.atacando = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
