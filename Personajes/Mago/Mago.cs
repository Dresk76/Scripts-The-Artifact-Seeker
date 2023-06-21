using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mago : MonoBehaviour
{
    [Header("CONDICIONES")]
    [HideInInspector] public bool estaEnPlataforma = false; // No atacar en la plataforma
    [HideInInspector] public bool estaEnEscaleras = false; // No atacar en las escaleras


    [Header("IDLE")]
    private bool _estaEnIdle;


    [Header("MOVIMIENTO")]
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento = 0.05f;
    private float _movientoHorizontal = 0f; // Movimiento en el eje x del Mago
    private Vector3 _velocidad = Vector3.zero; // Iniciar la velocidad del eje en Z en cero
    private bool _mirandoALaDerecha; // Validar si el personaje esta mirando a la derecha (por defecto si esta mirando a la derecha)


    [Header("MOVIMIENTO CON JOYSTICK")]
    public Joystick joystick;
    private float _horizontalJoystick = 0f;


    [Header("DASH")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float velocidadDash = 12f;
    [SerializeField] private float tiempoDash = 0.2f;


    [Header("CONTADOR PRIMER ATAQUE PC")]
    [SerializeField] private float contadorTPrimerAtaque;
    [SerializeField] [Range(1, 6)] private int tiempoHacerPrimerAtaque = 2;
    [SerializeField] private TMP_Text textoContadorPrimerAtaque;
    [SerializeField] private GameObject textoContadorPrimerAtaqueObjeto;
    [SerializeField] private Image circuloContadorFillPrimerAtaque;
    [SerializeField] private RectTransform particulasContadorPrimerAtaque;
    [SerializeField] private GameObject transparenciaPrimerAtaque;
    private bool _puedeHacerPrimerAtaquePC = true; // Para que solo realice el primer ataque si puede atacar en PC


    [Header("CONTADOR SEGUNDO ATAQUE PC")]
    [SerializeField] private float contadorTSegundoAtaque;
    [SerializeField] [Range(1, 6)] private int tiempoHacerSegundoAtaque = 3;
    [SerializeField] private TMP_Text textoContadorSegundoAtaque;
    [SerializeField] private GameObject textoContadorSegundoAtaqueObjeto;
    [SerializeField] private Image circuloContadorFillSegundoAtaque;
    [SerializeField] private RectTransform particulasContadorSegundoAtaque;
    [SerializeField] private GameObject transparenciaSegundoAtaque;
    private bool _puedeHacerSegundoAtaquePC = true; // Para que solo realice el primer ataque si puede atacar en PC


    [Header("CONTADOR DASH PC")]
    [SerializeField] private float contadorTDash;
    [SerializeField] [Range(1, 6)] private int tiempoHacerDash = 6;
    [SerializeField] private TMP_Text textoContadorDash;
    [SerializeField] private GameObject textoContadorDashObjeto;
    [SerializeField] private Image circuloContadorFillDash;
    [SerializeField] private RectTransform particulasContadorDash;
    [SerializeField] private GameObject transparenciaDash;
    private bool _puedeHacerDashPC = true;


    // [Header("CONTADOR PRIMER ATAQUE MOVIL")]
    // [SerializeField] private float contadorTPrimerAtaqueMovil;
    // [SerializeField] [Range(1, 6)] private int tiempoHacerPrimerAtaqueMovil = 2;
    // [SerializeField] private TMP_Text textoContadorPrimerAtaqueMovil;
    // [SerializeField] private GameObject textoContadorPrimerAtaqueObjetoMovil;
    // [SerializeField] private Image circuloContadorFillPrimerAtaqueMovil;
    // [SerializeField] private RectTransform particulasContadorPrimerAtaqueMovil;
    // [SerializeField] private GameObject transparenciaPrimerAtaqueMovil;


    // [Header("CONTADOR SEGUNDO ATAQUE MOVIL")]
    // [SerializeField] private float contadorTSegundoAtaqueMovil;
    // [SerializeField] [Range(1, 6)] private int tiempoHacerSegundoAtaqueMovil = 3;
    // [SerializeField] private TMP_Text textoContadorSegundoAtaqueMovil;
    // [SerializeField] private GameObject textoContadorSegundoAtaqueObjetoMovil;
    // [SerializeField] private Image circuloContadorFillSegundoAtaqueMovil;
    // [SerializeField] private RectTransform particulasContadorSegundoAtaqueMovil;
    // [SerializeField] private GameObject transparenciaSegundoAtaqueMovil;


    // [Header("CONTADOR DASH MOVIL")]
    // [SerializeField] private float contadorTDashMovil;
    // [SerializeField] [Range(1, 6)] private int tiempoHacerDashMovil = 6;
    // [SerializeField] private TMP_Text textoContadorDashMovil;
    // [SerializeField] private GameObject textoContadorDashObjetoMovil;
    // [SerializeField] private Image circuloContadorFillDashMovil;
    // [SerializeField] private RectTransform particulasContadorDashMovil;
    // [SerializeField] private GameObject transparenciaDashMovil;


    [Header("DETECTAR SUELO")]
    [SerializeField] private Transform puntoDeSuelo; // Para saber desde donde se va a hacer el chequeo del suelo
    [SerializeField] private Vector2 dimensionesCaja; // X = 0.515 Y = 0.025 Crear una caja para saber si esta en el suelo
    [SerializeField] private LayerMask groundLayer; // Permite desde Unity seleccionar una Layer de la lista de Layers
    [SerializeField] private bool dibujarCaja; // Para dibujar la caja por pantalla


    [Header("SALTO")]
    public float fuerzaDeSalto = 5.5f; // Fuerza a aplicar cuando el usuario pulse el boton de saltar
    [HideInInspector] public bool saltar = false; // Para indicar que el usuario ha solicitado un salto


    [Header("SALTO REGULABLE")]
    [Range(0, 1f)] [SerializeField] private float multiplicadorCancelarSalto = 0.7f;
    [SerializeField] private float multiplicadorGravedad = 1f;
    private bool _botonSaltoArriba = true;


    [Header("ATACAR")]
    private bool _estaAtacando; // Indicar si el personaje esta atacando, para no volver a atacar mientras esta atacando


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;


    [Header("REFERENCIA MENU GAME OVER")]
    private MenuGameOver _menuGameOver;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIA PARTICULAS DE MAGO")]
    private ParticulasPolvoMago _particulasPolvoMago;


    [Header("REFERENCIAS DEL MAGO OSCURO")]
    private Transform _posicionMagoOscuro;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip primerAtaque;
    [SerializeField] private AudioClip segundoAtaque;
    [SerializeField] private AudioClip iniciarSaltoEnIdle;
    [SerializeField] private AudioClip iniciarSaltoCorriendo;
    [SerializeField] private AudioClip pasoDerecho;
    [SerializeField] private AudioClip pasoIzquierdo;
    [SerializeField] private AudioClip invocar;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenDash = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenPrimerAtaque = 0.17f;
    [Range(0, 1f)][SerializeField] private float volumenSegundoAtaque = 0.17f;
    [Range(0, 1f)][SerializeField] private float volumenIniciarSaltoEnIdle = 0.7f;
    [Range(0, 1f)][SerializeField] private float volumenIniciarSaltoCorriendo = 0.35f;
    [Range(0, 1f)][SerializeField] private float volumenPasoDerecho = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenPasoIzquierdo = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenInvocar = 0.16f;


    [Header("VARIABLES COMPARTIDAS")]
    // La gravedad en el rigibody debe ser de 1.25
    private float _gravedadInicial; // Guardar la gravedad inicial del Mago


    [Header("REFERENCIAS Y VARIABLES PUBLICAS")]
    //--> MOVIMIENTO
    [Range(100f, 250f)] public float velocidadDeMovimiento = 150f; // Velocidad a la que se mueve el jugador
    [HideInInspector] public Vector3 velocidadObjetivo; // Guardar en X el _movientoHorizontal y en Y el Rigibody2D
    [HideInInspector] public Vector2 input; // Valores ingresados por el usuario en x o en y
    [HideInInspector] public bool puedeMoverse = true; // Indicar si el Mago se puede mover

    //--> MOVIMIENTO CON JOYSTICK
    [HideInInspector] public Vector2 move; // Valores ingresados por el usuario en x o en y

    //--> DASH
    // Condicion para hacer el Dash
    [HideInInspector] public bool puedeHacerDash = true;
    // [HideInInspector] public bool puedeHacerDashMovil = true;

    //--> ATAQUE
    // [HideInInspector] public bool puedeHacerPrimerAtaqueMovil = true; // Para que solo realice el primer ataque si puede atacar en Movil
    // [HideInInspector] public bool puedeHacerSegundoAtaqueMovil = true; // Para que solo realice el primer ataque si puede atacar en Movil
    [HideInInspector] public bool noAtacar = false; // Para que no ataque en rangos que no desee que lo haga

    //--> SALTO
    [HideInInspector] public bool puedeSaltar = true; // Para indicar que el usuario ha solicitado un salto

    //--> VALIDAR SUELO
    [HideInInspector] public bool enSuelo; // Indicar si el personaje esta en el suelo

    //---> REFERENCIAS
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    //---> PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
    // [HideInInspector] public ControllerType controllerType;






    private void Awake()
    {
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
        _menuGameOver = GameObject.Find("Menus").GetComponent<MenuGameOver>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _particulasPolvoMago = GameObject.Find("ParticulasPolvoMago").GetComponent<ParticulasPolvoMago>();
        _posicionMagoOscuro = GameObject.FindGameObjectWithTag("MagoOscuro").GetComponent<Transform>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }



    private void Start()
    {
        //---> Validar si el mago esta mirando a la izquierda o a la derecha y dependiendo de esto modificar la variable _mirandoALaDerecha
        if (transform.localScale.x < 0f)
        {
            _mirandoALaDerecha = false;
        } else if (transform.localScale.x > 0f)
        {
            _mirandoALaDerecha = true;
        }


        //---> Iniciarlizar a la escala de gravedad del _rigibody
        _gravedadInicial = rb2D.gravityScale;


        //---> Iniciar el valor del contador y del fillAmount del Primer Ataque
        contadorTPrimerAtaque = tiempoHacerPrimerAtaque;
        circuloContadorFillPrimerAtaque.fillAmount = 1f;


        //---> Iniciar el valor del contador y del fillAmount del Segundo Ataque
        contadorTSegundoAtaque = tiempoHacerSegundoAtaque;
        circuloContadorFillSegundoAtaque.fillAmount = 1f;


        //---> Iniciar el valor del contador y del fillAmount del Dash
        contadorTDash = tiempoHacerDash;
        circuloContadorFillDash.fillAmount = 1f;
    }



    // Es donde mejor se obtienen los datos de los input, que botones presiona el usuario y sus valores
    void Update()
    {
        //---> Si esta activo el menu de Pausa no puede realizar nada en el update
        if(_controlDelTiempo.getEstaPausado() || _menuGameOver.getEstaEnGameOver() || _objetosEntreEscenas.estaEnFinPortafolio)
            return;

        //---> Tomar valores del teclado con el movimiento del personaje en ambos ejes
        // GetAxisRaw para tomar valores desde 1 a -1 sin suavizado pero sin retardo
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //---> Guardar el movimiento en x del usuario por la velocidad a la que se mueve el Mago
        _movientoHorizontal =  input.x * velocidadDeMovimiento;

        // Debug.Log("Input.x: " + input.x);


        //---> Tomar valores del joystick con el movimiento del personaje en ambos ejes
        // GetAxisRaw para tomar valores desde 1 a -1 sin suavizado pero sin retardo
        move.x = joystick.Horizontal;

        //---> Guardar el movimiento Horizontal del Joystick del usuario por la velocidad a la que se mueve el Mago
        _horizontalJoystick = move.x * velocidadDeMovimiento;
        
        // Debug.Log("Move.x: " + move.x);


        //---> PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
        // if (Input.GetKeyDown(KeyCode.W) && puedeMoverse && enSuelo && puedeHacerDash && _puedeHacerDashPC && controllerType == ControllerType.PC)
        //---> Hacer un Dash
        if (Input.GetKeyDown(KeyCode.W) && puedeMoverse && enSuelo && !estaEnPlataforma && puedeHacerDash && _puedeHacerDashPC )
        {
            //---> LLamar corrutina para hacer un Dash
            StartCoroutine(Dash());
        }


        //---> Contador para hacer Dash
        ContadorDashPc();

        //---> Contador para habilitar el Primer Ataque
        ContadorPrimerAtaquePc();

        //---> Contador para habilitar el Segundo Ataque
        ContadorSegundoAtaquePc();



        //---> Contador para hacer Dash Movil
        // ContadorDashMovil();

        //---> Contador para habilitar el Primer Ataque Movil
        // ContadorPrimerAtaqueMovil();

        //---> Contador para habilitar el Segundo Ataque Movil
        // ContadorSegundoAtaqueMovil();


        //---> Validar si el personaje esta en el suelo para poder saltar pintando una caja
        enSuelo = Physics2D.OverlapBox(puntoDeSuelo.position, dimensionesCaja, 0f, groundLayer);


        //---> PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
        // if (Input.GetButton("Jump") && controllerType == ControllerType.PC)
        //---> Al mantener el boton presionado, solicita un salto
        if (Input.GetButton("Jump"))
        {
            saltar = true;
        }

        //---> PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
        // if (Input.GetButtonUp("Jump") && controllerType == ControllerType.PC)
        // ---> Al soltar el boton de salto llamar la funcion de bajar del salto
        if (Input.GetButtonUp("Jump"))
        {
            BotonSaltoArriba();
        }


        //---> Ataques
        //---> PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
        // if (puedeMoverse && enSuelo && !noAtacar && !estaEnPlataforma && !estaEnEscaleras && controllerType == ControllerType.PC)
        /*
            puedeHacerPrimerAtaque: Para haga el PrimerAtaque si ya lo esta realizando
            puedeMoverse: Para que no ataque si los carteles estan abiertos 
            enSuelo: Para que ataque si esta en el suelo y no haga un especie de flappy bird LoL
        */
        if (puedeMoverse && enSuelo && !noAtacar && !estaEnPlataforma && !estaEnEscaleras)
        {
            //---> Primer ataque
            if (Input.GetButtonDown("Fire1") && _puedeHacerPrimerAtaquePC)
            {
                //---> LLamar corrutina para hacer el Primer Ataque
                StartCoroutine(PrimerAtaque());
            }


            //---> Segundo ataque
            // puedeHacerSegundoAtaque: Para haga el SegundoAtaque si ya lo esta realizando
            if (Input.GetButtonDown("Fire2") && _puedeHacerSegundoAtaquePC)
            {
                //---> LLamar corrutina para hacer el Primer Ataque
                StartCoroutine(SegundoAtaque());
            }
        }
    }



    // FixedUpdate es donde se debe mover cualquier objeto fisico en Unity (Rigidbody2D)
    void FixedUpdate()
    {
        /*
            !_estaAtacando: Para que no se mueva o salte atacando
            puedeMoverse: Para que no se mueva o salte si los carteles estan abiertos 
            enSuelo: Para que se mueva o salte si esta en el suelo
        */
        if (!_estaAtacando && puedeMoverse)
        {
            //---> Mover el Personaje
            if (enSuelo)
            {
                //---> PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
                // if (controllerType == ControllerType.PC)
                // {
                //     //---> Mover el personaje a la misma velocidad en cualquier Computador
                //     Mover(_movientoHorizontal * Time.fixedDeltaTime);
                // }
                // else
                // {
                //     //---> Mover el personaje con joystick
                //     MoverConJoystick(_horizontalJoystick * Time.fixedDeltaTime);
                // }


                //---> MOVER CON PC A LA MISMA VELOCIDAD EN CUALQUIER PC
                Mover(_movientoHorizontal * Time.fixedDeltaTime);

                //---> MOVER EL MAGO CON JOYSTICK
                // MoverConJoystick(_horizontalJoystick * Time.fixedDeltaTime);
            }


            //---> Hacer Saltar Personaje
            if (saltar && _botonSaltoArriba && enSuelo && puedeSaltar)
            {
                Saltar();

                if (_estaEnIdle)
                {
                    //---> Sonido del Mago al iniciar el salto estando en Idle
                    _audioManager.PlayMago(iniciarSaltoEnIdle, volumenIniciarSaltoEnIdle);
                }
                else
                {
                    //---> Sonido del Mago al iniciar el salto corriendo
                    _audioManager.PlayMago(iniciarSaltoCorriendo, volumenIniciarSaltoCorriendo);
                }
            }

            //---> Para que el personaje pueda caer mas rapido
            if (rb2D.velocity.y < 0 && !enSuelo)
            {
                rb2D.gravityScale = _gravedadInicial * multiplicadorGravedad;
            }
            else
            {
                rb2D.gravityScale = _gravedadInicial;
            }

            // Para que no siempre mande la señal de saltar
            saltar = false;
        }
    }



    //---> Mover el Personaje con Joystick
    private void MoverConJoystick(float mover)
    {
        //---> Mover el personaje solo en el eje X y no alterarse al caer o saltar
        velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);

        //---> Para que haya un suavizado a la hora de acelerar o frenar
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref _velocidad, suavizadoDeMovimiento);
    
        //---> Voltear personaje hacia donde le indique el usuario que camine
        if (mover < 0f && _mirandoALaDerecha)
        {
            Flip();
        } else if (mover > 0f && !_mirandoALaDerecha)
        {
            Flip();
        }
    }



    //---> Mover el Personaje
    private void Mover(float mover)
    {
        //---> Mover el personaje solo en el eje X y no alterarse al caer o saltar
        velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);

        //---> Para que haya un suavizado a la hora de acelerar o frenar
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref _velocidad, suavizadoDeMovimiento);
    
        //---> Voltear personaje hacia donde le indique el usuario que camine
        if (mover < 0f && _mirandoALaDerecha)
        {
            Flip();
        } else if (mover > 0f && !_mirandoALaDerecha)
        {
            Flip();
        }
    }



    //---> Girar el personaje
    private void Flip()
    {
        _mirandoALaDerecha = !_mirandoALaDerecha;
        // Girar al Mago por su escala
        Vector3 escala = transform.localScale;
        escala.x *= -1f;
        transform.localScale = escala;
    }



    //---> Sonido de los pasos del pie derecho del Mago al caminar
    public void SonidoPasoDerecho()
    {
        _audioManager.PlayMago(pasoDerecho, volumenPasoDerecho);
    }



    //---> Sonido de los pasos del pie izquierdo del Mago al caminar
    public void SonidoPasoIzquierdo()
    {
        _audioManager.PlayMago(pasoIzquierdo, volumenPasoIzquierdo);
    }



    //---> Sonido al realizar una invocacion
    public void SonidoInvocar()
    {
        _audioManager.PlayMago(invocar, volumenInvocar);
    }



    //---> Hacer Saltar Personaje
    private void Saltar()
    {
        //---> Saltar hacia arriba
        rb2D.AddForce(Vector2.up * fuerzaDeSalto, ForceMode2D.Impulse);

        //---> Activar particulas de salto
        _particulasPolvoMago.ParticulaSalto();
    
        enSuelo = false; // Ya no esta en el suelo
        saltar = false; // Para que ya no mande la señal de saltar
        _botonSaltoArriba = false; // Indicar que NO ha soltado el boton de saltar
    }



    //---> Hacer bajar al personaje de un salto
    public void BotonSaltoArriba()
    {
        if (rb2D.velocity.y > 0)
        {
            rb2D.AddForce(Vector2.down * rb2D.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }

        _botonSaltoArriba = true; // Indicar que SI ha soltado el boton de saltar
        saltar = false; // Para que ya no mande la señal de saltar
    }



    //---> Hacer un Dash
    public IEnumerator Dash()
    {
        //---> Sonido del Mago al realizar un Dash
        _audioManager.PlayMago(dash, volumenDash);

        puedeMoverse = false;
        _puedeHacerDashPC = false;
        // puedeHacerDashMovil = false;
        //---> Para que no lo afecte la gravedad y se desplace recto
        rb2D.gravityScale = 0;
        rb2D.velocity = new Vector2(velocidadDash * transform.localScale.x, 0);
        animator.SetTrigger("Dash");
        trailRenderer.emitting = true;

        yield return new WaitForSecondsRealtime(tiempoDash);

        puedeMoverse = true;
        //---> Asignar su gravedad por defecto
        rb2D.gravityScale = _gravedadInicial;
        trailRenderer.emitting = false;
    }




    //---> Contador para hacer Dash PC
    private void ContadorDashPc()
    {
        //---> Modificar el texto del contador Dash
        textoContadorDash.text = contadorTDash.ToString("f0");

        if (!_puedeHacerDashPC)
        {
            //---> Activar el texto del contador
            textoContadorDashObjeto.gameObject.SetActive(true);

            //---> Activar la trasnparencia
            transparenciaDash.gameObject.SetActive(true);

            //---> Modificar y el fillAmount del contador con -1f para que haga el efecto hasta que llegue a 1
            circuloContadorFillDash.fillAmount -= 1f / (tiempoHacerDash - 1f) * Time.deltaTime;

            //--> Rotar las particulas a medida que avanza el fillAmount
            // Tiempo de la rotacion
            float rotacion = 360 * -circuloContadorFillDash.fillAmount;
            // Rotar las particulas
            particulasContadorDash.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotacion));

            //---> Comenzar a contar en reversa
            contadorTDash -= Time.deltaTime;

            //---> Liberar el Dash cuando el contador llegue a 0
            if (contadorTDash <= 1)
            {
                //---> Desactivar el texto del contador
                textoContadorDashObjeto.gameObject.SetActive(false);

                //---> Desactivar la trasnparencia
                transparenciaDash.gameObject.SetActive(false);

                contadorTDash = tiempoHacerDash;
                circuloContadorFillDash.fillAmount = 1f;
                _puedeHacerDashPC = true;
            }
        }
    }



    //---> Contador para hacer Dash
    // private void ContadorDashMovil()
    // {
    //     //---> Modificar el texto del contador Dash
    //     textoContadorDashMovil.text = contadorTDashMovil.ToString("f0");

    //     if (!puedeHacerDashMovil)
    //     {
    //         //---> Activar el texto del contador
    //         textoContadorDashObjetoMovil.gameObject.SetActive(true);

    //         //---> Activar la trasnparencia
    //         transparenciaDashMovil.gameObject.SetActive(true);

    //         //---> Modificar y el fillAmount del contador con -1f para que haga el efecto hasta que llegue a 1
    //         circuloContadorFillDashMovil.fillAmount -= 1f / (tiempoHacerDashMovil - 1f) * Time.deltaTime;

    //         //--> Rotar las particulas a medida que avanza el fillAmount
    //         // Tiempo de la rotacion
    //         float rotacion = 360 * -circuloContadorFillDashMovil.fillAmount;
    //         // Rotar las particulas
    //         particulasContadorDashMovil.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotacion));

    //         //---> Comenzar a contar en reversa
    //         contadorTDashMovil -= Time.deltaTime;

    //         //---> Liberar el Dash cuando el contador llegue a 0
    //         if (contadorTDashMovil <= 1)
    //         {
    //             //---> Desactivar el texto del contador
    //             textoContadorDashObjetoMovil.gameObject.SetActive(false);

    //             //---> Desactivar la trasnparencia
    //             transparenciaDashMovil.gameObject.SetActive(false);

    //             contadorTDashMovil = tiempoHacerDashMovil;
    //             circuloContadorFillDashMovil.fillAmount = 1f;
    //             puedeHacerDashMovil = true;
    //         }
    //     }
    // }



    //---> Corrutina para generar el Primer Ataque
    public IEnumerator PrimerAtaque()
    {
        //---> Sonido del Mago al realizar el primer ataque
        _audioManager.PlayMago(primerAtaque, volumenPrimerAtaque);

        _puedeHacerPrimerAtaquePC = false;
        // puedeHacerPrimerAtaqueMovil = false;
        //---> Se detiene al personaje con su movimiento en cero y activar el Idle
        velocidadObjetivo = Vector3.zero;
        //---> Se le quita la velocidad al personaje para que no ataque moviendose
        rb2D.velocity = Vector2.zero;
        animator.SetTrigger("PrimerAtaque");

        yield return new WaitForSecondsRealtime(tiempoHacerPrimerAtaque);
    }



    //---> Hacer el primer ataque PC
    private void ContadorPrimerAtaquePc()
    {
        //---> Modificar el texto del contador Primer Ataque
        textoContadorPrimerAtaque.text = contadorTPrimerAtaque.ToString("f0");

        if (!_puedeHacerPrimerAtaquePC)
        {
            //---> Activar el texto del contador
            textoContadorPrimerAtaqueObjeto.gameObject.SetActive(true);

            //---> Activar la trasnparencia
            transparenciaPrimerAtaque.gameObject.SetActive(true);

            //---> Modificar y el fillAmount del contador con -1f para que haga el efecto hasta que llegue a 1
            circuloContadorFillPrimerAtaque.fillAmount -= 1f / (tiempoHacerPrimerAtaque - 1f) * Time.deltaTime;

            //--> Rotar las particulas a medida que avanza el fillAmount
            // Tiempo de la rotacion
            float rotacion = 360 * -circuloContadorFillPrimerAtaque.fillAmount;
            // Rotar las particulas
            particulasContadorPrimerAtaque.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotacion));

            //---> Comenzar a contar en reversa
            contadorTPrimerAtaque -= Time.deltaTime;

            //---> Liberar el Dash cuando el contador llegue a 0
            if (contadorTPrimerAtaque <= 1)
            {
                //---> Desactivar el texto del contador
                textoContadorPrimerAtaqueObjeto.gameObject.SetActive(false);

                //---> Desactivar la trasnparencia
                transparenciaPrimerAtaque.gameObject.SetActive(false);

                contadorTPrimerAtaque = tiempoHacerPrimerAtaque;
                circuloContadorFillPrimerAtaque.fillAmount = 1f;
                _puedeHacerPrimerAtaquePC = true;
            }
        }
    }



    //---> Hacer el primer ataque Movil
    // private void ContadorPrimerAtaqueMovil()
    // {
    //     //---> Modificar el texto del contador Primer Ataque
    //     textoContadorPrimerAtaque.text = contadorTPrimerAtaque.ToString("f0");

    //     if (!puedeHacerPrimerAtaqueMovil)
    //     {
    //         //---> Activar el texto del contador
    //         textoContadorPrimerAtaqueObjetoMovil.gameObject.SetActive(true);

    //         //---> Activar la trasnparencia
    //         transparenciaPrimerAtaqueMovil.gameObject.SetActive(true);

    //         //---> Modificar y el fillAmount del contador con -1f para que haga el efecto hasta que llegue a 1
    //         circuloContadorFillPrimerAtaqueMovil.fillAmount -= 1f / (tiempoHacerPrimerAtaqueMovil - 1f) * Time.deltaTime;

    //         //--> Rotar las particulas a medida que avanza el fillAmount
    //         // Tiempo de la rotacion
    //         float rotacion = 360 * -circuloContadorFillPrimerAtaqueMovil.fillAmount;
    //         // Rotar las particulas
    //         particulasContadorPrimerAtaqueMovil.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotacion));

    //         //---> Comenzar a contar en reversa
    //         contadorTPrimerAtaqueMovil -= Time.deltaTime;

    //         //---> Liberar el Dash cuando el contador llegue a 0
    //         if (contadorTPrimerAtaqueMovil <= 1)
    //         {
    //             //---> Desactivar el texto del contador
    //             textoContadorPrimerAtaqueObjetoMovil.gameObject.SetActive(false);

    //             //---> Desactivar la trasnparencia
    //             transparenciaPrimerAtaqueMovil.gameObject.SetActive(false);

    //             contadorTPrimerAtaqueMovil = tiempoHacerPrimerAtaque;
    //             circuloContadorFillPrimerAtaqueMovil.fillAmount = 1f;
    //             puedeHacerPrimerAtaqueMovil = true;
    //         }
    //     }
    // }



    //---> Corrutina para generar el Segundo Ataque
    public IEnumerator SegundoAtaque()
    {
        //---> Sonido del Mago al realizar el segundo ataque
        _audioManager.PlayMago(segundoAtaque, volumenSegundoAtaque);

        _puedeHacerSegundoAtaquePC = false;
        // puedeHacerSegundoAtaqueMovil = false;
        //---> Se detiene al personaje con su movimiento en cero y activar el Idle
        velocidadObjetivo = Vector3.zero;
        //---> Se le quita la velocidad al personaje para que no ataque moviendose
        rb2D.velocity = Vector2.zero;
        animator.SetTrigger("SegundoAtaque");

        yield return new WaitForSecondsRealtime(tiempoHacerSegundoAtaque);
    }



    //---> Hacer el segundo ataque PC
    private void ContadorSegundoAtaquePc()
    {
        //---> Modificar el texto del contador Segundo Ataque
        textoContadorSegundoAtaque.text = contadorTSegundoAtaque.ToString("f0");

        if (!_puedeHacerSegundoAtaquePC)
        {
            //---> Activar el texto del contador
            textoContadorSegundoAtaqueObjeto.gameObject.SetActive(true);

            //---> Activar la trasnparencia
            transparenciaSegundoAtaque.gameObject.SetActive(true);

            //---> Modificar y el fillAmount del contador con -1f para que haga el efecto hasta que llegue a 1
            circuloContadorFillSegundoAtaque.fillAmount -= 1f / (tiempoHacerSegundoAtaque - 1f) * Time.deltaTime;

            //--> Rotar las particulas a medida que avanza el fillAmount
            // Tiempo de la rotacion
            float rotacion = 360 * -circuloContadorFillSegundoAtaque.fillAmount;
            // Rotar las particulas
            particulasContadorSegundoAtaque.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotacion));

            //---> Comenzar a contar en reversa
            contadorTSegundoAtaque -= Time.deltaTime;

            //---> Liberar el Dash cuando el contador llegue a 0
            if (contadorTSegundoAtaque <= 1)
            {
                //---> Desactivar el texto del contador
                textoContadorSegundoAtaqueObjeto.gameObject.SetActive(false);

                //---> Desactivar la trasnparencia
                transparenciaSegundoAtaque.gameObject.SetActive(false);

                contadorTSegundoAtaque = tiempoHacerSegundoAtaque;
                circuloContadorFillSegundoAtaque.fillAmount = 1f;
                _puedeHacerSegundoAtaquePC = true;
            }
        }
    }



    //---> Hacer el segundo ataque Movil
    // private void ContadorSegundoAtaqueMovil()
    // {
    //     //---> Modificar el texto del contador Segundo Ataque
    //     textoContadorSegundoAtaqueMovil.text = contadorTSegundoAtaqueMovil.ToString("f0");

    //     if (!puedeHacerSegundoAtaqueMovil)
    //     {
    //         //---> Activar el texto del contador
    //         textoContadorSegundoAtaqueObjetoMovil.gameObject.SetActive(true);

    //         //---> Activar la trasnparencia
    //         transparenciaSegundoAtaqueMovil.gameObject.SetActive(true);

    //         //---> Modificar y el fillAmount del contador con -1f para que haga el efecto hasta que llegue a 1
    //         circuloContadorFillSegundoAtaqueMovil.fillAmount -= 1f / (tiempoHacerSegundoAtaqueMovil - 1f) * Time.deltaTime;

    //         //--> Rotar las particulas a medida que avanza el fillAmount
    //         // Tiempo de la rotacion
    //         float rotacion = 360 * -circuloContadorFillSegundoAtaqueMovil.fillAmount;
    //         // Rotar las particulas
    //         particulasContadorSegundoAtaqueMovil.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotacion));

    //         //---> Comenzar a contar en reversa
    //         contadorTSegundoAtaqueMovil -= Time.deltaTime;

    //         //---> Liberar el Dash cuando el contador llegue a 0
    //         if (contadorTSegundoAtaqueMovil <= 1)
    //         {
    //             //---> Desactivar el texto del contador
    //             textoContadorSegundoAtaqueObjetoMovil.gameObject.SetActive(false);

    //             //---> Desactivar la trasnparencia
    //             transparenciaSegundoAtaqueMovil.gameObject.SetActive(false);

    //             contadorTSegundoAtaqueMovil = tiempoHacerSegundoAtaqueMovil;
    //             circuloContadorFillSegundoAtaqueMovil.fillAmount = 1f;
    //             puedeHacerSegundoAtaqueMovil = true;
    //         }
    //     }
    // }



    //---> Girar al Mago hacia donde este viendo el Mago oscuro
    public void MirarMagoOscuro()
    {
        if ((_posicionMagoOscuro.position.x > transform.position.x && !_mirandoALaDerecha) || (_posicionMagoOscuro.position.x < transform.position.x && _mirandoALaDerecha))
        {
            //---> Llamar ael metodo Flip()
            Flip();
        }
    }



    //---> Dibujar la caja que valida si esta en el suelo
    private void OnDrawGizmos()
    {
        if (dibujarCaja) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(puntoDeSuelo.position, dimensionesCaja);
        }
    }



    // LateUpdate es donde conviene hacer cualquier tipo de codigo que este relacionado con el animator
    void LateUpdate ()
    {
        //---> Activar el Idle
        // Idle sera true siempre y cuando el usuario no esta moviendo el personaje por la pantalla (si el _movimiento es cero)
        animator.SetBool("Idle", velocidadObjetivo == Vector3.zero); 

        //---> Inidcar cuando esta en el suelo
        animator.SetBool("EstaEnSuelo", enSuelo);

        //---> Pasar los valores del rb2D.velocity.y a la VelocidadVertical
        animator.SetFloat("VelocidadVertical", rb2D.velocity.y);

        //---> Saber si el personaje esta en estado de ataque
        // Da la informacion del estado actual que esta en el Animator y se le pregunta si su tag es Ataque
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("PrimerAtaque") || animator.GetCurrentAnimatorStateInfo(0).IsTag("SegundoAtaque"))
        {
            _estaAtacando = true;
        }
        else
        {
            _estaAtacando = false;
        }

        //---> Saber si el personaje esta en estado Idle
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            _estaEnIdle = true;
        }
        else
        {
            _estaEnIdle = false;
        }
    }
}
