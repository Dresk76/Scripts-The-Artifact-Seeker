using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [Header("CONDICIONES")]
    [SerializeField] private int sonidoDeCaracteres = 510;
    private bool _magoEnRango;
    private bool _comenzoElDialogo; // Indica si comenzo o termino el dialogo


    [Header("TIEMPOS")]
    [SerializeField] private float tiempoDeCartel = 0.7f;
    [SerializeField] private float tiempoIniciarTexto = 1f;
    [SerializeField] private float tiempoDeTipeo = 0.02f;
    [SerializeField] private float tiempoEntrePaginas = 0.4f;
    [SerializeField] private float tiempoVidaNueva = 0.7f;
    [SerializeField] private float tiempoAbrirPuerta = 0.8f;
    [SerializeField] private float tiempoMoverseMago = 1f;


    [Header("TECLA DE INTERACCION")]
    [SerializeField] private GameObject teclaInteractiva;


    [Header("LUZ")]
    [SerializeField] private GameObject luz;


    [Header("ANIMATOR LIBRO")]
    [SerializeField] private Animator animatorLibro;


    [Header("ARRAY CARTEL")]
    [SerializeField, TextArea(5, 90)] private string[] lineasDeDialogoEspanol;
    [SerializeField, TextArea(5, 90)] private string[] lineasDeDialogoIngles;
    private int _lineaIndex; // Indica que linea de dialogo se esta mostrando


    [Header("TITULOS")]
    [SerializeField] private string tituloEspanol;
    [SerializeField] private string tituloIngles;


    [Header("DICCIONARIO HABILITAR ESCENARIO")]
    private HabilitarEscenario _habilitarEscenario;


    [Header("CONTADOR OBJETOS")]
    private float contadorAutorretrado;
    private float contadorEscudoHabilidades;
    private float contadorLibroExperiencia;


    [Header("PUERTA")]
    [SerializeField] private Puerta puerta;


    [Header("REFERENCIA COLLIDER CETRO DE MAGO")]
    [SerializeField] private CircleCollider2D circleCollider2D;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip abrirCartel;
    [SerializeField] private AudioClip escribir;
    [SerializeField] private AudioClip completarParrafo;
    [SerializeField] private AudioClip pasarPagina;
    [SerializeField] private AudioClip cerrarCartel;
    [SerializeField] private AudioClip particulasVidaNueva;
    private AudioSource _audioSource;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenAbrirCartel = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenEscribir = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenCompletarParrafo = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenPasarPagina = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenCerrarCartel = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenParticulasVidaNueva = 0.5f;


    [Header("PARTICULAS VIDA")]
    [SerializeField] private ParticleSystem particulasVida;


    [Header("ANIMATOR")]
    [SerializeField] private Animator _animatorCartel;


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;
    private CapsuleCollider2D _capsuleColliderMago;
    private Animator _animatorMago;
    private SaludMago _saludMago;


    [Header("UI REFERENCIAS")]
    [SerializeField] private GameObject menuCartel;
    [SerializeField] private GameObject PanelTeclaInteractiva;
    [SerializeField] private TMP_Text textoDelCartel;
    [SerializeField] private TMP_Text textoTitulo;





    //---> Metodo Set para contadorAutorretrado
    public void SetContadorAutorretrado(float contadorAutorretrado)
    {
        this.contadorAutorretrado = contadorAutorretrado;
    }



    //---> Metodo Get para contadorAutorretrado
    public float GetContadorAutorretrado()
    {
        return this.contadorAutorretrado;
    }



    //---> Metodo Set para contadorEscudoHabilidades
    public void SetContadorEscudoHabilidades(float contadorEscudoHabilidades)
    {
        this.contadorEscudoHabilidades = contadorEscudoHabilidades;
    }



    //---> Metodo Get para contadorEscudoHabilidades
    public float GetContadorEscudoHabilidades()
    {
        return this.contadorEscudoHabilidades;
    }



    //---> Metodo Set para contadorLibroExperiencia
    public void SetContadorLibroExperiencia(float contadorLibroExperiencia)
    {
        this.contadorLibroExperiencia = contadorLibroExperiencia;
    }



    //---> Metodo Get para contadorLibroExperiencia
    public float GetContadorLibroExperiencia()
    {
        return this.contadorLibroExperiencia;
    }



    private void Awake()
    {
        _habilitarEscenario = GameObject.Find("HabilitarEscenario").GetComponent<HabilitarEscenario>();
        _audioSource = GetComponent<AudioSource>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _capsuleColliderMago = _mago.GetComponent<CapsuleCollider2D>();
        _saludMago = _mago.GetComponent<SaludMago>();
        _animatorMago = _mago.GetComponent<Animator>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar
        /*
            - Menu Cartel
            - Tecla Interactiva
            - luz
        */
        menuCartel.SetActive(false);
        teclaInteractiva.SetActive(false);
        luz.SetActive(false);
    }



    void Update()
    {
        //---> Si esta activo el menu de Pausa no puede realizar nada en el update y pausa el sonido de los Objetos
        if (_controlDelTiempo.getEstaPausado())
        {
            _audioSource.Pause();
            return;
        }
        else
        {
            _audioSource.UnPause();
        }


        if (_magoEnRango && Input.GetButtonDown("Fire1") && _mago.enSuelo)
        {
            //---> Si no ha iniciado el dialogo, se inicia
            if (!_comenzoElDialogo)
            {
                StartCoroutine(AbrirCartel());
            }
            //---> Si el string del array en el indice 0 se termino ya de tipear
            else if (textoDelCartel.text == lineasDeDialogoEspanol[_lineaIndex] || textoDelCartel.text == lineasDeDialogoIngles[_lineaIndex])
            {
                StartCoroutine(SiguienteLinea());
            }
            //---> Si la linea actual no se ha terminado de tipear, se detiene el tipado y se muestra la linea completa
            else
            {
                StopAllCoroutines();
                //---> Activar el PanelTeclaInteractiva
                PanelTeclaInteractiva.gameObject.SetActive(true);

                //---> Detener el sonido de tipeo y reproducir el de (completarParrafo)
                _audioManager.StopSFX();
                _audioManager.PlaySFX(completarParrafo, volumenCompletarParrafo);

                //---> Agregar el itulo del cartel dependiendo el idioma
                textoTitulo.text = (_objetosEntreEscenas.idioma == 0) ? tituloEspanol : tituloIngles;

                //---> Completar todo el parrafo dependiendo el idioma
                textoDelCartel.text = (_objetosEntreEscenas.idioma == 0) ? lineasDeDialogoEspanol[_lineaIndex] : lineasDeDialogoIngles[_lineaIndex];
            }
        }
    }



    private IEnumerator AbrirCartel()
    {
        //---> Iniciar con el cartel vacio
        textoDelCartel.text = string.Empty;

        //---> Desactivar la tecla interactiva
        teclaInteractiva.SetActive(false);

        //---> Detener el sonido de los objetos
        _audioSource.Stop();

        //---> Tiempo para abrir el cartel despues de hacer el ataque
        yield return new WaitForSeconds(tiempoDeCartel);

        //---> Restringir la animacion del libro
        animatorLibro.enabled = false;

        //---> Agregar el itulo del cartel dependiendo el idioma
        textoTitulo.text = (_objetosEntreEscenas.idioma == 0) ? tituloEspanol : tituloIngles;

        //---> Ubicarse en el indice 0 de cada frase
        _lineaIndex = 0;

        //---> Abrir el panel del Cartel
        menuCartel.gameObject.SetActive(true);

        //---> Sonido al abrir el cartel
        _audioManager.PlaySFX(abrirCartel, volumenAbrirCartel);

        //---> Desactivar la tecla interactiva
        teclaInteractiva.gameObject.SetActive(false);

        //---> Desactivar la luz del objeto
        luz.gameObject.SetActive(false);

        //---> Deshabilitar el movimiento del Mago
        _mago.puedeMoverse = false;
        _mago.velocidadObjetivo = Vector3.zero;
        _mago.rb2D.velocity = Vector2.zero;

        //---> Tiempo para que se muestre primero la animacion de Abrir Cartel
        yield return new WaitForSeconds(tiempoIniciarTexto);

        //---> Inidicar que comenzo el dialogo
        _comenzoElDialogo = true;

        //---> Mostrar el parrafo del cartel dependiendo del idioma
        StartCoroutine((_objetosEntreEscenas.idioma == 0) ? MostrarLineaEspanol() : MostrarLineaIngles());
    }



    //---> Mostrar el texto con efecto de tipado
    private IEnumerator MostrarLineaEspanol()
    {
        //---> Desctivar el PanelTeclaInteractiva
        PanelTeclaInteractiva.gameObject.SetActive(false);

        //---> Borrar lo que haya en el cartel cada vez que pase a un parrafo nuevo
        textoDelCartel.text = string.Empty;

        //---> Indice para validar cual es el numero de caracter actual
        int _indiceDeCaracter = 0;

        foreach (char ch in lineasDeDialogoEspanol[_lineaIndex])
        {
            //---> Concatenar cada carater 1 a 1
            textoDelCartel.text += ch;

            //---> Reproducir el sonido cada ciertos caracteres indicados en (sonidoDeCaracteres)
            if (_indiceDeCaracter % sonidoDeCaracteres == 0)
            {
                //---> Sonido al tipear
                _audioManager.PlaySFX(escribir, volumenEscribir);
            }

            _indiceDeCaracter++;

            //---> Tiempo de de tipeo por cada caracter ignorando la escala de tiempo
            yield return new WaitForSeconds(tiempoDeTipeo);
        }

        //---> Al terminar de mostrar el texto detener el sonido de tipeo y reproducir el de (completarParrafo)
        _audioManager.StopSFX();
        _audioManager.PlaySFX(completarParrafo, volumenCompletarParrafo);

        //---> Activar el PanelTeclaInteractiva
        PanelTeclaInteractiva.gameObject.SetActive(true);
    }



    //---> Mostrar el texto con efecto de tipado
    private IEnumerator MostrarLineaIngles()
    {
        //---> Desctivar el PanelTeclaInteractiva
        PanelTeclaInteractiva.gameObject.SetActive(false);

        //---> Borrar lo que haya en el cartel cada vez que pase a un parrafo nuevo
        textoDelCartel.text = string.Empty;

        //---> Indice para validar cual es el numero de caracter actual
        int _indiceDeCaracter = 0;

        foreach (char ch in lineasDeDialogoIngles[_lineaIndex])
        {
            //---> Concatenar cada carater 1 a 1
            textoDelCartel.text += ch;

            //---> Reproducir el sonido cada ciertos caracteres indicados en (sonidoDeCaracteres)
            if (_indiceDeCaracter % sonidoDeCaracteres == 0)
            {
                //---> Sonido al tipear
                _audioManager.PlaySFX(escribir, volumenEscribir);
            }

            _indiceDeCaracter++;

            //---> Tiempo de de tipeo por cada caracter ignorando la escala de tiempo
            yield return new WaitForSeconds(tiempoDeTipeo);
        }

        //---> Al terminar de mostrar el texto detener el sonido de tipeo y reproducir el de (completarParrafo)
        _audioManager.StopSFX();
        _audioManager.PlaySFX(completarParrafo, volumenCompletarParrafo);

        //---> Activar el PanelTeclaInteractiva
        PanelTeclaInteractiva.gameObject.SetActive(true);
    }




    private IEnumerator SiguienteLinea()
    {
        //---> Se incrementa el indice en 1 para pasar a la linea siguiente en cada frase
        _lineaIndex++;

        //---> Si indica que se acabo de actualizar es menor al total de string en el array, vuelve y se llama la corrutina de tipado
        if (_lineaIndex < lineasDeDialogoEspanol.Length || _lineaIndex < lineasDeDialogoIngles.Length)
        {
            //---> Sonido al pasar de pagina
            _audioManager.PlaySFX(pasarPagina, volumenPasarPagina);

            //---> Tiempo para pasar las paginas
            yield return new WaitForSeconds(tiempoEntrePaginas);

            //---> Agregar el itulo del cartel dependiendo el idioma
            textoTitulo.text = (_objetosEntreEscenas.idioma == 0) ? tituloEspanol : tituloIngles;

            //---> Mostrar el parrafo del cartel dependiendo del idioma
            StartCoroutine((_objetosEntreEscenas.idioma == 0) ? MostrarLineaEspanol() : MostrarLineaIngles());
        }
        //---> De lo contrario, se indica que el dialogo termino
        else
        {
            StartCoroutine(CerrarCartel());
        }
    }



    //---> Corrutina para tomar vidas al interactuar con un objeto magico
    private IEnumerator VidaNueva(int vidasNuevas)
    {
        //---> Mostrar las particulas de vida
        particulasVida.Play();

        //---> Reproducir de las particulas de vida
        _audioManager.PlaySFX(particulasVidaNueva, volumenParticulasVidaNueva);

        yield return new WaitForSeconds(tiempoVidaNueva);

        //---> Asignar vidas al Mago
        _saludMago.TomarVidas(vidasNuevas);
    }



    //---> Corrutina para Cambiar el estado de la Puerta
    private IEnumerator EstadoDePuerta()
    {
        yield return new WaitForSeconds(tiempoAbrirPuerta);

        if (puerta.GetComponent<BoxCollider2D>().enabled == true)
        {
            //---> En caso de que no haya ido a la salida
            // desactivar el BoxCollider2D para que solo se cierre una vez
            puerta.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            //---> Cambiar el estado de la puerta a abierta solo la primera vez
            StartCoroutine(puerta.AbrirPuertas(true, true));
        }
    }



    private IEnumerator CerrarCartel()
    {
        //---> Para que haga un ataque al cerrar el cartel
        _animatorMago.SetTrigger("Invocar");

        yield return new WaitForSeconds(tiempoDeCartel);

        //---> Sonido al cerrar el cartel
        _audioManager.PlaySFX(cerrarCartel, volumenCerrarCartel);

        //---> Desactivar el PanelTeclaInteractiva
        PanelTeclaInteractiva.gameObject.SetActive(false);

        //---> Animacion de cerrar el cartel
        _animatorCartel.SetTrigger("CerrarCartel");

        //---> Tiempo para que se muestre primero al animacion de Cerrar el cartel
        yield return new WaitForSeconds(tiempoIniciarTexto);

        //---> Cerrar el panel del Cartel
        menuCartel.gameObject.SetActive(false);

        //---> Activar la tecla interactiva
        teclaInteractiva.gameObject.SetActive(true);

        //---> Inidicar que termino el dialogo
        _comenzoElDialogo = false;

        //---> Activar la luz del objeto
        luz.gameObject.SetActive(true);

        //---> Restaurar la animacion del libro
        animatorLibro.enabled = true;

        //---> Reproducir el sonido de los objetos
        _audioSource.Play();


        //---> Variable para elegir entre cual idioma hacer el Switch
        string asignarTextoTitulo;
        asignarTextoTitulo = (_objetosEntreEscenas.idioma == 0) ? tituloEspanol : tituloIngles;


        //---> Enviar a (HabilitarEscenario) que cartel se abrio por primera vez
        switch (asignarTextoTitulo)
        {
            case "SOBRE MI":
            case "ABOUT ME":

                contadorAutorretrado = _habilitarEscenario.carteles["Autorretrato"] += 1;

                if (contadorAutorretrado == 1)
                {
                    //---> Llamar corrutina VidaNueva
                    StartCoroutine(VidaNueva(3));

                    //---> Llamar corrutina EstadoDePuerta()
                    StartCoroutine(EstadoDePuerta());

                    // Debug.Log("Contador Autorretrado: " + _contadorAutorretrado);
                }
                break;


            case "HABILIDADES":
            case "SKILLS":

                contadorEscudoHabilidades = _habilitarEscenario.carteles["EscudoHabilidades"] += 1;

                if (contadorEscudoHabilidades == 1)
                {
                    //---> Llamar corrutina VidaNueva
                    StartCoroutine(VidaNueva(2));

                    //---> Llamar corrutina EstadoDePuerta()
                    StartCoroutine(EstadoDePuerta());

                    // Debug.Log("Escudo Habilidades: " + _contadorEscudoHabilidades);
                }
                break;


            case "EXPERIENCIA":
            case "EXPERIENCE":

                contadorLibroExperiencia = _habilitarEscenario.carteles["LibroExperiencia"] += 1;
                
                if (contadorLibroExperiencia == 1)
                {
                    //---> Llamar corrutina VidaNueva
                    StartCoroutine(VidaNueva(3));

                    // Debug.Log("Libro Experiencia: " + _contadorLibroExperiencia);
                }
                break;
        }

        //---> Tiempo para que se restaure el movimiento del Mago
        yield return new WaitForSeconds(tiempoMoverseMago);

        //---> Habilitar el movimiento del Mago
        _mago.puedeMoverse = true;
    }



    private void MagoEnrango(bool enRango)
    {
        if (enRango)
        {
            _magoEnRango = true;
            teclaInteractiva.gameObject.SetActive(true);
            luz.gameObject.SetActive(true);
            //---> Desactivar el trigger del cetro del Mago
            circleCollider2D.isTrigger = false;

            //---> Reproducir el sonido de los objetos a descubrir
            _audioSource.Play();
        }
        else
        {
            _magoEnRango = false;
            teclaInteractiva.gameObject.SetActive(false);
            luz.gameObject.SetActive(false);
            //---> Activar el trigger del cetro del Mago
            circleCollider2D.isTrigger = true;

            //---> Detener el sonido de los objetos a descubrir
            _audioSource.Stop();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            MagoEnrango(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            MagoEnrango(false);
        }
    }
}
