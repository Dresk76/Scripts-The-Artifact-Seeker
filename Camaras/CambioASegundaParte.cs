using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CambioASegundaParte : MonoBehaviour
{
    [Header("CAMARAS")]
    [SerializeField] private CinemachineStateDrivenCamera camaraMago_1;
    [SerializeField] private CinemachineVirtualCamera camaraMago_2;


    [Header("ACTIVAR SONIDOS")]
    [SerializeField] private AudioSource[] sonidos;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - CamaraMago2
        camaraMago_2.gameObject.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Cambio de la velocidad de la fuerza de salto del Mago
            _mago.fuerzaDeSalto = 8f;

            //---> Cambio de Camaras
            camaraMago_2.gameObject.SetActive(true);
            camaraMago_1.gameObject.SetActive(false);

            //---> Activar sonidos de cupulas
            for (int i = 0; i < sonidos.Length; i++)
            {
                sonidos[i].Play();
            }
        }
    }
}
