using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class MenuIdioma : MonoBehaviour
{
    [Header("IDIOMAS")]
    [SerializeField] private TMP_Text idiomasText;
    [SerializeField] private List<string> idiomas = new List<string>();
    private int indiceActual = 0; // Variable para controlar el índice actual de la lista


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



    //---> Metodo para avanzar en la eleccion de un Idioma
    public void SiguienteIdioma()
    {
        //---> Incrementar el índice actual y asegurarse de que no se salga del rango de la lista
        /*
            EJEMPLO: Supongamos que la variable "indiceActual" tiene un valor de 2 
            y la lista "idiomas" tiene 5 elementos. Si se llama a la línea de código 
            anterior, el valor de "indiceActual" se actualizará de la siguiente manera:

            1. "indiceActual + 1" da como resultado 3.
            2. "3 % 5" da como resultado 3, ya que 3 dividido por 5 da un resto de 3.
            3. "indiceActual = 3" actualiza la variable "indiceActual" con el resultado.
            
            De esta manera, la línea de código asegura que "indiceActual" sea un índice 
            válido en la lista "idiomas", incluso si se intenta incrementar su valor más 
            allá del final de la lista. Al utilizar el operador módulo "%" de esta manera, 
            "indiceActual" siempre será un número entre 0 y "idiomas.Count - 1".
        */
        indiceActual = (indiceActual + 1) % idiomas.Count;
    
        //---> Asignar el texto correspondiente al índice actual a la propiedad text del componente TMP_Text
        idiomasText.text = idiomas[indiceActual];
        //---> Asignar a la variable idioma el numero elegido del idioma, pasandole el indice actual de la liosta idiomas
        _objetosEntreEscenas.idioma = idiomas.IndexOf(idiomas[indiceActual]);

        //---> Llamar metodo CambiarIdioma() y pasarle el idioma actual, para que haga el cambio
        CambiarIdioma(_objetosEntreEscenas.idioma);
    }



    //---> Metodo para retroceder en la eleccion de un Idioma
    public void IdiomaAnterior()
    {
        //---> Decrementar el índice actual y asegurarse de que no se salga del rango de la lista
        /*
            Ejemplo: Supongamos que la variable "indiceActual" tiene un valor de 0 y la 
            lista "idiomas" tiene 5 elementos. Si se llama a la línea de código anterior, el 
            valor de "indiceActual" se actualizará de la siguiente manera:

            1. "indiceActual - 1" da como resultado -1.
            2. "-1 + 5" da como resultado 4, ya que se está sumando la cantidad total de 
            elementos de la lista "idiomas" al resultado anterior.
            3. "4 % 5" da como resultado 4, ya que 4 dividido por 5 da un resto de 4.
            4. "indiceActual = 4" actualiza la variable "indiceActual" con el resultado.
            
            De esta manera, la línea de código asegura que "indiceActual" sea un índice 
            válido en la lista "idiomas", incluso si se intenta disminuir su valor más allá 
            del principio de la lista. Al utilizar el operador módulo "%" de esta manera, 
            "indiceActual" siempre será un número entre 0 y "idiomas.Count - 1".
        */
        indiceActual = (indiceActual - 1 + idiomas.Count) % idiomas.Count;
    
        //---> Asignar el texto correspondiente al índice actual a la propiedad text del componente TMP_Text
        idiomasText.text = idiomas[indiceActual];
        //---> Asignar a la variable idioma el numero elegido del idioma, pasandole el indice actual de la liosta idiomas
        _objetosEntreEscenas.idioma = idiomas.IndexOf(idiomas[indiceActual]);

        //---> Llamar metodo CambiarIdioma() y pasarle el idio, para que haga el cambio
        CambiarIdioma(_objetosEntreEscenas.idioma);
    }



    //---> Metodo para cambiar el Idioma
    public void CambiarIdioma(int indiceIdioma)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[indiceIdioma];
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

        //---> Cerrar el Menu de Idioma
        this.gameObject.SetActive(false);

        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        //---> Indicar que esta saliendo del Menu de opciones
        _objetosEntreEscenas.estaEnOpciones = false;
    }



    //---> Cerrar el Menu de Idioma
    public void LlamarVolver()
    {
        StartCoroutine(Volver());
    }
}
