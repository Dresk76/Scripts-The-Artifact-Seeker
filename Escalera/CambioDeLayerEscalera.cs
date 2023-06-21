using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeLayerEscalera : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _entro;


    [Header("REFERENCIAS DEL MAGO")]
    private SpriteRenderer _spriteRendererMago;
    private TrailRenderer _trailMago;
    private ParticleSystemRenderer _particulasMovimiento;
    private ParticleSystemRenderer _particulasCaida;
    private ParticleSystemRenderer _particulasSalto;


    [Header("REFERENCIAS DE OTRAS CLASES")]
    private Escalera _escalera;


    [Header("VARIABLES PUBLICAS")]
    [HideInInspector] public bool magoEnRango;





    private void Awake()
    {
        _spriteRendererMago = GameObject.FindGameObjectWithTag("Mago").GetComponent<SpriteRenderer>();
        _trailMago = GameObject.Find("TrailMago").GetComponent<TrailRenderer>();
        _particulasMovimiento = GameObject.Find("ParticulasMovimiento").GetComponent<ParticleSystemRenderer>();
        _particulasCaida = GameObject.Find("ParticulasCaida").GetComponent<ParticleSystemRenderer>();
        _particulasSalto = GameObject.Find("ParticulasSalto").GetComponent<ParticleSystemRenderer>();
        _escalera = FindObjectOfType<Escalera>();
    }



    private void Update()
    {
        if (magoEnRango && !_escalera.escalando)
        {
            //---> Cambiar orden in layer del Mago y sus hijos al entrar en el collider
            _spriteRendererMago.sortingOrder = 16;
            _trailMago.sortingOrder = 15;
            _particulasMovimiento.sortingOrder = 16;
            _particulasCaida.sortingOrder = 16;
            _particulasSalto.sortingOrder = 16;
            _entro = false;
        }
        else if (_escalera.escalando)
        {
            if (!_entro)
            {
                StartCoroutine(EntrarEnLaEscalera());
            }
        }
    }



    private IEnumerator EntrarEnLaEscalera()
    {
        //---> Cambiar orden in layer del Mago y sus hijos al entrar en la escalera
        _entro = true;
        yield return new WaitForSecondsRealtime(0.2f);
        _spriteRendererMago.sortingOrder = 11;
        _trailMago.sortingOrder = 10;
        _particulasMovimiento.sortingOrder = 11;
        _particulasCaida.sortingOrder = 11;
        _particulasSalto.sortingOrder = 11;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            magoEnRango = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            magoEnRango = false;
        }
    }
}
