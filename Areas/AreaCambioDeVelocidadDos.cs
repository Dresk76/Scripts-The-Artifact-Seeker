using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCambioDeVelocidadDos : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _magoEnRango;


    [Header("VELOCIDADES")]
    private float _velocidadActual;
    private float _velocidadAlEntrar = 50f;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;





    private void Awake()
    {
        _mago = FindObjectOfType<Mago>();
    }   



    private void Start()
    {
        _velocidadActual = _mago.velocidadDeMovimiento;
    }



    private void Update()
    {
        if (_magoEnRango)
        {
            _mago.velocidadDeMovimiento = _velocidadAlEntrar;
        }
        else
        {
            _mago.velocidadDeMovimiento = _velocidadActual;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            _magoEnRango = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            _magoEnRango = false;
        }
    }
}
