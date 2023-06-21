using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControles : MonoBehaviour
{
    [Header("MENSAJE DE CONFIRMACION")]
    [SerializeField] private GameObject menuDisquete;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("REFERENCIAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;
    private SonidosUI _sonidosUI;





    private void Awake()
    {
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _sonidosUI = _objetosEntreEscenas.GetComponent<SonidosUI>();
    }



    private void Update()
    {
        //---> Volver al menu inicial al pulsar Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //---> Iniciar corrutina Volver()
            StartCoroutine(Volver());
        }
    }



    //---> Mostrar casette de actualizacion de cambios antes de salir y cerrar el Menu de Graficos
    private IEnumerator Volver()
    {
        //---> Reproducir Sonido de Volver
        _sonidosUI.SonidoVolver();

        //---> Mostrar Disquete de guardado
        menuDisquete.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        menuDisquete.SetActive(false);

        //---> Cerrar el Menu de Controles
        this.gameObject.SetActive(false);

        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        //---> Indicar que esta saliendo del Menu de opciones
        _objetosEntreEscenas.estaEnOpciones = false;
    }



    //---> Cerrar el Menu de Controles
    public void LlamarVolver()
    {
        StartCoroutine(Volver());
    }
}
