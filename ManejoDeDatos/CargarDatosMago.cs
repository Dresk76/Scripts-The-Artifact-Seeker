using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CargarDatosMago : MonoBehaviour
{
    [Header("CONTADOR")]
    private int _contador = 0; // Contador para que solo se abra la primera vez el Menu Fin Portafolio


    [Header("ESTADO POR DEFECTO DE LAS OPCIONES")]
    [SerializeField] private TMP_Text textoContinuar;
    [SerializeField] private GameObject gemasContinuar;
    [SerializeField] private TMP_ColorGradient colorBlanco;


    [Header("REFERENCIA DEL MAGO")]
    private Mago _mago;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip fade;
    [SerializeField] private AudioClip transportarPrimerPiso;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenFade = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenTransportarPrimerPiso = 0.13f;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("UI REFERENCIAS")]
    [SerializeField] private GameObject menuFinPortafolio;
    private TransicionCamaras _transicionCamaras;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _transicionCamaras = GameObject.Find("PanelTransicionCamaras").GetComponent<TransicionCamaras>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu Fin Portafolio
        menuFinPortafolio.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Llamar el metodo RealizarCambio()
            RealizarCambio();
        }
    }



    //---> Metodo para que la opcion de Continuar aparezca señalada al abrir el Menu de Fin Portafolio
    private void EstadoPorDefectoContinuar()
    {
        //---> Inciar señalado por defecto la opcion de Reaunudar
        textoContinuar.colorGradientPreset = colorBlanco;
        textoContinuar.fontSize = 65f;
        gemasContinuar.gameObject.SetActive(true);
    }



    //---> Metodo para realizar el cambio al primer piso
    private void RealizarCambio()
    {
        //---> Sonido al Transportar el Mago al Primer Piso
        _audioManager.PlayBoom(transportarPrimerPiso, volumenTransportarPrimerPiso);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de entrada de la TransicionCamaras
        _transicionCamaras.IniciarCambioCamara(true);

        //---> Llamar metodo de CargarDatos
        CargarDatos();

        if (_contador < 1)
        {
            //---> Deshabilitar el movimiento del Mago
            _mago.puedeMoverse = false;
            _mago.velocidadObjetivo = Vector3.zero;
            _mago.rb2D.velocity = Vector2.zero;

            //---> Llamar el metodo EstadoPorDefectoContinuar()
            EstadoPorDefectoContinuar();

            //---> Activar el menuFinPortafolio
            menuFinPortafolio.SetActive(true);

            //---> Indicar que esta entrando al Menu Fin Portafolio
            _objetosEntreEscenas.estaEnFinPortafolio = true;
        }

        //---> Actualizar el contador
        _contador += 1;
    }



    //---> Cargar los datos del Mago cuando entre en el area
    private void CargarDatos()
    {
        //---> Variable de tipo DatosMago donde se van a recibir los datos guardados en el archivo
        DatosMago datosMago = SaveManager.CargarDatosMago();

        //---> Cargar la posicion del Mago
        _mago.transform.position = new Vector2(datosMago.position[0], datosMago.position[1]);
        // Debug.Log("Datos cargados desde Script: CargarDatosMago");
    }
}
