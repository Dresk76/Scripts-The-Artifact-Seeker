using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaludMagoOscuro : MonoBehaviour
{
    [Header("SALUD MAGO OSCURO")]
    [SerializeField] private int vidaMaxima = 6;
    [SerializeField] private int vidaActual;


    [Header("TIEMPOS")]
    private float _tiempoDetenerMusica = 3f;
    private float _tiempoMuerteMagoOscuro = 3f;
    private float _tiempoMusicaFinal = 5f;
    private float _tiempoVictoriaMago = 2f;
    private float _tiempoAperturaPuerta = 0.2f;
    private float _tiempoDeTipeo = 0.08f;


    [Header("PUERTA SALIDA")]
    [SerializeField] private Puerta puerta;


    [Header("PARTICULAS GOLPE")]
    [SerializeField] private ParticleSystem particulasGolpe;


    [Header("COLOR DAÑO")]
    [SerializeField] private Color _colorDaño;
    private Color _colorInicial;
    private float _tiempoColor = 0.4f;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip gemido;
    [SerializeField] private AudioClip golpe;
    [SerializeField] private AudioClip muerte;
    [SerializeField] private AudioClip mitadDeVida;
    [SerializeField] private AudioClip tuFinEstaCerca;
    [SerializeField] private AudioClip victoriaMago;
    [SerializeField] private AudioClip musicaFinal;
    [SerializeField] private AudioClip musicaBackground;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenGemido = 0.8f;
    [Range(0, 1f)][SerializeField] private float volumenGolpe = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenMuerte = 0.35f;
    [Range(0, 1f)][SerializeField] private float volumenMitadDeVida = 0.45f;
    [Range(0, 1f)][SerializeField] private float volumenTuFinEstaCerca = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenVictoriaMago = 0.5f;
    [Range(0, 1f)][SerializeField] private float volumenMusicaFinal = 0.27f;
    [Range(0, 1f)][SerializeField] private float volumenMusicaBackground = 0.3f;


    [Header("ARRAY DIALOGO")]
    [SerializeField, TextArea(2, 35)] private string[] lineasDeDialogoEspanol;
    [SerializeField, TextArea(2, 35)] private string[] lineasDeDialogoIngles;
    private int _lineaIndex; // Indica que linea de dialogo se esta mostrando


    [Header("REFERENCIA OBJETO ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIAS UI")]
    [SerializeField] private GameObject[] vidas;
    [SerializeField] private GameObject barraDeVidaMagoOscuro;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject menuDeDialogos;
    [SerializeField] private GameObject imgVictoryMago;
    [SerializeField] private TMP_Text textoDelDialogo;


    [Header("REFERENCIAS")]
    [SerializeField] private CapsuleCollider2D capsuleColliderMagoOscuro;
    [SerializeField] private CapsuleCollider2D hurtBoxMagoOscuro;
    private MagoOscuro _magoOscuro;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;





    //---> Modificar la vidaActual por un metodo set
    public void SetVidaActual(int nuevaVida)
    {
        this.vidaActual = nuevaVida;
    }



    //---> Pasar la vidaActual por un metodo get
    public int GetVidaActual()
    {
        return this.vidaActual;
    }



    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _magoOscuro = GetComponent<MagoOscuro>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void Start()
    {
        //---> Salud actual del Mago Oscuro
        vidaActual = vidaMaxima;

        //---> Guardar color inicial del Mago Oscuro
        _colorInicial = _spriteRenderer.color;
    }



    //---> Salud del Mago Oscuro
    public void TomarDaño(int daño, Vector2 puntoDeContacto, Vector2 normal)
    {
        //---> Restar el daño a la salud si es mayor a 0 y que no siga restando
        if (vidaActual > 0)
        {
            vidaActual -= daño;
        }

        //---> Mostrar por la UI el cambio de las vidas del Mago Oscuro
        DesactivarVida(vidaActual);


        if (vidaActual == 3)
        {
            //---> Metodo de Golpe
            Golpe();

            //---> Sonido del Mago Oscuro al llegar a la mitad de la vida
            _audioManager.PlayDialogo(mitadDeVida, volumenMitadDeVida);

            //---> Realizar el rebote
            _magoOscuro.ReboteGolpe(puntoDeContacto, normal);
        }
        else if(vidaActual == 1)
        {
            //---> Metodo de Golpe
            Golpe();

            //---> Sonido del Mago al tener la ultima vida el Mago Oscuro
            _audioManager.PlayDialogo(tuFinEstaCerca, volumenTuFinEstaCerca);

            //---> Realizar el rebote
            _magoOscuro.ReboteGolpe(puntoDeContacto, normal);
        }
        else if (vidaActual > 0)
        {
            //---> Metodo de Golpe
            Golpe();

            //---> Realizar el rebote
            _magoOscuro.ReboteGolpe(puntoDeContacto, normal);
        }
        else
        {
            //---> Corrutina de Muerte
            StartCoroutine(Muerte());
        }
    }



    //---> Metodo desactiva la vida en el índice especificado por (indice) en la UI
    private void DesactivarVida(int indice)
    {
        vidas[indice].SetActive(false);
    }



    //---> Metodo para activar todas las vidas del Mago Oscuro en la UI
    public void ActivarVida()
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].SetActive(true);
        }
    }



    //---> Metodo a realizar al recibir daño
    private void Golpe()
    {
        //---> Particulas de golpe
        particulasGolpe.Play();

        //---> Hacer animacion de golpe del Mago Oscuro
        _animator.SetTrigger("Golpe");

        //---> Reproducir sonido de gemido del Mago Oscuro
        _audioManager.PlayMagoOscuro(gemido, volumenGemido);

        //---> Reproducir sonido de Golpe del Mago
        _audioManager.PlaySFX(golpe, volumenGolpe);

        //---> Cambio de color del Mago con el daño
        StartCoroutine(RespuestaVisual());
    }



    //---> Cambio de color del Mago con el daño
    private IEnumerator RespuestaVisual()
    {
        //---> Cambiar el color del Mago Oscuro
        _spriteRenderer.color = _colorDaño;

        yield return new WaitForSeconds(_tiempoColor);

        //---> Cambiar el color del Mago Oscuro al inicial
        _spriteRenderer.color = _colorInicial;
    }



    //---> Metodo para retirar el Material del Mago y Mago Oscuro
    private void RetirarMaterial()
    {
        capsuleColliderMagoOscuro.sharedMaterial = null;
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
    private IEnumerator IniciarDialogo()
    {
        //---> Ubicarse en el indice 0 de cada frase
        _lineaIndex = 0;

        //---> Abrir el panel de dialogos
        menuDeDialogos.gameObject.SetActive(true);

        //---> Mostrar el parrafo de los dialogos dependiendo del idioma
        StartCoroutine((_objetosEntreEscenas.idioma == 0) ? MostrarLineaEspanol() : MostrarLineaIngles());

        //---> Dialogo del Mago al Ganar
        _audioManager.PlayDialogo(victoriaMago, volumenVictoriaMago);

        //---> Activar la imagen de Victory Mago
        imgVictoryMago.gameObject.SetActive(true);

        //---> Tiempo grito de Victoria del Mago
        yield return new WaitForSeconds(_tiempoVictoriaMago);

        //---> Llamar corrutina TerminarDialogo()
        StartCoroutine(TerminarDialogo());
    }



    //---> Corrutina para terminar el dialogo de Muerte del Mago
    private IEnumerator TerminarDialogo()
    {
        //---> Cerrar el panel de dialogos
        menuDeDialogos.gameObject.SetActive(false);

        //---> Desactivar la imagen de Victory Mago
        imgVictoryMago.gameObject.SetActive(false);

        //---> Activar Controller
        controller.gameObject.SetActive(true);

        //---> Reproducir musica del background
        _audioManager.PlayMusic(musicaBackground, volumenMusicaBackground, true);

        //---> Tiempo apertura de puerta
        yield return new WaitForSeconds(_tiempoAperturaPuerta);

        //---> Metodo AbrirPuertaSalida()
        AbrirPuertaSalida();
    }



    //---> Metodo a realizar al morir
    private IEnumerator Muerte()
    {
        //---> Llamar metodo RetirarMaterial()
        RetirarMaterial();

        //---> Animacion de muerte del Mago Oscuro
        _animator.SetTrigger("Muerte");

        //---> Cambiar de Layer al Mago Oscuro para que el Mago lo pueda atravesar cuando muera
        this.gameObject.layer = LayerMask.NameToLayer("MagoOscuro");

        //---> Desactivar el HutBox del Mago Oscuro para que el Mago al atacar ya no haga contacto
        hurtBoxMagoOscuro.gameObject.SetActive(false);

        //---> Desactivar la Barra de vida del Mago Oscuro
        barraDeVidaMagoOscuro.SetActive(false);

        //---> Sonido del Mago Oscuro al Morir
        _audioManager.PlayDialogo(muerte, volumenMuerte);

        //---> Detener la musica de Combate gradualmente
        StartCoroutine(_audioManager.FadeOutMusic(0.3f, 0f, _tiempoDetenerMusica));

        //---> Tiempo Dialogo Muerte Mago Oscuro
        yield return new WaitForSeconds(_tiempoMuerteMagoOscuro);

        //---> Reproducir Musica Final
        _audioManager.PlayMusic(musicaFinal, volumenMusicaFinal, false);

        //---> Tiempo Musica Final
        yield return new WaitForSeconds(_tiempoMusicaFinal);

        //---> Desactivar Controller
        controller.SetActive(false);

        //---> Llamar corrutina IniciarDialogo()
        StartCoroutine(IniciarDialogo());
    }



    //---> Metodo para abrir la puerta de salida al derrotar al mago en caso de que se cierre
    private void AbrirPuertaSalida()
    {
        if (puerta.GetComponent<BoxCollider2D>().enabled == true)
        {
            //---> En caso de que no haya ido a la salida
            // desactivar el BoxCollider2D para que solo se cierre una vez
            puerta.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            //---> Cambiar el estado de la puerta de salida a abierta
            StartCoroutine(puerta.AbrirPuertas(true, true));
        }
    }
}
