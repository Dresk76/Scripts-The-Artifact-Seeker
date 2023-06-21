using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class EntradaDelMagoOscuro : MonoBehaviour
{
    [Header("TIEMPOS")]
    private float _tiempoDeTipeo = 0.07f;
    private float _tiempoSalidaMagoOscuro = 4f;
    private float _tiempoDialogoMoriras = 2.5f;
    private float _tiempoDialogoSilencio = 1.5f;
    private float _tiempoDialogoElPoderArcano = 3f;


    [Header("PORTAL")]
    [SerializeField] private ParticleSystem particulasPortal;


    [Header("CAMARA")]
    [SerializeField] private CinemachineVirtualCamera camaraMago_2;
    [SerializeField] private CinemachineVirtualCamera camaraPortal;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip fade;
    [SerializeField] private AudioClip cinematicaBoom;
    [SerializeField] private AudioClip portal;
    [SerializeField] private AudioClip moriras;
    [SerializeField] private AudioClip silencio;
    [SerializeField] private AudioClip elPoderArcano;
    [SerializeField] private AudioClip musicaCombate;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenFade = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenCinematicaBoom = 0.25f;
    [Range(0, 1f)][SerializeField] private float volumenPortal = 0.1f;
    [Range(0, 1f)][SerializeField] private float volumenMoriras = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenSilencio = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenElPoderArcano = 0.38f;
    [Range(0, 1f)][SerializeField] private float volumenMusicaCombate = 0.3f;


    [Header("ARRAY DIALOGO")]
    [SerializeField, TextArea(2, 35)] private string[] lineasDeDialogoEspanol;
    [SerializeField, TextArea(2, 35)] private string[] lineasDeDialogoIngles;
    private int _lineaIndex; // Indica que linea de dialogo se esta mostrando


    [Header("AREA DASH")]
    [SerializeField] private GameObject areaDash;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIA DE DIALOGO")]
    private Dialogo _dialogo;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;
    private SaludMago _saludMago;


    [Header("REFERENCIAS DEL MAGO OSCURO")]
    private MagoOscuro _magoOscuro;
    private Animator _animatorMagoOscuro;
    private SpriteRenderer _spriteRendererMagoOscuro;


    [Header("UI REFERENCIAS")]
    [SerializeField] private Image aroJoystick;
    [SerializeField] private Image rellenoJoystick;
    [SerializeField] private GameObject barraDeVidaMago;
    [SerializeField] private GameObject barraDeVidaMagoOscuro;
    [SerializeField] private GameObject menuDeDialogos;
    [SerializeField] private GameObject imgMago;
    [SerializeField] private GameObject imgMagoOscuro;
    [SerializeField] private GameObject controller;
    [SerializeField] private TMP_Text textoDelDialogo;
    private TransicionCamaras _transicionCamaras;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _dialogo = GameObject.Find("LibroExperiencia").GetComponent<Dialogo>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _saludMago = _mago.GetComponent<SaludMago>();
        _magoOscuro = GameObject.FindGameObjectWithTag("MagoOscuro").GetComponent<MagoOscuro>();
        _animatorMagoOscuro = _magoOscuro.GetComponent<Animator>();
        _spriteRendererMagoOscuro = _magoOscuro.GetComponent<SpriteRenderer>();
        _transicionCamaras = GameObject.Find("PanelTransicionCamaras").GetComponent<TransicionCamaras>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar
        /*
            - CamaraPortal
            - Barra de Vida del Mago Oscuro
            - Menu de Dialogos
        */
        camaraPortal.gameObject.SetActive(false);
        barraDeVidaMagoOscuro.SetActive(false);
        menuDeDialogos.SetActive(false);


        //---> Desactivar el spriteRenderer del Mago Oscuro al iniciar
        _spriteRendererMagoOscuro.enabled = false;
    }



    private IEnumerator MostrarEntrada()
    {
        //---> Llamar metodo para girar al Mago hacia el MagoOscuro
        _mago.MirarMagoOscuro();

        //---> Deshabilitar el movimiento del Mago
        _mago.puedeMoverse = false;
        _mago.velocidadObjetivo = Vector3.zero;
        _mago.rb2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);

        //---> Detener la musica del Background
        _audioManager.StopMusic();

        //---> Reproducir sonido de Cinematica boom
        _audioManager.PlayBoom(cinematicaBoom, volumenCinematicaBoom);

        yield return new WaitForSeconds(1f);

        //---> Retirar transparencia del Joystick
        aroJoystick.color = new Color(255f, 255f, 255f, 0f);
        rellenoJoystick.color = new Color(255f, 255f, 255f, 0f);

        //---> Desactivar la Barra de vida del Mago
        barraDeVidaMago.gameObject.SetActive(false);

        //---> Desactivar Controller
        controller.gameObject.SetActive(false);

        //---> Desactivar Camara Principal 2
        camaraMago_2.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        //---> Activar Camara Portal
        camaraPortal.gameObject.SetActive(true);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de entrada de la TransicionCamaras
        _transicionCamaras.IniciarCambioCamara(true);

        yield return new WaitForSeconds(0.7f);

        //---> Hacer animacion del portal
        particulasPortal.Play();

        //---> Reproducir sonido al abrir el portal
        _audioManager.PlayPortal(portal, volumenPortal);

        yield return new WaitForSeconds(_tiempoSalidaMagoOscuro);

        //---> Hacer animacion salir del portal del Mago Oscuro
        _animatorMagoOscuro.SetTrigger("PortalOut");

        //---> Activar el spriteRenderer del Mago Oscuro
        _spriteRendererMagoOscuro.enabled = true;

        //---> Iniciar Corrutina TerminarEntrada
        StartCoroutine(TerminarEntrada());
    }



    //---> Currutina Terminar entrada del Mago Oscuro
    private IEnumerator TerminarEntrada()
    {
        //---> Detener animacion del portal
        particulasPortal.Stop();
        yield return new WaitForSeconds(1f);

        //---> Desactivar Camara Portal
        camaraPortal.gameObject.SetActive(false);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de salida de la TransicionCamaras
        _transicionCamaras.TerminarCambioCamara(true);

        yield return new WaitForSeconds(1f);

        //---> Activar Camara Principal 2
        camaraMago_2.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //---> Iniciar corrutina Dialogo
        StartCoroutine(IniciarDialogos());
    }



    //---> Metodo para Reproducir la musica de Combate
    public void ReproducirMusicaDeCombate()
    {
        _audioManager.PlayMusic(musicaCombate, volumenMusicaCombate, true);
    }



    //---> Mostrar el texto con efecto de tipado
    private IEnumerator MostrarLineaEspanol()
    {
        //---> Borrar lo que haya en el dialogo actual cada vez que pase a un dialogo nuevo
        textoDelDialogo.text = string.Empty;

        foreach (char ch in lineasDeDialogoEspanol[_lineaIndex])
        {
            //---> Concatenar cada carater 1 a 1
            textoDelDialogo.text += ch;

            //---> Tiempo de de tipeo por cada caracter ignorando la escala de tiempo
            yield return new WaitForSeconds(_tiempoDeTipeo);
        }
    }



    //---> Mostrar el texto con efecto de tipado
    private IEnumerator MostrarLineaIngles()
    {
        //---> Borrar lo que haya en el dialogo actual cada vez que pase a un dialogo nuevo
        textoDelDialogo.text = string.Empty;

        foreach (char ch in lineasDeDialogoIngles[_lineaIndex])
        {
            //---> Concatenar cada carater 1 a 1
            textoDelDialogo.text += ch;

            //---> Tiempo de de tipeo por cada caracter ignorando la escala de tiempo
            yield return new WaitForSeconds(_tiempoDeTipeo);
        }
    }



    //---> Corrutina Dialogo entre el Mago Oscuro y el Mago
    private IEnumerator IniciarDialogos()
    {
        //---> Ubicarse en el indice 0 de cada frase
        _lineaIndex = 0;

        //---> Abrir el panel de dialogos
        menuDeDialogos.gameObject.SetActive(true);

        //---> Llamar Metodo ReproducrMusicaDeCombate()
        ReproducirMusicaDeCombate();

        //---> Mostrar el parrafo de los dialogos dependiendo del idioma
        StartCoroutine((_objetosEntreEscenas.idioma == 0) ? MostrarLineaEspanol() : MostrarLineaIngles());



        //---> Dialogo Mago Oscuro - Vas a Morir
        _audioManager.PlayDialogo(moriras, volumenMoriras);

        //---> Activar la imagen del Mago Oscuro
        imgMagoOscuro.gameObject.SetActive(true);

        yield return new WaitForSeconds(_tiempoDialogoMoriras);



        //---> Dialogo Mago - Silencio
        _audioManager.PlayDialogo(silencio, volumenSilencio);

        SiguienteLinea();

        //---> Desactivar la imagen del Mago Oscuro
        imgMagoOscuro.gameObject.SetActive(false);

        //---> Activar la imagen del Mago
        imgMago.gameObject.SetActive(true);

        yield return new WaitForSeconds(_tiempoDialogoSilencio);



        //---> Dialogo Mago - El Poder Arcano es Inconmensurable
        _audioManager.PlayDialogo(elPoderArcano, volumenElPoderArcano);

        SiguienteLinea();

        yield return new WaitForSeconds(_tiempoDialogoElPoderArcano);


        //---> Llamar metodo TerminarDialogos()
        TerminarDialogos();
    }



    //---> Metodo para pasar a la siguiente linea
    private void SiguienteLinea()
    {
        //---> Se incrementa el indice en 1 para pasar a la linea siguiente en cada frase
        _lineaIndex++;

        //---> Si indica que se acabo de actualizar es menor al total de string en el array, vuelve y se llama la corrutina de tipado
        if (_lineaIndex < lineasDeDialogoEspanol.Length || _lineaIndex < lineasDeDialogoIngles.Length)
        {
            //---> Mostrar el parrafo de los dialogos dependiendo del idioma
            StartCoroutine((_objetosEntreEscenas.idioma == 0) ? MostrarLineaEspanol() : MostrarLineaIngles());
        }
    }



    //---> Corrutina Dialogo entre el Mago Oscuro y el Mago
    private void TerminarDialogos()
    {
        //---> Cerrar el panel de dialogos
        menuDeDialogos.gameObject.SetActive(false);

        //---> Desactivar la imagen del Mago Oscuro
        imgMagoOscuro.gameObject.SetActive(false);

        //---> Desactivar la imagen del Mago
        imgMago.gameObject.SetActive(false);

        //---> Habilitar el movimiento del Mago
        _mago.puedeMoverse = true;

        //---> Activar las Barras de vida del Mago y Mago Oscuro
        barraDeVidaMago.gameObject.SetActive(true);
        barraDeVidaMagoOscuro.gameObject.SetActive(true);

        //---> Activar Controller
        controller.gameObject.SetActive(true);

        //---> Cambiar de Layer a Mago Oscuro a la por Default para que pueda haber contacto con el Mago
        _magoOscuro.gameObject.layer = 0;

        //---> Habilitar el movimiento del Mago Oscuro
        _animatorMagoOscuro.SetBool("PuedeCorrer", true);

        //---> Desactivar el BoxCollider2D para que solo se active la camara de portal una vez
        this.GetComponent<BoxCollider2D>().enabled = false;

        //---> Desactivar el Area Dash para que pueda realizarlo al combatir con el Mago Oscuro
        areaDash.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Llamar metodo de GuardarDatos
            GuardarDatos();

            //---> Iniciar corrutina MostrarEntrada()
            StartCoroutine(MostrarEntrada());

            //---> Para testeo
            // StartCoroutine(IniciarDialogos());
        }
    }



    //---> Guardar los datos hasta la primera pelea al entrar en el Area de CambioAPrimeraParte
    private void GuardarDatos()
    {
        //---> Guardar los datos
        SaveManagerFight.GuardarDatoPelea(_mago, _saludMago, _magoOscuro, _dialogo);
        // Debug.Log("Datos guardados desde Script: EntradaDelMagoOscuro");
    }
}
