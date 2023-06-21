using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulasPolvoMago : MonoBehaviour
{
    [Header("PARTICULAS DE MOVIMIENTO")]
    [SerializeField] private ParticleSystem particulasMovimiento;
    [Range(0, 3)][SerializeField] private int reproducirDespuesDeVelocidad = 1;
    [Range(0, 0.2f)][SerializeField] private float periodoFormacionPolvo = 0.15f;


    [Header("PARTICULAS DE CAIDA")]
    [SerializeField] private ParticleSystem particulasDeCaida;


    [Header("PARTICULAS DE SALTO")]
    [SerializeField] private ParticleSystem particulasDeSalto;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("CONTADOR")]
    private float _contador;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    private void Update()
    {
        //---> Iniciar el contador
        _contador += Time.deltaTime;

        /*
            Si esta el Mago esta en el sueloo y el valor abs del movimiento del mago es mayor 
            al valor de reproducirDespuesDeVelocidad, se activan las particulas.
        */
        if (_mago.enSuelo && Mathf.Abs(_mago.rb2D.velocity.x) > reproducirDespuesDeVelocidad)
        {
            if (_contador > periodoFormacionPolvo)
            {
                particulasMovimiento.Play();
                _contador = 0;
            }
        }
    }



    //---> Metodo para llamar al terminar el estado de LevantarseDeSalto desde MagoCaidaBehaviour
    public void ParticulasCaida()
    {
        particulasDeCaida.Play();
    }



    //---> Metodo para la particula de salto
    public void ParticulaSalto()
    {
        particulasDeSalto.Play();
    }
}
