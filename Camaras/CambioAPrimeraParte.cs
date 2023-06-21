using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CambioAPrimeraParte : MonoBehaviour
{
    [Header("FUERZA DE SALTO MAGO")]
    private float _fuerzaDeSaltoInicial;

    [Header("CAMARAS")]
    [SerializeField] private CinemachineStateDrivenCamera camaraMago_1;
    [SerializeField] private CinemachineVirtualCamera camaraMago_2;


    [Header("ACTIVAR SONIDOS")]
    [SerializeField] private AudioSource[] sonidos;


    [Header("REFERENCIA DEL MAGO")]
    private Mago _mago;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    private void Start()
    {
        //---> Al iniciar guardar la fuerza de salto inicial del Mago
        _fuerzaDeSaltoInicial = _mago.fuerzaDeSalto;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Llamar metodo de GuardarDatos
            GuardarDatos();

            //---> Cambio de la velocidad de la fuerza de salto del Mago
            _mago.fuerzaDeSalto = _fuerzaDeSaltoInicial;

            //---> Cambio de Camaras
            camaraMago_1.gameObject.SetActive(true);
            camaraMago_2.gameObject.SetActive(false);

            //---> Desactivar sonidos de cupulas del quinto piso
            for (int i = 0; i < sonidos.Length; i++)
            {
                sonidos[i].Stop();
            }
        }
    }



    //---> Guardar la posicion del Mago al entrar en el Area de CambioAPrimeraParte
    private void GuardarDatos()
    {
        //---> Guardar los datos del Mago
        SaveManager.GuardarDatosMago(_mago);
        // Debug.Log("Datos guardados desde Script: CambioAPrimeraCamara");
    }
}
