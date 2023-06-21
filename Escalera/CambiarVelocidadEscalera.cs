using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarVelocidadEscalera : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _magoEnRango;


    [Header("VELOCIDADES")]
    private float _velocidadActual;
    private float _velocidadAlEntrarEscalera = 80f;
    private float _velocidadAlMantenerEscalera = 100f;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("REFERENCIAS DE OTRAS CLASES")]
    private Escalera _escalera;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _escalera = FindObjectOfType<Escalera>();
    }



    private void Start()
    {
        //---> Guardar la velocidad actual del Mago
        _velocidadActual = _mago.velocidadDeMovimiento;
    }



    private void Update()
    {
        if (_magoEnRango)
        {
            CambioDeVelocidad();
        }
        else
        {
            //---> Si sale del rango asignar la velocidad inicial que se guardo
            _mago.velocidadDeMovimiento = _velocidadActual;
        }
    }



    //---> Para poder aplicar el mismo script en ambas escaleras segun el signo que se active
    private void CambioDeVelocidad()
    {
        for (int i = 0; i < _escalera.signos.Length; i++)
        {
            switch(_escalera.signos[i].name)
            {
                case "TeclaInteractiva_1":
                    if (_escalera.signos[i].activeSelf)
                    {
                        if(_mago.input.x >= 0f)
                        {
                            _mago.velocidadDeMovimiento = _velocidadAlEntrarEscalera;
                        }
                        else
                        {
                            _mago.velocidadDeMovimiento = _velocidadAlMantenerEscalera;
                        }
                    }
                    break;

                case "TeclaInteractiva_2":
                    if (_escalera.signos[i].activeSelf)
                    {
                        if(_mago.input.x <= 0f)
                        {
                            _mago.velocidadDeMovimiento = _velocidadAlEntrarEscalera;
                        }
                        else
                        {
                            _mago.velocidadDeMovimiento = _velocidadAlMantenerEscalera;
                        }
                    }
                    break;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            _magoEnRango = true;

            //---> Restringir el ataque del Mago
            _mago.estaEnEscaleras = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            _magoEnRango = false;

            //---> Habilitar el ataque del Mago
            _mago.estaEnEscaleras = false;
        }
    }
}
