using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    [Header("CAMBIO A PANELES MENU INICIAL")]
    [SerializeField] private GameObject opcionContinuar;
    [SerializeField] private GameObject panelesMenuInicial;


    [Header("ESTADO POR DEFECTO DE LAS OPCIONES")]
    [SerializeField] private TMP_Text textoPortafolio;
    [SerializeField] private GameObject gemaPortafolio;
    [SerializeField] private TMP_ColorGradient colorOpcMoradoClaro;


    [Header("REFERENCIAS")]
    [SerializeField] private GameObject menuOpciones;
    [SerializeField] private GameObject menuCreditos;
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIAS AUDIO")]
    private AudioManager _audioManager;
    [SerializeField] private AudioClip cualquierBoton;
    [SerializeField] private AudioClip musicMenuInicial;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenCualquierBoton = 0.5f;
    [Range(0, 1f)][SerializeField] private float volumenMusicMenuInicial = 0.3f;


    [Header("REFERENCIA MENU DE CARGA")]
    [SerializeField] private GameObject menuCarga;
    [SerializeField] private Slider loadBar;





    private void Awake()
    {
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu de Carga
        menuCarga.SetActive(false);

        //---> Llamar Metodo OpcionesPorDefectoMenuInicial()
        OpcionesPorDefectoMenuInicial();
    }



    private void Update()
    {
        //---> Metodo para controlar el volumen del Menu Inicial pasando como parametros si esta pausado y el volumen para que lo disminuya o lo aumente segun el estado de Pausa
        _audioManager.ControlMusicMenuInicialUI(_objetosEntreEscenas.estaEnOpciones, volumenMusicMenuInicial);

        //---> Si presiona cualquier tecla mostrar panelesMenuInicial y desactivar presionar cualquier boton
        if (Input.anyKeyDown && !panelesMenuInicial.activeSelf)
        {
            //---> Reproducir un sonido al presionar cualquier boton
            _audioManager.PlaySFX(cualquierBoton, volumenCualquierBoton);

            //---> Desactivar el texto de Presionar cualquier boton
            opcionContinuar.gameObject.SetActive(false);

            //---> Activar el panel del Menu Inicial
            panelesMenuInicial.gameObject.SetActive(true);

            //---> Reproducir la musica del Menu Inicial
            _audioManager.PlayMusicMenuInicialUI(musicMenuInicial, volumenMusicMenuInicial, true);
        }
    }



    //---> Meotodo para iniciar mostrar al iniciar el texto de presionar para continuar y la opcion de portafolio señalada
    public void OpcionesPorDefectoMenuInicial()
    {
        //---> Iniciar la opcion de presionar cualquier boton en true y los panelesMenuInicial en false
        opcionContinuar.gameObject.SetActive(true);
        panelesMenuInicial.gameObject.SetActive(false);

        //---> Inciar señalado por defecto la opcion de Portafolio
        textoPortafolio.colorGradientPreset = colorOpcMoradoClaro;
        gemaPortafolio.gameObject.SetActive(true);
    }



    //---> Metodo para iniciar el Portafolio
    public void Portafolio(int sceneIndex)
    {
        //---> Activar el Menu de carga
        menuCarga.SetActive(true);

        //---> Llamar corrutina y pasarle la Scena a cargar
        StartCoroutine(LoadAsync(sceneIndex));
    }



    //---> Corrutina para que mientras se hace la carga de la escena se actualice continuamente el valor del Slider
    private IEnumerator LoadAsync(int sceneIndex)
    {
        /*
            (LoadSceneAsync):
            Para que no pause la ejecucion del juego para cargar la escena, sino que mientras 
            el juego corra se cargue la ejecucion de la escena en segundo plano.
        */
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        //---> Booleano (isDone) Para actualizar constantemente el Slider mientras no se haya cargado aun la Scena
        while(!asyncOperation.isDone)
        {
            //---> Imprimir por consola el valor del progreso
            // Debug.Log(asyncOperation.progress);

            //---> Actualizar el valor del Slider para mostrar el progreso de carga
            loadBar.value = asyncOperation.progress / 0.9f;

            /*
                Pausar la ejecución hasta el proximo frame y si en el proximo frame no ha terminado, se va a 
                actualizar nuevamente el valor del Slider y asi hasta que se cargue por completo la Scena
            */
            yield return null;
        }


         //---> Desactivar al cargarse por completo la Scena
        if (asyncOperation.isDone)
        {
            //---> Desactivar el Menu de Carga
            menuCarga.SetActive(false);

            //---> Desactivar el Menu de Inicio
            this.gameObject.SetActive(false);

            //---> Detener la musica del Menu Inicial
            _audioManager.StopMusicMenuInicialUI();

            //---> Llamar Metodo OpcionesPorDefectoMenuInicial()
            OpcionesPorDefectoMenuInicial();
        }
    }



    //---> Metodo Opciones
    public void Opciones()
    {
        //---> Abrir el Menu de opciones
        menuOpciones.SetActive(true);

        //---> Indicar que esta entrando al Menu de opciones
        _objetosEntreEscenas.estaEnOpciones = true;
    }



    //---> Metodo para salir del portafolio
    public void Salir()
    {
        //---> Salir del Portafolio
        Application.Quit();

        Debug.Log("Salir");

        //---> Detener la musica del Menu Inicial
        _audioManager.StopMusicMenuInicialUI();
    }
}
