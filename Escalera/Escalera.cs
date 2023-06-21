using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalera : MonoBehaviour
{
    [Header("TECLA DE INTERACCION")]
    [SerializeField] private GameObject teclaInteractiva;


    [Header("SIGNOS")]
    public GameObject[] signos; // Signos al entrar en las escaleras


    [Header("MATERIAL")]
    [SerializeField] private PhysicsMaterial2D material;
    private PhysicsMaterial2D _materialInicial;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("REFERENCIAS DE OTRAS CLASES")]
    private ActivarEscaleraArriba _activarEscaleraArriba;
    private ActivarEscaleraAbajo _activarEscaleraAbajo;
    private CambioDeLayerEscalera _cambioDeLayerEscalera;


    [Header("REFERENCIAS")]
    [SerializeField] private CapsuleCollider2D capsuleColliderMago;
    [SerializeField] private EdgeCollider2D edgeColliderEscalera;
    [SerializeField] private BoxCollider2D boxColliderCambiarVelocidad;


    [Header("VARIABLES PUBLICAS")]
    [HideInInspector] public bool magoEnRango;
    [HideInInspector] public bool escalando;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _activarEscaleraArriba = FindObjectOfType<ActivarEscaleraArriba>();
        _activarEscaleraAbajo = FindObjectOfType<ActivarEscaleraAbajo>();
        _cambioDeLayerEscalera = FindObjectOfType<CambioDeLayerEscalera>();
    }



    private void Start()
    {
        //---> Guardar el material inicial del Mago
        _materialInicial = capsuleColliderMago.sharedMaterial;
    }



    private void Update()
    {
        if (magoEnRango)
        {
            //---> Detectar escalera
            Escalar();

            //---> Detenerse en la escalera
            DetenerseEnEscalera();
        }
        else
        {
            //---> Asignar el material inicial al Mago
            capsuleColliderMago.sharedMaterial = _materialInicial;
        }
    }



    // Detectar escalera
    private void Escalar()
    {
        if ((_activarEscaleraArriba.magoEnRango || _activarEscaleraAbajo.magoEnRango))
        {
            if (!escalando)
            {
                StartCoroutine(ActivarBoxColliderCambiarVelocidad());
            }
        }
        else
        {
            escalando = false;
            edgeColliderEscalera.gameObject.layer = 0;
            edgeColliderEscalera.isTrigger = true;
            boxColliderCambiarVelocidad.gameObject.layer = 0;
        }
    }



    private IEnumerator ActivarBoxColliderCambiarVelocidad()
    {
        edgeColliderEscalera.gameObject.layer = LayerMask.NameToLayer("Suelo");
        edgeColliderEscalera.isTrigger = false;
        escalando = true;
        yield return new WaitForSecondsRealtime(1f);
        boxColliderCambiarVelocidad.gameObject.layer = LayerMask.NameToLayer("Suelo");
    }



    //---> Para poder aplicar el mismo script en ambas escaleras segun el signo que se active
    private void DetenerseEnEscalera()
    {
        for (int i = 0; i < signos.Length; i++)
        {
            switch(signos[i].name)
            {
                case "TeclaInteractiva_1":
                    if (signos[i].activeSelf)
                    {
                        if (_mago.input.x >= 0f && !_cambioDeLayerEscalera.magoEnRango)
                        {
                            RetirarMaterial();
                        }
                        else if (_mago.input.x < 0f)
                        {
                            AsignarMaterial();
                        }
                    }
                    break;

                case "TeclaInteractiva_2":
                    if (signos[i].activeSelf)
                    {
                        if (_mago.input.x <= 0f && !_cambioDeLayerEscalera.magoEnRango)
                        {
                            RetirarMaterial();
                        }
                        else if (_mago.input.x > 0f)
                        {
                            AsignarMaterial();
                        }
                    }
                    break;
            }
        }
    }



    //---> Metodo para Retirar el Material del Mago
    private void RetirarMaterial()
    {
        capsuleColliderMago.sharedMaterial = null;
    }



    //---> Metodo para Asignar el Material del Mago
    private void AsignarMaterial()
    {
        capsuleColliderMago.sharedMaterial = material;
    }



    //---> Cambios de velocidad en la escalera
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            teclaInteractiva.SetActive(true);
            magoEnRango = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            teclaInteractiva.SetActive(false);
            magoEnRango = false;
            _activarEscaleraArriba.magoEnRango = false;
            _activarEscaleraAbajo.magoEnRango = false;
        }
    }
}
