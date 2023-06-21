using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SaludMago : MonoBehaviour
{
    //---> Evento MuerteMago
    public event EventHandler MuerteMago;

    [Header("SALUD MAGO")]
    [SerializeField] private int vidaMaxima = 10;
    [SerializeField] private int vidaInicial = 0;
    [SerializeField] private int vidaActual;


    [Header("REBOTE POR GOLPE")]
    // Para el jugador no se mueva mientras esta siendo impactado
    private float _tiempoPerdidaControl = 1f;


    [Header("MATERIAL")]
    [SerializeField] private PhysicsMaterial2D materialResbaladizo;
    private PhysicsMaterial2D _materialInicialMago;


    [Header("COLOR DAÑO")]
    [SerializeField] private Color _colorDaño;
    private Color _colorInicial;
    private float _tiempoColor = 0.4f;


    [Header("TIEMPOS")]
    private float _tiempoDetenerMusica = 6.5f;
    private float _tiempoDaño = 0.5f; // TIEMPO DE EFECTO DAÑO EN LA BARRA DE VIDA
    private float _tiempoGameOver = 5f; // TIEMPO MOSTRAR GAME OVER
    private float _tiempoDeTipeo = 0.12f;


    [Header("PARTICULAS GOLPE")]
    [SerializeField] private ParticleSystem particulasGolpe;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip vidaNuevaBarraDeVida;
    [SerializeField] private AudioClip gemido;
    [SerializeField] private AudioClip golpe;
    [SerializeField] private AudioClip muerteConDialogo;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenVidaNuevaBarraDeVida = 0.38f;
    [Range(0, 1f)][SerializeField] private float volumenGemido = 1f;
    [Range(0, 1f)][SerializeField] private float volumenGolpe = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenMuerteConDialogo = 0.8f;


    [Header("ARRAY DIALOGO")]
    [SerializeField, TextArea(2, 35)] private string[] lineasDeDialogoEspanol;
    [SerializeField, TextArea(2, 35)] private string[] lineasDeDialogoIngles;
    private int _lineaIndex; // Indica que linea de dialogo se esta mostrando


    [Header("REFERENCIA OBJETO ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIAS DEL MAGO OSCURO")]
    private MagoOscuro _magoOscuro;
    private Animator _animatorMagoOscuro;


    [Header("REFERENCIAS UI")]
    [SerializeField] private BarraDeVidaMago barraDeVidaMago;
    [SerializeField] private BarraDeDañoMago barraDeDañoMago;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject menuDeDialogos;
    [SerializeField] private GameObject imgMuerteMago;
    [SerializeField] private TMP_Text textoDelDialogo;
    [SerializeField] private Image aroJoystick;
    [SerializeField] private Image rellenoJoystick;


    [Header("REFERENCIAS")]
    [SerializeField] private CapsuleCollider2D capsuleColliderMago;
    [SerializeField] private CapsuleCollider2D capsuleColliderMagoOscuro;
    private Mago _mago;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private SpriteRenderer _spriteRenderer;
    private CombateMago _combateMago;





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
        _magoOscuro = GameObject.FindGameObjectWithTag("MagoOscuro").GetComponent<MagoOscuro>();
        _animatorMagoOscuro = _magoOscuro.GetComponent<Animator>();
        _mago = GetComponent<Mago>();
        _animator = GetComponent<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _combateMago = GetComponent<CombateMago>();
    }



    private void Start()
    {
        //---> Inicializar la salud actual del Mago
        vidaActual = vidaInicial;

        //---> Barra de vida
        barraDeVidaMago.InicializarBarraDeVida(vidaMaxima, vidaActual);

        //---> Barra de daño
        barraDeDañoMago.InicializarBarraDeDaño(vidaMaxima, vidaActual);

        //---> Guardar color inicial del Mago
        _colorInicial = _spriteRenderer.color;

        //---> Guardar el material inicial del Mago
        _materialInicialMago = capsuleColliderMago.sharedMaterial;
    }



    //---> Aumentar la salud del Mago
    public void TomarVidas(int nuevaVida)
    {
        //---> Si las vidas estan al maximo hace un return para que el resto del codigo no se ejecute
        if (vidaActual == vidaMaxima)
        {
            return;
        }

        //---> Iniciar corrutina VidaNueva
        StartCoroutine(VidaNueva(nuevaVida));
    }



    //---> Disminuir la salud del Mago
    public void TomarDaño(int daño, Vector2 puntoDeContacto, Vector2 normal)
    {
        //---> Hasta que sea mayor a 1 ya que primero el Mago Oscuro hace el golpe y despues de valida
        if (vidaActual > 1)
        {
            //---> Metodo de Golpe
            Golpe(daño);

            //---> Realizar el rebote
            _combateMago.ReboteGolpe(puntoDeContacto, normal);
        }
        else
        {
            //---> Corrutina de Muerte
            Muerte(daño);
        }
    }



    //---> Corrutina para mostrar el daño en la UI
    private IEnumerator MostrarDañoUI()
    {
        //---> Mostrar por la UI el cambio de la barra de vida
        barraDeVidaMago.CambiarVidaActual(vidaActual);

        yield return new WaitForSeconds(_tiempoDaño);

        //---> Mostrar por la UI el cambio de la barra de daño
        barraDeDañoMago.CambiarDañoActual(vidaActual);
    }



    //---> Corrutina para el manejo de una nueva vida del Mago
    private IEnumerator VidaNueva(int nuevaVida)
    {
        //---> Sumar la nueva vida a la salud
        vidaActual += nuevaVida;

        //---> Reproducir sonido al tomar una nueva vida en la barra de vida
        _audioManager.PlaySFX(vidaNuevaBarraDeVida, volumenVidaNuevaBarraDeVida);

        yield return new WaitForSeconds(0.3f);

        //---> Mostrar por la UI el cambio de la barra de vida
        barraDeVidaMago.CambiarVidaActual(vidaActual);

        //---> Mostrar por la UI el cambio de la barra de daño
        barraDeDañoMago.CambiarDañoActual(vidaActual);
    }



    //---> Metodo a realizar al recibir daño
    private void Golpe(int daño)
    {
        //---> Restar el daño a la salud actual si es mayor a 0 y que no siga restando
        vidaActual -= daño;

        //---> Reproducir sonido de gemido del Mago
        _audioManager.PlayMago(gemido, volumenGemido);

        //---> Reproducir sonido de golpe del Mago
        _audioManager.PlaySFX(golpe, volumenGolpe);

        //---> Mostrar por la UI el cambio de la barra de vida y de daño
        StartCoroutine(MostrarDañoUI());

        //---> Particulas de golpe
        particulasGolpe.Play();

        //---> Animacion de golpe
        _animator.SetTrigger("Golpe");

        //---> Perder el control del Mago
        StartCoroutine(PerderControl());

        //---> Cambio de color del Mago con el daño
        StartCoroutine(RespuestaVisual());
    }



    //---> Perder el control del Mago al ser impactado
    private IEnumerator PerderControl()
    {
        _mago.puedeMoverse = false;

        yield return new WaitForSeconds(_tiempoPerdidaControl);

        _mago.puedeMoverse = true;
    }



    //---> Cambio de color del Mago con el daño
    private IEnumerator RespuestaVisual()
    {
        //---> Cambiar el color del Mago
        _spriteRenderer.color = _colorDaño;

        yield return new WaitForSeconds(_tiempoColor);

        //---> Cambiar el color del Mago al inicial
        _spriteRenderer.color = _colorInicial;
    }



    //---> Metodo para retirar el Material del Mago y Mago Oscuro
    private void RetirarMaterial()
    {
        capsuleColliderMago.sharedMaterial = null;
        capsuleColliderMagoOscuro.sharedMaterial = null;
    }



    //---> Metodo para Asignar el Material del Mago y Mago Oscuro
    private void AsignarMaterial()
    {
        capsuleColliderMago.sharedMaterial = _materialInicialMago;
        capsuleColliderMagoOscuro.sharedMaterial = materialResbaladizo;
    }



    //---> Metodo a realizar al morir
    private void Muerte(int daño)
    {
        //---> Llamar metodo RetirarMaterial()
        RetirarMaterial();

        //---> Restringir el movimiento del Mago al morir
        _mago.puedeMoverse = false;

        //---> Restringir el movimiento del Mago Oscuro al morir
        _magoOscuro.puedeMoverse = false;

        //---> Dejar al Mago Oscuro en estado de Idle al morir el Mago
        _magoOscuro.GetComponent<Animator>().SetBool("PuedeCorrer", false);

        //---> Restar el daño a la salud actual si es mayor a 0 y que no siga restando
        vidaActual -= daño;

        //---> Cambio de color del Mago con el daño
        StartCoroutine(RespuestaVisual());

        //---> Mostrar por la UI el cambio de la barra de vida y de daño
        StartCoroutine(MostrarDañoUI());

        //---> Detener la musica de Combate gradualmente
        StartCoroutine(_audioManager.FadeOutMusic(0.3f, 0f, _tiempoDetenerMusica));

        //---> Animacion de muerte del Mago
        _animator.SetTrigger("Muerte");

        //---> Desactivar Controller
        controller.SetActive(false);

        //---> Llamar corrutina IniciarDialogo()
        StartCoroutine(IniciarDialogo());
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
        yield return new WaitForSeconds(0.1f);

        //---> Ubicarse en el indice 0 de cada frase
        _lineaIndex = 0;

        //---> Abrir el panel de dialogos
        menuDeDialogos.gameObject.SetActive(true);

        //---> Mostrar el parrafo de los dialogos dependiendo del idioma
        StartCoroutine((_objetosEntreEscenas.idioma == 0) ? MostrarLineaEspanol() : MostrarLineaIngles());

        //---> Dialogo del Mago al Morir
        _audioManager.PlayDialogo(muerteConDialogo, volumenMuerteConDialogo);

        //---> Activar la imagen de Muerte Mago
        imgMuerteMago.gameObject.SetActive(true);

        //---> Tiempo para la siguiente linea
        yield return new WaitForSeconds(1.5f);

        SiguienteLinea();

        yield return new WaitForSeconds(_tiempoGameOver);

        //---> Llamar metodo TerminarDialogo()
        TerminarDialogo();
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



    //---> Corrutina para terminar el dialogo de Muerte del Mago
    private void TerminarDialogo()
    {
        //---> Cerrar el panel de dialogos
        menuDeDialogos.gameObject.SetActive(false);

        //---> Desactivar la imagen de Muerte Mago
        imgMuerteMago.gameObject.SetActive(false);

        //---> Activar Controller
        controller.SetActive(true);

        //---> Llamar el evento MuerteMago cuando el Mago muera
        MuerteMago?.Invoke(this, EventArgs.Empty);
    }



    //---> Metodo a realizar para cuando intente de nuevo el Combate
    public void Revivir()
    {
        //---> Llamar metodo AsignarMaterial()
        AsignarMaterial();

        //---> Pasar al Mago al estado de Revivir
        _animator.SetTrigger("Revivir");

        //---> Para que se mueva de nuevo el Mago
        _mago.puedeMoverse = true;

        //---> Restaurar el estado Idle del Mago Oscuro
        _animatorMagoOscuro.SetBool("PuedeCorrer", true);

        //---> Pasar al Mago Oscuro al estado de iniciar
        _animatorMagoOscuro.SetTrigger("Iniciar");

        //---> Para que se mueva de nuevo el Mago Oscuro
        _magoOscuro.puedeMoverse = true;
    }
}
