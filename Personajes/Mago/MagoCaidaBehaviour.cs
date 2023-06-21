using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoCaidaBehaviour : StateMachineBehaviour
{
    [Header("REFERENCIAS PARTICULAS DEL MAGO")]
    private ParticulasPolvoMago _particulasPolvoMago;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip impactoEnSuelo;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenImpactoEnSuelo = 0.15f;


    [Header("REFERENCIAS")]
    private Mago _mago;





    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //---> Referencias
        _particulasPolvoMago = GameObject.Find("ParticulasPolvoMago").GetComponent<ParticulasPolvoMago>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _mago = animator.GetComponent<Mago>();
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
        _particulasPolvoMago.ParticulasCaida();

        //---> Reproducir Sonido del Mago al caer al suelo 
        _audioManager.PlayMago(impactoEnSuelo, volumenImpactoEnSuelo);
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
