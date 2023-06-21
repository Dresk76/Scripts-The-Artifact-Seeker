using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuFinPortafolio : MonoBehaviour
{
    [Header("TIEMPOS")]
    private float tiempoLinea = 0.3f;


    [Header("MENU SALIR")]
    [SerializeField] private GameObject menuSalir;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip enter;
    [SerializeField] private AudioClip continuar;
    [SerializeField] private AudioClip reiniciar;
    [SerializeField] private AudioClip salir;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenEnter = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenContinuar = 0.8f;
    [Range(0, 1f)][SerializeField] private float volumenReiniciar = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenSalir = 0.55f;


    [Header("REFERENCIA DEL MAGO")]
    private Mago _mago;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIAS BARRA DE CARGA")]
    [SerializeField] private GameObject menuCarga;
    [SerializeField] private Slider loadBar;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
    }



    //---> Sonido de Mouse over
    public void SonidoEnter()
    {
        _audioManager.PlayEnterUI(enter, volumenEnter);
    }



    //---> Corrutina para mostrar una linea antes de realizar lo de Continuar
    private IEnumerator LineaContinuar(GameObject linea)
    {
        //---> Reproducir sonido de Continuar
        _audioManager.PlaySFX(continuar, volumenContinuar);

        linea.gameObject.SetActive(true);
        //---> Tiempo para mostrar la linea y Continuar en el portafolio
        yield return new WaitForSecondsRealtime(tiempoLinea);
        linea.gameObject.SetActive(false);

        //---> Indicar que esta saliendo al Menu Fin Portafolio
        _objetosEntreEscenas.estaEnFinPortafolio = false;

        //---> Desactivar el menuFinPortafolio
        this.gameObject.SetActive(false);

        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        //---> Habilitar el movimiento del Mago
        _mago.puedeMoverse = true;
    }



    //---> Metodo para Continuar en el portafolio
    public void Continuar(GameObject linea)
    {
        StartCoroutine(LineaContinuar(linea));
    }



    //---> Corrutina para mostrar una linea antes de Reiniciar el portafolio
    private IEnumerator LineaReiniciar(GameObject linea)
    {
        //---> Reproducir sonido de Reiniciar
        _audioManager.PlaySFX(reiniciar, volumenReiniciar);

        linea.gameObject.SetActive(true);
        //---> Tiempo para mostrar la linea y Reniciar el portafolio
        yield return new WaitForSecondsRealtime(tiempoLinea);
        linea.gameObject.SetActive(false);

        //---> Indicar que esta saliendo al Menu Fin Portafolio
        _objetosEntreEscenas.estaEnFinPortafolio = false;

        //---> Activar el Menu de carga
        menuCarga.SetActive(true);

        //---> Llamar corrutina y pasarle la Scena a cargar
        StartCoroutine(LoadAsync(1));
    }



    //---> Metodo para Reiniciar el portafolio
    public void Reiniciar(GameObject linea)
    {
        StartCoroutine(LineaReiniciar(linea));
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
