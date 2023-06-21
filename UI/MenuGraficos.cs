using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuGraficos : MonoBehaviour
{
    [Header("PANTALLA COMPLETA")]
    [SerializeField] private Toggle fullsScreenToggle;


    [Header("RESOLUCION")]
    [SerializeField] private TMP_Text resolutionText;
    Resolution[] resolutions; // Resoluciones disponibles en cada ordenador
    private List<Resolution> filterResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0; // Index de la resolución actual


    [Header("MENSAJE DE CONFIRMACION")]
    [SerializeField] private GameObject menuDisquete;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("REFERENCIAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;
    private SonidosUI _sonidosUI;





    private void Awake()
    {
        // _changeCursor = GameObject.Find("CursorManager").GetComponent<ChangeCursor>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _sonidosUI = _objetosEntreEscenas.GetComponent<SonidosUI>();
    }



    private void Start()
    {
        //---> Al iniciar validar si se esta en Pantalla completa o modo ventana y marcar o desmarcar el toggle
        fullsScreenToggle.isOn = Screen.fullScreen;

        //---> Llamar la función ReviewResolution
        ReviewResolution();
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



    private void ReviewResolution()
    {
        //---> Detecta las reosoluciones disponibles del ordenador
        resolutions = Screen.resolutions;
        filterResolutions = new List<Resolution>();

        currentRefreshRate = Screen.currentResolution.refreshRate;

        // Debug.Log("RefreshRate: " + currentRefreshRate);

        for (int i = 0; i < resolutions.Length; i++)
        {
            // Debug.Log("Resolution: " + resolutions[i]);
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filterResolutions.Add(resolutions[i]);
            }
        }

        //---> Lista para guardar cada tamaño de la resolucion
        List<string> options = new List<string>();
        for (int i = 0; i < filterResolutions.Count; i++)
        {
            string resolutionOption = filterResolutions[i].width + "x" + filterResolutions[i].height + " " + filterResolutions[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            if (filterResolutions[i].width == Screen.width && filterResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        //---> Agregar las opciones de la lista en (resolutionText)
        resolutionText.text = options[currentResolutionIndex];
    }



    public void SetResolution(int resolutionIndex)
    {
        //---> El Resolution es igual al indice de valores de (resolucionesDropDown)
        Resolution resolution = filterResolutions[resolutionIndex];
        // resolution = filteredResolutions[resolutionIndex];

        //---> Toma el valor del indice realiza el camnbio a esa resolucion indicada
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        //---> Actualizar el texto con la resolución seleccionada
        resolutionText.text = resolution.width + " x " + resolution.height + resolution.refreshRate + "Hz";
    }



    public void SetNextResolution()
    {
        currentResolutionIndex++;

        if (currentResolutionIndex >= resolutions.Length)
        {
            currentResolutionIndex = 0;
        }

        // Debug.Log("SetNextResolution = Current Resolution Index: " + currentResolutionIndex);

        SetResolution(currentResolutionIndex);
    }



    public void SetPreviousResolution()
    {
        currentResolutionIndex--;

        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutions.Length - 1;
        }

        // Debug.Log("SetPreviousResolution = Current Resolution Index: " + currentResolutionIndex);

        SetResolution(currentResolutionIndex);
    }



    //---> Cambiar a pantalla completa si presiona la tecla G
    public void PantallaCompleta(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
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

        //---> Cerrar el Menu de Graficos
        this.gameObject.SetActive(false);

        //---> Restaurar el cursor a defaultCursor
        _changeCursor.OnCursorExit();

        //---> Indicar que esta saliendo del Menu de opciones
        _objetosEntreEscenas.estaEnOpciones = false;
    }



    //---> Cerrar el Menu de Graficos
    public void LlamarVolver()
    {
        StartCoroutine(Volver());
    }
}