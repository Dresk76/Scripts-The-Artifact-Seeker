using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSalir : MonoBehaviour
{
    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip cancelar;
    [SerializeField] private AudioClip confirmar;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenCancelar = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenConfirmar = 0.55f;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("REFERENCIAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;
    private GameObject _menuPausa;


    [Header("REFERENCIAS BARRA DE CARGA")]
    [SerializeField] private GameObject menuCarga;
    [SerializeField] private Slider loadBar;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _menuPausa = GameObject.FindGameObjectWithTag("MenuPausa");
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu de Carga
        menuCarga.SetActive(false);
    }



    private void Update()
    {
        //---> Cancelar al dar Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //---> Llamar Metodo Cancelar
            Cancelar();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //---> Llamar Metodo Confirmar
            Confirmar();
        }
    }



    //---> Metodo para Cancelar el Salir del Portafolio
    public void Cancelar()
    {
        //---> Reproducir sonido de Cencelar
        _audioManager.PlaySFX(cancelar, volumenCancelar);

        //---> Desactivar el Menu Salir
        this.gameObject.SetActive(false);

        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        //---> Indicar que esta saliendo del Menu Salir
        _objetosEntreEscenas.estaEnSalir = false;
    }



    //---> Metodo para volver a la escena del Menu Inicial
    public void Confirmar()
    {
        //---> Reaunudar la escala del tiempo actual
        Time.timeScale = 1f;

        //---> Reproducir sonido de Salir
        _audioManager.PlaySFX(confirmar, volumenConfirmar);

        //---> Detener la Musica para que se inicie de nuevo al volver a entrar en el portafolio
        _audioManager.StopMusic();

        //---> Indicar que esta saliendo del Menu Salir
        _objetosEntreEscenas.estaEnSalir = false;

        //---> Activar el Menu Inicial
        _objetosEntreEscenas.menuInicial.SetActive(true);

        //---> Activar el Menu de carga
        menuCarga.SetActive(true);

        //---> Llamar corrutina y pasarle la Scena a cargar
        StartCoroutine(LoadAsync(0));
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
    }
}
