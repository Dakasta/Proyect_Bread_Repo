using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeCaminar : StateMachineBehaviour
{
    private Boss1 jefe;
    private Rigidbody2D rb2D;

    [SerializeField] private float velocidadMovimiento;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jefe = animator.GetComponent<Boss1>();
        rb2D = jefe.rb2D;

        jefe.MirarJugador();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D.linearVelocity = new Vector2(velocidadMovimiento, rb2D.linearVelocity.y) * animator.transform.right;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
    }

}
