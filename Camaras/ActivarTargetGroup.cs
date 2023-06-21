using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//---> Activar las camaras de los puntos donde estan los textos
public class ActivarTargetGroup: MonoBehaviour
{
    [Header("CAMARA TARGET GROUP")]
    [SerializeField] private CinemachineVirtualCamera camaraTargetGroup;





    private void Start()
    {
        //---> Desactivar al iniciar - Camra del Objeto
        camaraTargetGroup.gameObject.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            camaraTargetGroup.gameObject.SetActive(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            camaraTargetGroup.gameObject.SetActive(false);
        }
    }
}
