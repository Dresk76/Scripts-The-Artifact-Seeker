using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoOscuroBehaviour : StateMachineBehaviour
{
    [Header("DASH O SALTO")]
    [SerializeField] private float tiempo_Dash_Salto = 1.5f;
    [SerializeField] private float contadorT;


    [Header("VALORES RANDOM")]
    public int _salidaRandom;
    public int _ataqueRandom;


    [Header("REFERENCIAS")]
    private MagoOscuro _magoOscuro;





    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Referencias
        _magoOscuro = animator.GetComponent<MagoOscuro>();




        //---> llamar Metodo para Asignar el Material del Mago Oscuro
        _magoOscuro.AsignarMaterial();

        //---> Inicializar la _salidaRandom
        _salidaRandom = Random.Range(0, 2);

        //---> Inicializar el _ataqueRandom
        _ataqueRandom = Random.Range(0, 2);

        //---> Iniciar el valor del contadorT
        contadorT = tiempo_Dash_Salto;

        //---> Llamar metodo para girar al Mago Oscuro hacia el Mago
        _magoOscuro.MirarMago(true);

        //---> Numero aleatorio es entre 0 y 2 para uno de los 2 ataques
        animator.SetInteger("AtaqueAleatorio", _ataqueRandom);

        // animator.SetInteger("Dash_Salto", Random.Range(0, 2));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Comenzar a contar en reversa
        contadorT -= Time.deltaTime;


        //---> Mover el Mago Oscuro
        _magoOscuro.Mover();


        //---> Dash o Salto del Mago Oscuro cuando el contador llegue a 0
        if (contadorT <= 0)
        {
            switch(_salidaRandom)
            {
                case 0:
                    _magoOscuro.SaltarAdelante();
                    break;
                case 1:
                    _magoOscuro.StartCoroutine(_magoOscuro.Dash());
                    break;
            }
        }

        // Debug.Log("Valor Random Dash o Salto: " + _valorRandom);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> llamar Metodo para Retirar el Material del Mago Oscuro
        _magoOscuro.RetirarMaterial();
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
