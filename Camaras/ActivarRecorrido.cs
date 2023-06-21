using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

//---> Activar el recorrido de los puntos que se deben abrir al entrar en la escena del juego
public class ActivarRecorrido : MonoBehaviour
{
    [Header("ACTIVAR PARTICULAS")]
    [SerializeField] private ParticleSystem[] particulas;


    [Header("TIEMPOS DEL RECORRIDO")]
    private float _tiempoIniciarRecorrido = 1.65f;
    private float _tiempoPorCamara = 1.5f;
    private float _tiempoIniciarPortafolio = 0.5f;


    [Header("CAMARAS")]
    [SerializeField] private CinemachineStateDrivenCamera camaraMago_1;
    [SerializeField] private CinemachineBlendListCamera camaraRecorrido;


    [Header("AREA PRIMERA PARTE")]
    [SerializeField] private GameObject areaCambioAPrimeraParte;


    [Header("REFERENCIA COLOR JOYSTICK")]
    private Color _colorAro;
    private Color _colorRelleno;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip musicaBackground;
    [SerializeField] private AudioClip fade;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenMusicaBackground = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenFade = 0.4f;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;
    private Animator _animator;


    [Header("UI REFERENCIAS")]
    [SerializeField] private Image aroJoystick;
    [SerializeField] private Image rellenoJoystick;
    [SerializeField] private GameObject barraDeVidaMago;
    [SerializeField] private GameObject controller;
    private TransicionCamaras _transicionCamaras;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _mago = FindObjectOfType<Mago>();
        _animator = GameObject.FindGameObjectWithTag("Mago").GetComponent<Animator>();
        _transicionCamaras = GameObject.Find("PanelTransicionCamaras").GetComponent<TransicionCamaras>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar
        /*
            - Camara de recorrido 
            - Barra de Vida del Mago
            - Area Primera Parte
        */
        camaraRecorrido.gameObject.SetActive(false);
        barraDeVidaMago.SetActive(false);
        areaCambioAPrimeraParte.SetActive(false);


        //---> Guardar el color del Joystick
        _colorAro = aroJoystick.color;
        _colorRelleno = rellenoJoystick.color;
    }



    private IEnumerator IniciarRecorrido()
    {
        //---> Reproducir musica del background
        _audioManager.PlayMusic(musicaBackground, volumenMusicaBackground, true);

        //---> Deshabilitar el movimiento del Mago
        _mago.puedeMoverse = false;
        _mago.velocidadObjetivo = Vector3.zero;
        _mago.rb2D.velocity = Vector2.zero;

        //---> Hacer un golpe de invocacion al iniciar el recorrido de las camaras
        _animator.SetTrigger("Invocar");
        
        yield return new WaitForSeconds(_tiempoIniciarRecorrido);

        //---> Desactivar Camara Principal 1
        camaraMago_1.gameObject.SetActive(false);

        //---> Retirar transparencia del Joystick
        aroJoystick.color = new Color(255f, 255f, 255f, 0f);
        rellenoJoystick.color = new Color(255f, 255f, 255f, 0f);

        //---> Desactivar Controller
        controller.gameObject.SetActive(false);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de entrada de la TransicionCamaras
        _transicionCamaras.IniciarCambioCamara(true);

        yield return new WaitForSeconds(0.1f);

        //---> Activar Camaras Recorrido
        camaraRecorrido.gameObject.SetActive(true);

        //---> Recorrido Camara Autorretrato
        yield return new WaitForSeconds(_tiempoPorCamara);
        _audioManager.PlaySFX(fade, volumenFade);       // Sonido al realizar el fade
        _transicionCamaras.IniciarCambioCamara(true);   // Hacer el Fade de entrada de la TransicionCamaras

        //---> Recorrido Camara Escudo de habilidades
        yield return new WaitForSeconds(_tiempoPorCamara);
        _audioManager.PlaySFX(fade, volumenFade);       // Sonido al realizar el fade
        _transicionCamaras.IniciarCambioCamara(true);   // Hacer el Fade de entrada de la TransicionCamaras

        //---> Recorrido Camara Libro
        yield return new WaitForSeconds(_tiempoPorCamara);
        _audioManager.PlaySFX(fade, volumenFade);       // Sonido al realizar el fade
        _transicionCamaras.IniciarCambioCamara(true);   // Hacer el Fade de entrada de la TransicionCamaras

        //---> Recorrido Camara Cofre
        yield return new WaitForSeconds(_tiempoPorCamara);
        _audioManager.PlaySFX(fade, volumenFade);       // Sonido al realizar el fade
        _transicionCamaras.TerminarCambioCamara(true);  // Hacer el Fade de salida de la TransicionCamaras


        //---> Desactivar Camaras Recorrido
        camaraRecorrido.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        //---> Restaurar transparencia del Joystick
        aroJoystick.color = _colorAro;
        rellenoJoystick.color = _colorRelleno;

        //---> Activar la Barra de vida del Mago
        barraDeVidaMago.gameObject.SetActive(true);

        //---> Activar Controller
        controller.gameObject.SetActive(true);

        //---> Activar Camara Principal 1
        camaraMago_1.gameObject.SetActive(true);

        yield return new WaitForSeconds(_tiempoIniciarPortafolio);

        //---> Activar el Area de CambioAPrimeraParte
        areaCambioAPrimeraParte.gameObject.SetActive(true);

        //---> Habilitar el movimiento del Mago
        _mago.puedeMoverse = true;

        //---> Desactivar los sonidos de las particulas
        ActivarParticulas(false);
    }



    //---> Metodo para activar las particulas del area
    private void ActivarParticulas(bool activar)
    {
        foreach(ParticleSystem particula in particulas)
        {
            if (activar)
            {
                particula.Play();
            }
            else
            {
                particula.Stop();
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.CompareTag("Mago"))
        {
            //---> Activar las particulas
            ActivarParticulas(true);

            //---> Desactivar el BoxCollider2D para que haga el recorrido una vez
            this.GetComponent<BoxCollider2D>().enabled = false;

            //---> Iniciar corrutina recorrido de objetos a descubrir
            StartCoroutine(IniciarRecorrido());
        }
    }
}
