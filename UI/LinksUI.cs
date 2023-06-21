using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinksUI : MonoBehaviour
{
    [Header("NOMBRES PDF")]
    [SerializeField] private string pdfSpanish;
    [SerializeField] private string pdfEnglish;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;





    private void Awake()
    {
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
    }



    //---> Metodo para abrir las Redes
    public void OpenRedes(string enlace)
    {
        //---> Abrir link en el explorador
        Application.OpenURL(enlace);
    }



    //---> Metodo para abrir los PDFs
    public void OpenPDF()
    {
        //---> Pasar para abrir el PDF dependiendo si esta en idioma Español o Ingles
        string pdf = (_objetosEntreEscenas.idioma == 0) ? pdfSpanish : pdfEnglish;

        //---> Abrir link en el explorador que esta en los servidores de GitHub
        Application.OpenURL(pdf);
    }
}
