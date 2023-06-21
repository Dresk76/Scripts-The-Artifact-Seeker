using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlDelTiempo : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _estaPausado;


    [Header("ESTADO POR DEFECTO DE LAS OPCIONES")]
    [SerializeField] private TMP_Text textoReaunudar;
    [SerializeField] private GameObject gemasReaunudar;
    [SerializeField] private TMP_ColorGradient colorBlanco;


    [Header("REFERENCIAS OPCIONES")]
    [SerializeField] private TMP_Text textoOpciones;
    [SerializeField] private GameObject gemasOpciones;
    [SerializeField] private TMP_Text textoSalir;
    [SerializeField] private GameObject gemasSalir;
    [SerializeField] private TMP_ColorGradient colorGris;


    [Header("VELOCIDAD CAMBIO TIEMPO")]
    //---> Velocidad a la que se van a mover los objetos en pantalla, rango entre 0 - 10
    [Range(0, 10f)][SerializeField] private float velocidadDelTiempo = 0.5f;
    //---> Contener la escala del tiempo a la que se esta ejecutando unity, para volver el tiempo a su velocidad inicial
    private float _escalaDeTiempoInicial;
    //---> Contiene el valor de ficxedDeltaTime antes de relentizar o acelear el tiempo, para volver el tiempo a su velocidad inicial
    private float _fixedDeltaTimeInicial;


    [Header("LINEA REAUNUDAR")]
    [SerializeField] private GameObject lineaReaudar;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip sonidoMenuPausa;
    [SerializeField] private AudioClip reaunudar;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenMenuPausa = 0.7f;
    [Range(0, 1f)][SerializeField] private float volumenReaunudar = 0.8f;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIA MENU GAME OVER")]
    private MenuGameOver _menuGameOver;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("UI REFERENCIAS")]
    [SerializeField] private GameObject menuPausa;





    //---> Metodo Set para estaPausado
    public void setEstaPausado(bool estaPausado)
    {
        this._estaPausado = estaPausado;
    }



    //---> Metodo Get para retornar el valor de la variable estaPausado
    public bool getEstaPausado()
    {
        return this._estaPausado;
    }



    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _menuGameOver = GameObject.Find("Menus").GetComponent<MenuGameOver>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu de Pausa
        menuPausa.SetActive(false);


        //---> Asignar el valor contenido del tiempo en escala en el que se encuentra unity
        _escalaDeTiempoInicial = Time.timeScale;

        //---> Hacer lo mismo con la variable _fixedDeltaTimeInicial
        _fixedDeltaTimeInicial = Time.fixedDeltaTime;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //---> Condicion para que solo se desactive el MetodoPausa() con ESC al estar en el Menu de Pausa
            if(!_objetosEntreEscenas.estaEnOpciones && !_objetosEntreEscenas.estaEnSalir && !_menuGameOver.getEstaEnGameOver() && !_objetosEntreEscenas.estaEnFinPortafolio)
            {
                MetodoPausa();

                // Debug.Log("Esta Pausado: " + _estaPausado);
            }
        }
    }



    //---> Metodo para que la opcion de Reaunudar aparezca señalada al abrir el Menu de Pausa
    private void EstadoPorDefectoReaunudar()
    {
        //---> Inciar señalado por defecto la opcion de Reaunudar
        textoReaunudar.colorGradientPreset = colorBlanco;
        textoReaunudar.fontSize = 65f;
        gemasReaunudar.gameObject.SetActive(true);
    }



    //---> Metodo para que al Reaunudar el portafolio las opciones se deshabiliten
    private void DesactivarOpciones()
    {
        //---> Opcion de Opciones
        textoOpciones.colorGradientPreset = colorGris;
        textoOpciones.fontSize = 60f;
        gemasOpciones.gameObject.SetActive(false);

        //---> Opcion de Salir
        textoSalir.colorGradientPreset = colorGris;
        textoSalir.fontSize = 60f;
        gemasSalir.gameObject.SetActive(false);
    }



    private void MetodoPausa()
    {
        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        //---> Modificar el bool de _estaPausado
        _estaPausado = !_estaPausado;

        //---> Llamar metodo ControlSonido()
        ControlSonido();

        if (_estaPausado)
        {
            //---> Pausar la escala del tiempo actual
            Time.timeScale = 0f;

            //---> Reproducir sonido del Menu de Pausa
            _audioManager.PlayMenuPausa(sonidoMenuPausa, volumenMenuPausa);

            //---> Llamar el metodo EstadoReaunudar()
            EstadoPorDefectoReaunudar();

            //---> Mostar el Menu de Pausa
            menuPausa.SetActive(true);
        }
        else
        {
            //---> Reaunudar la escala del tiempo actual
            Time.timeScale = 1f;

            //---> Reproducir sonido Reauudar
            _audioManager.PlaySFX(reaunudar, volumenReaunudar);

            //---> Ocultar la linea Reaunudar
            lineaReaudar.SetActive(false);

            //---> Desactivar el Menu de Pausa
            menuPausa.gameObject.SetActive(false);

            //---> Llamar el metodo DesactivarOpciones()
            DesactivarOpciones();
        }
    }



    //---> Metodo para controlar el Pause() y el UnPause() de la musica y efectos
    public void ControlSonido()
    {
        //---> Llamar metodo para pausar la musica al estar en el menu de Pausa
        _audioManager.PauseMusic(_estaPausado);

        //---> Llamar metodo para pausar los efectos al estar en el menu de Pausa
        _audioManager.PauseSFX(_estaPausado);

        //---> Llamar metodo para pausar los efectos del Mago al estar en el menu de Pausa
        _audioManager.PauseMago(_estaPausado);

        //---> Llamar metodo para pausar los efectos del Mago Oscuro al estar en el menu de Pausa
        _audioManager.PauseMagoOscuro(_estaPausado);

        //---> Llamar metodo para pausar los efectos del Fuego al estar en el menu de Pausa
        _audioManager.PauseFuego(_estaPausado);

        //---> Llamar metodo para pausar los efectos de las Puertas al estar en el menu de Pausa
        _audioManager.PausePuerta(_estaPausado);

        //---> Llamar metodo para pausar los efectos de las Areas de Sonido al estar en el menu de Pausa
        _audioManager.PauseAreaSonido(_estaPausado);

        //---> Llamar metodo para pausar el efecto del Boom al estar en el menu de Pausa
        _audioManager.PauseBoom(_estaPausado);

        //---> Llamar metodo para pausar el efecto del Portal al estar en el menu de Pausa
        _audioManager.PausePortal(_estaPausado);

        //---> Llamar metodo para pausar los efectos de los Dialogos al estar en el menu de Pausa
        _audioManager.PauseDialogo(_estaPausado);
    }




    //---> Cambiar la velocidad del juego
    public void InicioControlTiempo()
    {
        //---> Asiganar el valor de la variable velocidadDelTiempo al Time.timeScale,
        //---> para que la escala del tiempo pase a ser la que se desea 
        Time.timeScale = velocidadDelTiempo;

        //---> Para que los frames se generen a la misma velocidad a la que se esta escalando el tiempo
        Time.fixedDeltaTime = _fixedDeltaTimeInicial * velocidadDelTiempo;
    }



    //---> Restaurar la velocidad del juego
    public void ParoControlTiempo()
    {
        //---> Asignar el valoe almacenado en _escalaDeTiempoInicial al Time.timeScale,
        //---> para devolver la escala del tiempó a su forma original
        Time.timeScale = _escalaDeTiempoInicial;
        Time.fixedDeltaTime = _fixedDeltaTimeInicial;
    }
}
