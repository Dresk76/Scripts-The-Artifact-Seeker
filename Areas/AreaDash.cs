using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDash : MonoBehaviour
{
    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("TRANSPARENCIA DASH")]
    [SerializeField] private GameObject transparenciaDash;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    //---> Si el mago entra en el rango no puede atacar
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _mago.puedeHacerDash = false;
            transparenciaDash.SetActive(true);
        }
    }



    //---> Si el mago sale del rango puede volver a atacar
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _mago.puedeHacerDash = true;
            transparenciaDash.SetActive(false);
        }
    }
}
