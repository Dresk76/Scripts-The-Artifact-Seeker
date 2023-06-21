using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    [Header("TIEMPOS")]
    private float tiempoLinea = 0.3f;


    [Header("MENU SALIR")]
    [SerializeField] private GameObject menuSalir;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip enter;
    [SerializeField] private AudioClip reaunudar;
    [SerializeField] private AudioClip opciones;
    [SerializeField] private AudioClip salir;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenEnter = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenReaunudar = 0.8f;
    [Range(0, 1f)][SerializeField] private float volumenOpciones = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenSalir = 0.55f;


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu Salir
        menuSalir.SetActive(false);
    }



    //---> Sonido de Mouse over
    public void SonidoEnter()
    {
        _audioManager.PlayEnterUI(enter, volumenEnter);
    }



    //---> Corrutina para mostrar una linea antes de realizar lo de Reaunudar
    private IEnumerator LineaReaunudar(GameObject linea)
    {
        //---> Reaunudar la escala del tiempo actual
        Time.timeScale = 1f;

        //---> Reproducir sonido de Reaunudar
        _audioManager.PlaySFX(reaunudar, volumenReaunudar);

        linea.gameObject.SetActive(true);
        //---> Tiempo para mostrar la linea y Reaunudar el Portafolio
        yield return new WaitForSecondsRealtime(tiempoLinea);
        linea.gameObject.SetActive(false);

        //---> Pasar al metodo MetodoPausa() estaPausado = false para que reconozca que salio del Menu de Pausa
        _controlDelTiempo.setEstaPausado(false);

        //---> Llamar metodo ControlSonido()
        _controlDelTiempo.ControlSonido();

        //---> Desactivar el MenuPausa
        this.gameObject.SetActive(false);

        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        // Debug.Log("Esta Pausado: " + _controlDelTiempo.getEstaPausado());
    }



    //---> Metodo para Reaunudar el portafolio
    public void Reunudar(GameObject linea)
    {
        StartCoroutine(LineaReaunudar(linea));
    }



    //---> Corrutina para mostrar una linea antes de realizar lo de Opciones
    private IEnumerator LineaOpciones(GameObject linea)
    {
        //---> Reproducir sonido de Opciones
        _audioManager.PlaySFX(opciones, volumenOpciones);

        //---> Indicar que esta entrando al Menu de opciones
        _objetosEntreEscenas.estaEnOpciones = true;

        linea.gameObject.SetActive(true);
        //---> Tiempo para mostrar la linea y abrir el Menu de Opciones
        yield return new WaitForSecondsRealtime(tiempoLinea);
        linea.gameObject.SetActive(false);

        //---> Activar el Menu de opciones
        _objetosEntreEscenas.menuOpciones.SetActive(true);
    }



    //---> Metodo para ingresar a las Opciones del portafolio
    public void Opciones(GameObject linea)
    {
        StartCoroutine(LineaOpciones(linea));
    }



    //---> Corrutina para mostrar una linea antes de realizar lo de Salir
    private IEnumerator LineaSalir(GameObject linea)
    {
        //---> Reproducir sonido de Salir
        _audioManager.PlaySFX(salir, volumenSalir);

        //---> Indicar que esta entrando al Menu Salir
        _objetosEntreEscenas.estaEnSalir = true;

        linea.gameObject.SetActive(true);
        //---> Tiempo para mostrar la linea y abrir el Menu Salir
        yield return new WaitForSecondsRealtime(tiempoLinea);
        linea.gameObject.SetActive(false);

        //---> Activar el Menu Salir
        menuSalir.SetActive(true);
    }



    //---> Metodo para Salir del portafolio
    public void Salir(GameObject linea)
    {
        StartCoroutine(LineaSalir(linea));
    }
}