using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuGameOver : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _estaEnGameOver;
    [HideInInspector] public int _contadorAnyKey = 0;


    [Header("TIEMPOS")]
    private float _tiempoAlpha = 1.5f;
    private float _tiempoGameOver = 1.6f;


    [Header("PUERTA SALIDA")]
    [SerializeField] private Puerta puerta;


    [Header("REFERENCIA DE DIALOGO")]
    private Dialogo _dialogo;


    [Header("REFERENCIA DEL MAGO")]
    private Mago _mago;
    private SaludMago _saludMago;


    [Header("REFERENCIAS DEL MAGO OSCURO")]
    private MagoOscuro _magoOscuro;
    private SaludMagoOscuro _saludMagoOscuro;


    [Header("REFERENCIA ENTRADA DEL MAGO OSCURO")]
    private EntradaDelMagoOscuro _entradaDelMagoOscuro;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip fade;
    [SerializeField] private AudioClip musicaGameOver;
    [SerializeField] private AudioClip latido;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenFade = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenMusicaGameOver = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenLatido = 0.8f;


    [Header("REFERENCIAS UI")]
    [SerializeField] private BarraDeVidaMago barraDeVidaMago;
    [SerializeField] private BarraDeDañoMago barraDeDañoMago;
    [SerializeField] private GameObject textoContinuar;
    [SerializeField] private GameObject menuGameOver;


    [Header("REFERENCIAS")]
    private CanvasGroup _canvasGroup;





    //---> Metodo Set para _estaEnGameOver
    public void setEstaEnGameOver(bool estaEnGameOver)
    {
        this._estaEnGameOver = estaEnGameOver;
    }



    //---> Metodo Get para retornar el valor de la variable _estaEnGameOver
    public bool getEstaEnGameOver()
    {
        return this._estaEnGameOver;
    }



    private void Awake()
    {
        _dialogo = GameObject.Find("LibroExperiencia").GetComponent<Dialogo>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _saludMago = _mago.GetComponent<SaludMago>();
        _magoOscuro = GameObject.FindGameObjectWithTag("MagoOscuro").GetComponent<MagoOscuro>();
        _saludMagoOscuro = _magoOscuro.GetComponent<SaludMagoOscuro>();
        _entradaDelMagoOscuro = GameObject.Find("EntradaDelMagoOscuro").GetComponent<EntradaDelMagoOscuro>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _canvasGroup = menuGameOver.GetComponent<CanvasGroup>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - Menu Game Over
        menuGameOver.SetActive(false);

        //---> Suscribir un metodo al evento MuerteMago
        _saludMago.MuerteMago += ActivarMenu;
    }



    //---> Volver a intentar la batalla contra el Mago Oscuro
    private void Update()
    {
        if (Input.anyKeyDown && _estaEnGameOver)
        {
            //---> Sumar al Contador AnyKey
            _contadorAnyKey += 1;

            if (_contadorAnyKey == 1)
                //---> Llamar la corrutina DesactivarMenu() solo 1 vez
                StartCoroutine(DesactivarMenu());
        }
    }



    //---> Metodo para mostrar el Menu Gradualmente en su Alpha
    public IEnumerator FadeInAlpha(float alphaInicial, float alphaObjetivo, float lerpDuration)
    {
        //---> Llevar la cuenta del tiempo transcurrido contando desde 0
        float tiempoTranscurrido = 0f;

        //---> Mientras el tiempo transcurrido sea menor que la duracion de la transicion
        while(tiempoTranscurrido < lerpDuration)
        {
            /*
                A medida que transcurre el tiempo la división va a pasar de 0 a 1 haciendo que la
                interpolacion del valor inicial al valor final transcurra en (lerpDuration) segundos
                con un Curva de animacion
            */
            _canvasGroup.alpha = Mathf.Lerp(alphaInicial, alphaObjetivo, tiempoTranscurrido / lerpDuration);

            //---> Ir sumando a la variable (tiempoTranscurrido) el tiempo de cada frame
            tiempoTranscurrido += Time.deltaTime;

            //---> Detener la ejecución de la Corrutina hasta el proximo frame
            yield return null;
        }

        //---> Para que al llegar al final le asigne el valor de 1 al Alpha
        _canvasGroup.alpha = alphaObjetivo;

        //---> Indicar que esta entrando al Menu de Game Over
        _estaEnGameOver = true;
    }



    //---> Metodo para activar el Menu Game Over
    private void ActivarMenu(object sender, EventArgs e)
    {
        //---> Reiniciar el Contador AnyKey
        _contadorAnyKey = 0;

        //---> Reproducir la musica del Menu Game Over
        _audioManager.PlayMusic(musicaGameOver, volumenMusicaGameOver, true);

        //---> Activar el Menu Game Over
        menuGameOver.SetActive(true);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Llamar corrutina FadeInAlpha()
        StartCoroutine(FadeInAlpha(0f, 1f, _tiempoAlpha));

        //---> Activar el texto de Presione una tecla para continuar
        textoContinuar.SetActive(true);

        //---> Reproducir sonido de Latido
        _audioManager.PlayLatido(latido, volumenLatido, true);
    }



    //---> Metodo para desactivar el Menu Game Over
    private IEnumerator DesactivarMenu()
    {
        //---> Llamar metodo Revivir() del Script SaludMago
        _saludMago.Revivir();

        //---> Restaurar la vida del Mago Oscuro
        _saludMagoOscuro.SetVidaActual(6);

        //---> Mostrar por la UI el cambio de las vida del Mago Oscuro
        _saludMagoOscuro.ActivarVida();

        //---> Cambiar el estado de la puerta de salida a abierta
        StartCoroutine(puerta.AbrirPuertas(true, false));

        //---> Activar el BoxCollider2D de Puerta
        puerta.GetComponent<BoxCollider2D>().enabled = true;

        //---> Llamar metodo CargarDatos()
        CargarDatos();

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Llamar corrutina FadeInAlpha()
        StartCoroutine(FadeInAlpha(1f, 0f, _tiempoAlpha));

        //---> Desactivar el texto de Presione una tecla para continuar
        textoContinuar.SetActive(false);

        //---> Detener sonido de Latido
        _audioManager.StopLatido();

        //---> Llamar Metodo ReproducirMusicaDeCombate() del Script EntradaDelMagoOscuro
        _entradaDelMagoOscuro.ReproducirMusicaDeCombate();

        //---> Tiempo para inidcar que salio del GameOver
        yield return new WaitForSeconds(_tiempoGameOver);

        //---> Desactivar el Menu Game Over
        menuGameOver.SetActive(false);

        //---> Indicar que esta saliendo del Menu de Game Over
        _estaEnGameOver = false;
    }



    //---> Cargar los datos (DatosHastaCombate) cuando entre en el area
    private void CargarDatos()
    {
        //---> Variable de tipo DatosHastaCombate donde se van a recibir los datos guardados en el archivo
        DatosHastaCombate datosHastaCombate = SaveManagerFight.CargarDatosPelea();

        //---> Cargar los datos del Mago y los estados actuales de los objetos
        _mago.transform.position = new Vector2(datosHastaCombate.positionMago[0], datosHastaCombate.positionMago[1]);
        //---> Restaurar la Salud del Mago guardada
        _saludMago.SetVidaActual(datosHastaCombate.vidaActual);
        //---> Mostrar por la UI el cambio de la barra de vida del Mago
        barraDeVidaMago.CambiarVidaActual(datosHastaCombate.vidaActual);
        //---> Mostrar por la UI el cambio de la barra de daño del Mago
        barraDeDañoMago.CambiarDañoActual(datosHastaCombate.vidaActual);
        //---> Cargar la posicion del MAgo Oscuro
        _magoOscuro.transform.position = new Vector2(datosHastaCombate.positionMagoOscuro[0], datosHastaCombate.positionMagoOscuro[1]);
        //---> Restaurar los objetos que se abrieron
        _dialogo.SetContadorAutorretrado(datosHastaCombate.contadorAutorretrado);
        _dialogo.SetContadorEscudoHabilidades(datosHastaCombate.contadorEscudoHabilidades);
        _dialogo.SetContadorLibroExperiencia(datosHastaCombate.contadorLibroExperiencia);
        // Debug.Log("Datos cargados desde Script: MenuGameOver");
    }
}
