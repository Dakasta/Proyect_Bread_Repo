using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [Header("Movement And Jump Configuration")]
    [SerializeField] float Speed = 6;
    [SerializeField] float Jumpforce = 6;
    [SerializeField] bool IsGrounded;
    [SerializeField] float GroundCheckRadius;
    [SerializeField] LayerMask GroundLayer;


    [SerializeField] Transform Groundcheck;//referencia posicion suelo
    Rigidbody2D playerRb;
    Animator anim;
    PlayerInput input;
    Vector2 moveInput;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }
    void Start()
    {

    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {

        playerRb.linearVelocity = new Vector2(moveInput.x * Speed, playerRb.linearVelocity.y);
    }
    // Update is called once per frame
    void Update()
    {

    }



    #region impuf methods
    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();

    }
    public void OnJump(InputAction.CallbackContext context)
    {



    }
    public void OnAtack(InputAction.CallbackContext context)
    {



    }

    #endregion



}