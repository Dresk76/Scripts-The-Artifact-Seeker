using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoOscuroIniciarBehaviour : StateMachineBehaviour
{
    [Header("REFERENCIAS")]
    private MagoOscuro _magoOscuro;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("REFERENCIA MENU GAME OVER")]
    private MenuGameOver _menuGameOver;





    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Referencias
        _magoOscuro = animator.GetComponent<MagoOscuro>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _menuGameOver = GameObject.Find("Menus").GetComponent<MenuGameOver>();



        if (_menuGameOver.getEstaEnGameOver())
        {
            //---> Llamar metodo para girar al Mago Oscuro hacia el Mago
            _magoOscuro.MirarMago(false);

            //---> Llamar metodo para girar al Mago hacia el MagoOscuro
            _mago.MirarMagoOscuro();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
