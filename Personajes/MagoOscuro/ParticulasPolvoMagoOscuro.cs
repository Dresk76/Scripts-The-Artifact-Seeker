using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulasPolvoMagoOscuro : MonoBehaviour
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
    private MagoOscuro _magoOscuro;


    [Header("CONTADOR")]
    private float _contador;





    private void Awake()
    {
        _magoOscuro = GameObject.FindGameObjectWithTag("MagoOscuro").GetComponent<MagoOscuro>();
    }



    private void Update()
    {
        //---> Iniciar el contador
        _contador += Time.deltaTime;

        /*
            Si esta el Mago esta en el suelo y el valor abs del movimiento del mago es mayor 
            al valor de reproducirDespuesDeVelocidad, se activan las particulas.
        */
        if (_magoOscuro.enSuelo && Mathf.Abs(_magoOscuro.rb2D.velocity.x) > reproducirDespuesDeVelocidad)
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
