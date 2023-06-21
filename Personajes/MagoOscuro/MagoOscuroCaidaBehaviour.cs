using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoOscuroCaidaBehaviour : StateMachineBehaviour
{
    [Header("REFERENCIAS PARTICULAS DEL MAGO")]
    private ParticulasPolvoMagoOscuro _particulasPolvoMagoOscuro;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip impactoEnSuelo;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenImpactoEnSuelo = 0.25f;


    [Header("REFERENCIAS")]
    private MagoOscuro _magoOscuro;





    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Referencias
        _magoOscuro = animator.GetComponent<MagoOscuro>();



        //---> Referencias
        _particulasPolvoMagoOscuro = GameObject.Find("ParticulasPolvoMagoOscuro").GetComponent<ParticulasPolvoMagoOscuro>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        //---> llamar Metodo para Retirar el Material del Mago Oscuro
        _magoOscuro.RetirarMaterial();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Al salir de la Animacion LevantarseDeSalto llamar metodo ParticulasCaida
        _particulasPolvoMagoOscuro.ParticulasCaida();

        //---> Reproducir sonido del Mago Oscuro al caer al suelo 
        _audioManager.PlayMagoOscuro(impactoEnSuelo, volumenImpactoEnSuelo);
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
