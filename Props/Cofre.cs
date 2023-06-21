using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _magoEnRango;
    private bool _cofreAbierto;
    private bool _idle = true;
    private int _contadorCofre;


    [Header("TECLA DE INTERACCION")]
    [SerializeField] private GameObject teclaInteractiva;


    [Header("LUZ")]
    [SerializeField] private GameObject luz;


    [Header("ACTIVAR PARTICULAS CENTRO DE LUZ")]
    [SerializeField] private ParticleSystem centroDeLuz;
    
    
    [Header("CONTACTAME")]
    [SerializeField] private GameObject iconosRedes;


    [Header("DICCIONARIO HABILITAR ESCENARIO")]
    private HabilitarEscenario _habilitarEscenario;


    [Header("PARTICULAS VIDA")]
    [SerializeField] private ParticleSystem particulasVida;
    


    [Header("TIEMPOS")]
    [SerializeField] private float tiempoEnMostrarRedes = 0.2f;
    [SerializeField] private float tiempoEnHabilitarBoton = 1.1f;
    [SerializeField] private float tiempoVidaNueva = 1f;
    [SerializeField] private float tiempoMoverseMago = 1.5f;


    [Header("REFERENCIA COLLIDER CETRO DE MAGO")]
    [SerializeField] private CircleCollider2D circleCollider2D;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip abrirCofre;
    [SerializeField] private AudioClip cerrarCofre;
    [SerializeField] private AudioClip particulasVidaNueva;
    [SerializeField] private AudioSource sacudirCofre;
    private AudioSource _audioSource;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenAbrirCofre = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenCerrarCofre = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenParticulasVidaNueva = 0.5f;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;
    private Animator _animatorMago;
    private SaludMago _saludMago;


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;


    [Header("REFERENCIAS")]
    private Animator _animatorCofre;


    [Header("UI REFERENCIAS")]
    [SerializeField] private GameObject menuRedes;





    private void Awake()
    {
        _habilitarEscenario = GameObject.Find("HabilitarEscenario").GetComponent<HabilitarEscenario>();
        _audioSource = GetComponent<AudioSource>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _animatorMago = _mago.GetComponent<Animator>();
        _saludMago = _mago.GetComponent<SaludMago>();
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
        _animatorCofre = GetComponent<Animator>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu Redes
        menuRedes.SetActive(false);

        //---> Iniciar con el mago en Idle
        if (_idle)
        {
            _animatorMago.SetBool("Idle", true);
        }
    }



    private void Update()
    {
        //---> Si esta activo el menu de Pausa no puede realizar nada en el update y pausa el sonido de los Objetos
        if (_controlDelTiempo.getEstaPausado())
        {
            _audioSource.Pause();
            return;
        }
        else
        {
            _audioSource.UnPause();
        }


        if (_magoEnRango && Input.GetButtonDown("Fire1") && _mago.enSuelo)
        {
            if (!_cofreAbierto)
            {
                //---> Llamar la corrutina AbrirCofre
                StartCoroutine(AbrirCofre());
            }
        }
    }



    //---> Abrir el cofre
    private IEnumerator AbrirCofre()
    {   
        //---> Cofre abierto
        _cofreAbierto = true;

        //---> Para que no se mueva el Mago
        _mago.puedeMoverse = false;

        //---> Desactivar el trigger del cetro del Mago
        circleCollider2D.isTrigger = false;

        //---> Contar las veces que se abre el cofre
        _contadorCofre += 1;

        //---> Desactivar la tecla interactiva
        teclaInteractiva.SetActive(false);

        //---> Detener el sonido de sacudir el cofre al abrirse
        sacudirCofre.Stop();

        //---> Sonido al abrir el cofre
        _audioManager.PlaySFX(abrirCofre, volumenAbrirCofre);

        //---> Desactivar la luz del cofre
        luz.gameObject.SetActive(false);

        //---> Animacion Abrir el cofre
        _animatorCofre.SetBool("EstadoCofre", true);

        yield return new WaitForSeconds(tiempoEnMostrarRedes);

        //---> Activar iconos de Contactame
        iconosRedes.gameObject.SetActive(true);

        yield return new WaitForSeconds(tiempoEnHabilitarBoton);

        //---> Llamar corrutina VidaNueva
        StartCoroutine(VidaNueva(2));

        //---> Activar el menu de redes
        menuRedes.gameObject.SetActive(true);

        //---> Tiempo para que se restaure el movimiento del Mago
        yield return new WaitForSeconds(tiempoMoverseMago);

        //---> Para que se mueva de nuevo el Mago
        _mago.puedeMoverse = true;
    }


    //---> Corrutina para tomar una al interactuar con un objeto magico
    private IEnumerator VidaNueva(int vidasNuevas)
    {
        if (_contadorCofre == 1)
        {
            //---> Mostrar las particulas de vida
            particulasVida.Play();

            //---> Reproducir de las particulas de vida
            _audioManager.PlaySFX(particulasVidaNueva, volumenParticulasVidaNueva);

            yield return new WaitForSeconds(tiempoVidaNueva);

            //---> Asignar 3 vidas al Mago
            _saludMago.TomarVidas(vidasNuevas);
        }
    }


    //---> Cerrar el cofre
    private IEnumerator CerrarCofre()
    {
        if (_cofreAbierto)
        {
            //---> Activar el trigger del cetro del Mago
            circleCollider2D.isTrigger = true;

            //---> Sonido al cerrar el cofre
            _audioManager.PlaySFX(cerrarCofre, volumenCerrarCofre);

            //---> Animacion Cerrar el cofre
            _animatorCofre.SetBool("EstadoCofre", false);

            //---> Desactivar el menu de redes
            menuRedes.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            //---> Desactivar iconos de Contactame
            iconosRedes.gameObject.SetActive(false);

            //---> Activar la luz del cofre
            luz.gameObject.SetActive(false);

            //---> Cofre cerrado
            _cofreAbierto = false;
        }
    }



    //---> Activar la particula de iluminacion
    public void ActivarParticulas()
    {
        centroDeLuz.Play();
    }



    //---> Sonido al realizar una invocacion
    public void SonidoSacudirCofre()
    {
        sacudirCofre.Play();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _magoEnRango = true;
            teclaInteractiva.SetActive(true);
            luz.gameObject.SetActive(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _magoEnRango = false;
            teclaInteractiva.SetActive(false);
            StartCoroutine(CerrarCofre());
        }
    }
}
