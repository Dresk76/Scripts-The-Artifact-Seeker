using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _magoEnRango;


    [Header("TIEMPO PARA ABRIR PUERTA")]
    [SerializeField] private float tiempoAbrirPuerta = 1f;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip cerrarPuerta;
    [SerializeField] private AudioClip abrirPuerta;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenCerrarPuerta = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenAbrirPuerta = 0.5f;


    [Header("REFERENCIAS")]
    private Animator _animator;


    [Header("VARIABLES PUBLICAS")]
    [SerializeField] private bool puertaAbierta = true; // Pasar la puerta con la cual esta interactuando





    //---> Modificar la puertaAbierta por un metodo set
    public void SetEstadoDePuerta(bool puertaAbierta)
    {
        this.puertaAbierta = puertaAbierta;
    }



    //---> Pasar el estado de la puerta por un metodo get
    public bool GetEstadoDePuerta()
    {
        return this.puertaAbierta;
    }



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }



    //---> Cambiar el estado de la puerta
    public IEnumerator AbrirPuertas(bool abrir, bool reproducir)
    {
        if (reproducir)
        {
            //---> Sonido al abrirse la puerta
            _audioManager.PlayPuerta(abrirPuerta, volumenAbrirPuerta);
    
            yield return new WaitForSecondsRealtime(tiempoAbrirPuerta);
        }

        //---> Animacion de abrir puerta
        _animator.SetBool("PuertaAbierta", abrir);

        //---> Indicar que la puerta esta Abierta
        puertaAbierta = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Desactivar el BoxCollider2D para que solo se cierre una vez
            this.GetComponent<BoxCollider2D>().enabled = false;

            //---> Animacion de cerrar puerta
            _animator.SetBool("PuertaAbierta", false);

            //---> Sonido al cerrarse la puerta
            _audioManager.PlayPuerta(cerrarPuerta, volumenCerrarPuerta);

            //---> Indicar que la puerta esta Cerrada
            puertaAbierta = false;
        }
    }
}
