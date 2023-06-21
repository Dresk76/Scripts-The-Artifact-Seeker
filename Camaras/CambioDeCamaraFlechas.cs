using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class CambioDeCamaraFlechas : MonoBehaviour
{
    [Header("CONDICIONES")]
    [SerializeField] private GameObject escaleraUno;
    [SerializeField] private GameObject areaDeVelocidadUno;
    [SerializeField] private GameObject areaDeVelocidadDos;
    private bool _magoEnRango;
    private bool _camaraActiva;


    [Header("TECLA DE INTERACCION")]
    [SerializeField] private GameObject teclaInteractiva;
    [SerializeField] private SpriteRenderer mouseSolo;
    [SerializeField] private SpriteRenderer mousePresionado;


    [Header("CARTELES")]
    [SerializeField] private GameObject[] simbolosCarteles;


    [Header("TIEMPOS DEL RECORRIDO")]
    private float _tiempoRecorrido = 7f;
    private float _tiempoEsperaMago = 0.5f;


    [Header("CAMARAS")]
    [SerializeField] private CinemachineStateDrivenCamera camaraMago_1;
    [SerializeField] private CinemachineVirtualCamera camaraFlechas;


    [Header("REFERENCIA COLOR JOYSTICK")]
    private Color _colorAro;
    private Color _colorRelleno;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip fade;
    [SerializeField] private AudioClip musicaRecorrido;
    [SerializeField] private AudioClip musicaBackground;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenFade = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenMusicaRecorrido = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenMusicaBackground = 0.3f;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;
    private Animator _animatorMago;


    [Header("REFERENCIAS")]
    [SerializeField] private Animator _animatorCamaraFlechas;


    [Header("UI REFERENCIAS")]
    [SerializeField] private Image aroJoystick;
    [SerializeField] private Image rellenoJoystick;
    [SerializeField] private GameObject barraDeVidaMago;
    [SerializeField] private GameObject controller;
    private TransicionCamaras _transicionCamaras;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _animatorMago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Animator>();
        _transicionCamaras = GameObject.Find("PanelTransicionCamaras").GetComponent<TransicionCamaras>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar
        /*
            - Camara Flechas
            - Area de velocidad 1
            - Area de velocidad 2
        */
        camaraFlechas.gameObject.SetActive(false);
        areaDeVelocidadUno.SetActive(false);
        areaDeVelocidadDos.SetActive(false);


        //---> Guardar el color del Joystick
        _colorAro = aroJoystick.color;
        _colorRelleno = rellenoJoystick.color;
    }



    private void Update()
    {
        if (_magoEnRango && Input.GetButtonDown("Fire1") && _mago.enSuelo)
        {
            if (!_camaraActiva)
            {
                // Iniciar Recorrido
                StartCoroutine(IniciarRecorrido());
            }
        }
    }



    //---> Recorrido de las tres flechas
    private IEnumerator IniciarRecorrido()
    {
        //---> Desactivar Camara Principal 1
        camaraMago_1.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        //---> Deshabilitar el movimiento del Mago
        _mago.puedeMoverse = false;
        yield return new WaitForSeconds(0.5f);

        //---> Remover transparencia la tecla interactiva
        mouseSolo.color = new Color(255f, 255f, 255f, 0f);
        mousePresionado.color = new Color(255f, 255f, 255f, 0f);

        //---> Retirar transparencia del Joystick
        aroJoystick.color = new Color(255f, 255f, 255f, 0f);
        rellenoJoystick.color = new Color(255f, 255f, 255f, 0f);

        //---> Desactivar la Barra de vida del Mago
        barraDeVidaMago.gameObject.SetActive(false);

        //---> Desactivar Controller
        controller.gameObject.SetActive(false);

        //---> Desactivar  Mobile Controller
        // mobileController.gameObject.SetActive(false);

        //---> Activar Camara Flechas
        camaraFlechas.gameObject.SetActive(true);
        _camaraActiva = true;

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de entrada de la TransicionCamaras
        _transicionCamaras.IniciarCambioCamara(true);

        //---> Iniciar el reccorido de la camara
        _animatorCamaraFlechas.SetBool("Idle", false);

        if (simbolosCarteles[0].activeSelf) 
        {
            //---> Reproducir musica del recorrido
            _audioManager.PlayMusic(musicaRecorrido, volumenMusicaRecorrido, false);

            _animatorCamaraFlechas.SetTrigger("FlechaUno");
            yield return new WaitForSeconds(_tiempoRecorrido);
        } 
        else if (simbolosCarteles[1].activeSelf) 
        {
            //---> Reproducir musica del recorrido
            _audioManager.PlayMusic(musicaRecorrido, volumenMusicaRecorrido, false);

            _animatorCamaraFlechas.SetTrigger("FlechaDos");
            yield return new WaitForSeconds(_tiempoRecorrido);
        }
        else if (simbolosCarteles[2].activeSelf)
        {
            //---> Reproducir musica del recorrido
            _audioManager.PlayMusic(musicaRecorrido, volumenMusicaRecorrido, false);

            _animatorCamaraFlechas.SetTrigger("FlechaTres");
            yield return new WaitForSeconds(_tiempoRecorrido);
        }

        //---> Detener Recorrido
        StartCoroutine(DetenerRecorrido());
    }



    //---> Detener el recorrido
    private IEnumerator DetenerRecorrido()
    {
        _animatorCamaraFlechas.SetBool("Idle", true);

        //---> Desactivar Camara Flechas
        camaraFlechas.gameObject.SetActive(false);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de salida de la TransicionCamaras
        _transicionCamaras.TerminarCambioCamara(true);
        yield return new WaitForSeconds(1f);

        //---> Reproducir musica del background
        _audioManager.PlayMusic(musicaBackground, volumenMusicaBackground, true);

        //---> Restaurar transparencia la tecla interactiva
        mouseSolo.color = new Color(255f, 255f, 255f, 255f);
        mousePresionado.color = new Color(255f, 255f, 255f, 255f);

        //---> Restaurar transparencia del Joystick
        aroJoystick.color = _colorAro;
        rellenoJoystick.color = _colorRelleno;

        //---> Activar la Barra de vida del Mago
        barraDeVidaMago.gameObject.SetActive(true);

        //---> Activar Controller
        controller.gameObject.SetActive(true);

        //---> Activar Camara Principal 1
        camaraMago_1.gameObject.SetActive(true);
        _camaraActiva = false;
        yield return new WaitForSeconds(_tiempoEsperaMago);

        //---> Habilitar el movimiento del Mago
        _mago.puedeMoverse = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _magoEnRango = true;
            teclaInteractiva.SetActive(true);

            if (simbolosCarteles[0].activeSelf) 
            {
                escaleraUno.SetActive(false);
                areaDeVelocidadUno.SetActive(true);
            }
            else if (simbolosCarteles[1].activeSelf) 
            {
                escaleraUno.SetActive(false);
                areaDeVelocidadDos.SetActive(true);
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _magoEnRango = false;
            teclaInteractiva.SetActive(false);

            if (!simbolosCarteles[0].activeSelf)
            {
                escaleraUno.SetActive(true);
                areaDeVelocidadUno.SetActive(false);
                areaDeVelocidadDos.SetActive(false);
            }
        }
    }
}