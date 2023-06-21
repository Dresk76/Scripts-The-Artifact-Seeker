using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosEntreEscenas : MonoBehaviour
{
    [Header("MENU INICIAL")]
    //---> Para activar el Menu Inicial
    public GameObject menuInicial;


    [Header("MENU A ACTIVAR")]
    //---> Para activar el Menu de Graficos o el Menu Idioma dependiendo del dispositivo en que se encuentre
    public GameObject menuOpciones;


    [Header("CAMBIO DE IDIOMA")]
    //---> Para indicar en que idioma esta siendo elejido
    public int idioma; // 0 - Español 1 - Ingles


    [Header("EN MENU OPCIONES")]
    //---> Para indicar cuando entra o sale del Menu de Opciones
    public bool estaEnOpciones = false;


    [Header("EN MENU SALIR")]
    //---> Para indicar cuando entra o sale del Menu Salir
    public bool estaEnSalir = false;


    [Header("EN MENU FIN PORTAFOLIO")]
    //---> Para indicar cuando entra o sale del Menu Fin Portafolio
    public bool estaEnFinPortafolio = false;
}
