using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoOscuro : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _mirandoALaDerecha = true;


    [Header("MOVIMIENTO")]
    [SerializeField] private float velocidadDeMovimiento = 3f;
    [SerializeField] private float distanciaEntreMagos; // Para mostrar la distancia entre los magos


    [Header("DASH")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float velocidadDash = 7f;
    [SerializeField] private float tiempoDash = 0.2f;
    // La gravedad en el rigibody debe ser de 1.25
    private float _gravedadInicial; // Guardar la gravedad inicial del Mago


    [Header("DETECTAR SUELO")]
    [SerializeField] private Transform puntoDeSuelo;
    [SerializeField] private Vector2 dimensionesCaja; // X = 0.6 Y = 0.025
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool dibujarCaja;


    [Header("SALTO ADELANTE")]
    [SerializeField] private float fuerzaDeSaltoAdelante = 6f;
    [SerializeField] private float fuerzaDeAvance = 1f;


    [Header("SALTO ATRAS")]
    [SerializeField] private float fuerzaDeSaltoAtras = 5f;
    [SerializeField] private float fuerzaDeRetroceso = 2f;


    [Header("COMBATE MAGO OSCURO")]
    [SerializeField] private Transform hitBox;
    [SerializeField] private float radioAtaque = 0.8f;
    [SerializeField] private int dañoAtaque = 1;
    [SerializeField] private bool dibujarCirculo; // Para dibujar el circulo de ataque por pantalla


    [Header("MATERIAL")]
    [SerializeField] private PhysicsMaterial2D materialResbaladizo;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip primerAtaque;
    [SerializeField] private AudioClip segundoAtaque;
    [SerializeField] private AudioClip saltoAdelante;
    // [SerializeField] private AudioClip saltoAtras;
    [SerializeField] private AudioClip pasoDerecho;
    [SerializeField] private AudioClip pasoIzquierdo;
    [SerializeField] private AudioClip gruñidoMagoOscuro;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenDash = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenPrimerAtaque = 0.2f;
    [Range(0, 1f)][SerializeField] private float volumenSegundoAtaque = 0.14f;
    [Range(0, 1f)][SerializeField] private float volumenSaltoAdelante = 0.17f;
    // [Range(0, 1f)][SerializeField] private float volumenSaltoAtras = 0.3f;
    [Range(0, 1f)][SerializeField] private float volumenPasoDerecho = 0.2f;
    [Range(0, 1f)][SerializeField] private float volumenPasoIzquierdo = 0.2f;
    [Range(0, 1f)][SerializeField] private float volumenGruñidoMagoOscuro = 0.5f;


    [Header("REBOTE POR GOLPE")]
    [SerializeField] private Vector2 velocidadRebote; // X = 1.5 Y = 4
    [SerializeField] private float fuerzaRebote = 1.5f;


    [Header("REFERENCIAS PARTICULAS DE MAGO")]
    private ParticulasPolvoMagoOscuro _particulasPolvoMagoOscuro;


    [Header("REFERENCIAS DEL MAGO")]
    private Transform _posicionMago;
    private SaludMago _saludMago;


    [Header("REFERENCIAS")]
    private CapsuleCollider2D _capsuleColliderMagoOscuro;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;


    [Header("REFERENCIAS Y VARIABLES PUBLICAS")]
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public bool puedeMoverse = true;
    [HideInInspector] public bool enSuelo;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _particulasPolvoMagoOscuro = GameObject.Find("ParticulasPolvoMagoOscuro").GetComponent<ParticulasPolvoMagoOscuro>();
        _posicionMago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Transform>();
        _saludMago = GameObject.FindGameObjectWithTag("Mago").GetComponent<SaludMago>();
        rb2D = GetComponent<Rigidbody2D>();
        _capsuleColliderMagoOscuro = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void Start()
    {
        //---> Iniciarlizar a la escala de gravedad del _rigibody
        _gravedadInicial = rb2D.gravityScale;
    }



    private void Update()
    {
        //---> Validar si el Mago Oscuro esta en el suelo
        enSuelo = Physics2D.OverlapBox(puntoDeSuelo.position, dimensionesCaja, 0f, groundLayer);

        //---> Validar la distancia que hay entre los magos y pasarla al animator
        distanciaEntreMagos = Vector2.Distance(transform.position, _posicionMago.position);
        _animator.SetFloat("DistanciaMagos", distanciaEntreMagos);
    }



    //---> Mover el Mago Oscuro
    public void Mover()
    {
        if (puedeMoverse)
        {
            //---> Para que sea positivo si esta mirando a la derecha y negativo a la izquierda
            rb2D.velocity = new Vector2(velocidadDeMovimiento, rb2D.velocity.y) * transform.right;
        }
    }



    //---> Girar al Mago oscuro hacia donde este viendo el Mago
    public void MirarMago(bool puedeReproducir)
    {
        if ((_posicionMago.position.x > transform.position.x && !_mirandoALaDerecha) || (_posicionMago.position.x < transform.position.x && _mirandoALaDerecha))
        {
            _mirandoALaDerecha = !_mirandoALaDerecha;

            //---> Girar al Mago Oscuro por su rotacion
            transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);

            if (puedeReproducir)
            {
                //---> Sonido de un paso del Mago Oscuro al girar
                _audioManager.PlayMagoOscuro(pasoIzquierdo, volumenPasoIzquierdo);
            }
        }
    }



    //---> Hacer un Dash
    public IEnumerator Dash()
    {
        puedeMoverse = false;

        //---> Para que no lo afecte la gravedad y se desplace recto
        rb2D.gravityScale = 0;
        rb2D.velocity = new Vector2(velocidadDash * transform.localScale.x, 0) * transform.right;
        _animator.SetTrigger("Dash");
        trailRenderer.emitting = true;

        yield return new WaitForSecondsRealtime(tiempoDash);

        //---> Para que se mueva de nuevo el Mago Oscuro
        puedeMoverse = true;

        //---> Asignar su gravedad por defecto
        rb2D.gravityScale = _gravedadInicial;
        trailRenderer.emitting = false;

        //---> Sonido del del Mago Oscuro al realizar un Dash
        _audioManager.PlayMagoOscuro(dash, volumenDash);

        //---> llamar Metodo para Retirar el Material del Mago Oscuro
        RetirarMaterial();
    }



    public void SaltarAdelante()
    {
        //---> Sonido del Mago Oscuro al iniciar el salto hacia atras
        _audioManager.PlayMagoOscuro(saltoAdelante, volumenSaltoAdelante);

        //---> Saltar hacia arriba
        rb2D.AddForce(Vector2.up * fuerzaDeSaltoAdelante, ForceMode2D.Impulse);

        //---> Saltar hacia adelante
        rb2D.AddForce(transform.right * fuerzaDeAvance, ForceMode2D.Impulse);

        //---> Activar particulas de salto
        _particulasPolvoMagoOscuro.ParticulaSalto();
    }



    public void SaltarAtras()
    {
        //---> Sonido del Mago Oscuro al iniciar el salto hacia adeante
        // _audioManager.PlaySFX(saltoAtras, volumenSaltoAtras);

        //---> Saltar hacia arriba
        rb2D.AddForce(Vector2.up * fuerzaDeSaltoAtras, ForceMode2D.Impulse);

        //---> Saltar hacia adelante
        rb2D.AddForce(-transform.right * fuerzaDeRetroceso, ForceMode2D.Impulse);

        //---> Activar particulas de salto
        _particulasPolvoMagoOscuro.ParticulaSalto();
    }



    public void Ataque()
    {
        //---> Arreglo de objetos que se tocan cuando se ataca
        Collider2D [] objetos = Physics2D.OverlapCircleAll(hitBox.position, radioAtaque);

        foreach (Collider2D collision in objetos)
        {
            //---> Solo hacer el contacto con el HurtBox del Mago
            if (collision.gameObject.layer == LayerMask.NameToLayer("Hurt"))
            {
                //---> Obtener la matriz de contactos de la colisión
                ContactPoint2D[] contactos = new ContactPoint2D[1];
                collision.GetContacts(contactos);

                //---> Iterar sobre los contactos para obtener la información de cada contacto
                foreach (ContactPoint2D contacto in contactos)
                {
                    //---> Obtener punto de contacto y la normal
                    Vector2 puntoDeContacto = contactos[0].point;
                    Vector2 normal = contactos[0].normal;

                    //---> Mandar el daño al enemigo con el punto de contacto y la normal
                    _saludMago.TomarDaño(dañoAtaque, puntoDeContacto, normal);
                }
            }
        }

        //---> llamar Metodo para Retirar el Material del Mago Oscuro
        RetirarMaterial();
    }



    //---> Rebote del Mago Oscuro al ser atacado
    public void ReboteGolpe(Vector2 puntoGolpe, Vector2 normal)
    {
        //---> Calcular la nueva velocidad
        Vector2 nuevaVelocidad = new Vector2(velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);

        //---> Aplicar la fuerza de rebote
        rb2D.velocity = nuevaVelocidad;

        //---> Aplicar la fuerza de rebote en la dirección normal al punto de contacto
        rb2D.AddForce(normal * fuerzaRebote, ForceMode2D.Impulse);
    }



    //---> Sonido de los pasos del pie derecho del Mago Oscuro al caminar
    public void SonidoPasoDerecho()
    {
        _audioManager.PlayMagoOscuro(pasoDerecho, volumenPasoDerecho);
    }



    //---> Sonido de los pasos del pie izquierdo del Mago Oscuro al caminar
    public void SonidoPasoIzquierdo()
    {
        _audioManager.PlayMagoOscuro(pasoIzquierdo, volumenPasoIzquierdo);
    }



    //---> Sonido del Mago al realizar el primer ataque
    public void SonidoPrimerAtaque()
    {
        _audioManager.PlayMagoOscuro(primerAtaque, volumenPrimerAtaque);
    }



    //---> Sonido del Mago al realizar el segundo ataque
    public void SonidoSegundoAtaque()
    {
        _audioManager.PlayMagoOscuro(segundoAtaque, volumenSegundoAtaque);
    }



    private void OnDrawGizmos()
    {
        //---> Dibujar el circulo de ataque
        if (dibujarCirculo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position, radioAtaque);
        }

        //---> Dibujar la caja de salto
        if (dibujarCaja) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(puntoDeSuelo.position, dimensionesCaja);
        }
    }



    //---> Cuando detecte que el Mago cae en la cabeza del Mago Oscuro hace un gruñido
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mago"))
        {
            if (collision.GetContact(0).normal.y <= -0.9)
            {
                _audioManager.PlayMagoOscuro(gruñidoMagoOscuro, volumenGruñidoMagoOscuro);
            }
        }
    }



    //---> Metodo para Asignar el Material inicial del Mago Oscuro
    public void AsignarMaterial()
    {
        _capsuleColliderMagoOscuro.sharedMaterial = materialResbaladizo;
    }



    //---> Metodo para Retirar el Material del Mago Oscuro
    public void RetirarMaterial()
    {
        _capsuleColliderMagoOscuro.sharedMaterial = null;
    }



    private void LateUpdate()
    {
        _animator.SetBool("EstaEnSuelo", enSuelo);
        _animator.SetFloat("VelocidadVertical", rb2D.velocity.y);
    }
}
