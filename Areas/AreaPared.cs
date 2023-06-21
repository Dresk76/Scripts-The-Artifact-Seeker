using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPared : MonoBehaviour
{
    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("TRANSPARENCIA ATAQUES")]
    [SerializeField] private GameObject transparenciaPrimerAtaque;
    [SerializeField] private GameObject transparenciaSegundoAtaque;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    //---> Metodo para no atacar en el area indicada
    private void NoAtacar(bool noAtacar)
    {
        if (noAtacar)
        {
            _mago.noAtacar = true;
            transparenciaPrimerAtaque.SetActive(true);
            transparenciaSegundoAtaque.SetActive(true);
        }
        else
        {
            _mago.noAtacar = false;
            transparenciaPrimerAtaque.SetActive(false);
            transparenciaSegundoAtaque.SetActive(false);
        }
    }



    //---> Si el mago entra en el rango no puede atacar
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Llamar metodo para no atacar
            NoAtacar(true);
        }
    }



    //---> Si el mago sale del rango puede volver a atacar
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Llamar metodo para no atacar
            NoAtacar(false);
        }
    }
}
