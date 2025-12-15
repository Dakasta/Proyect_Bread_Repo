using System.Collections;
using System.Collections.Genetic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private RigidBody2D playerRB;
    private Animator anim;
    private float horizontalInput;



    public float speed;
    public float jumpForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
