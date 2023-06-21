using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCreditos : MonoBehaviour
{
    [Header("REFERENCIAS")]
    private SonidosUI _sonidosUI;





    private void Awake()
    {
        _sonidosUI = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<SonidosUI>();
    }



    private void Update()
    {
        //---> Volver al menu inicial al pulsar Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //---> Llamar metodo Volver()
            Volver();
        }
    }



    //---> Metodo para cerrar el Menu de Creditos
    private void Volver()
    {
        //---> Reproducir Sonido de Volver
        _sonidosUI.SonidoVolver();

        //---> Cerrar el Menu de Creditos
        this.gameObject.SetActive(false);
    }
}
