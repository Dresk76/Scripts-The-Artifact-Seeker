using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextosPasilloInicial : MonoBehaviour
{
    [Header("TEXTOS")]
    [SerializeField] private GameObject[] textosEspanol;
    [SerializeField] private GameObject[] textosIngles;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;





    private void Awake()
    {
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
    }




    private void Update()
    {
        //---> Hace referencia a un if. 
        /*
            Si isSpanishActive es iaual a 0 entonces es true
            else
            Si isSpanishActive es false
        */
        bool isSpanishActive = (_objetosEntreEscenas.idioma == 0);

        /*
            Llamar el metodo SetActiveTextos para cada lista de idioma
            Pasarles el estado de isSpanishActive
            - Activar los textos en español si isSpanishActive es true o los desactivarlos si es false
            - Activar los textos en inglés si isSpanishActive es false o los desactivarlos si es true.
        */
        SetActiveTextos(textosEspanol, isSpanishActive);
        SetActiveTextos(textosIngles, !isSpanishActive);
    }



    //---> Metodo para recorrer las dos listas y activar o desactivar segun sea el caso
    private void SetActiveTextos(GameObject[] textos, bool isActive)
    {
        foreach (GameObject texto in textos)
        {
            texto.SetActive(isActive);
        }
    }
}
