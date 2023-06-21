using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimerAtaqueBehaviour : StateMachineBehaviour
{
    [Header("REFERENCIAS")]
    private MagoOscuro _magoOscuro;





    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Referencias
        _magoOscuro = animator.GetComponent<MagoOscuro>();


        //---> Sonido del Mago Oscuro al realizar el primer ataque
        _magoOscuro.SonidoPrimerAtaque();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Al salir de la animacion Ataque llamar metodo para saltar atras
        _magoOscuro.SaltarAtras();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
