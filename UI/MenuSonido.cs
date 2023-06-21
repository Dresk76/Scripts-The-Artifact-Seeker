using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSonido : MonoBehaviour
{
    [Header("CONTROL DE AUDIO")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;


    [Header("MENSAJE DE CONFIRMACION")]
    [SerializeField] private GameObject menuDisquete;


    [Header("REFERENCIAS AUDIO")]
    private AudioManager _audioManager;
    [SerializeField] private AudioClip musicaMenuSonido;
    [SerializeField] private AudioClip sfxMenuSonido;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenMusicaMenuSonido = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenSFXMenuSonido = 0.4f;


    [Header("REFERENCIA DE CHANGECURSOR")]
    [SerializeField] private ChangeCursor _changeCursor;


    [Header("REFERENCIAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;
    private SonidosUI _sonidosUI;



    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _sonidosUI = _objetosEntreEscenas.GetComponent<SonidosUI>();
    }



    private void Start()
    {
        //---> Ejecutar los Metodos al iniciar para que sean compatibles los Sliders y el AudioMixer
        SetGeneralVolume();
        SetMusicVolume();
        SetSFXVolume();
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



    //---> Metodo para controlar el Volumen General
    public void SetGeneralVolume()
    {
        //---> Obtener el valor del slider
        float volume = generalSlider.value;

        //---> Establecer el valor del parametro del volumen del mezclador de audio en el valor deslizante
        //---> NOTA: Poner el valor Min Value del Slider en 0.0001 porque sino, no se silencia la musica al bajar del todo el Slider
        audioMixer.SetFloat("General", Mathf.Log10(volume)*20);
    }



    //---> Metodo para controlar el Volumen de la Musica
    public void SetMusicVolume()
    {
        //---> Obtener el valor del slider
        float volume = musicSlider.value;

        //---> Establecer el valor del parametro del volumen del mezclador de audio en el valor deslizante
        //---> NOTA: Poner el valor Min Value del Slider en 0.0001 porque sino, no se silencia la musica al bajar del todo el Slider
        audioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
    }



    //---> Metodo para controlar el Volumen de los Efectos (SFX)
    public void SetSFXVolume()
    {
        //---> Obtener el valor del slider
        float volume = SFXslider.value;

        //---> Establecer el valor del parametro del volumen del mezclador de audio en el valor deslizante
        //---> NOTA: Poner el valor Min Value del Slider en 0.0001 porque sino, no se silencia la musica al bajar del todo el Slider
        audioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
    }



    //---> Metodo para que reproduzca la musica del Menu de Sonido
    public void ReproducirMusicaMenuSonido()
    {
        _audioManager.PlayMusicMenuSonidoUI(musicaMenuSonido, volumenMusicaMenuSonido, true);
    }



    //---> Metodo para que detenga la musica del Menu de Sonido
    public void DetenerMusicaMenuSonido()
    {
        _audioManager.StopMusicMenuSonidoUI();
    }



    //---> Metodo para que reproduzca los efectos del Menu de Sonido
    public void ReproducirSFXMenuSonido()
    {
        _audioManager.PlaySFXMenuSonidoUI(sfxMenuSonido, volumenSFXMenuSonido, true);
    }



    //---> Metodo para que detenga los efectos del Menu de Sonido
    public void DetenerSFXMenuSonido()
    {
        _audioManager.StopSFXMenuSonidoUI();
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

        //---> Detener los efectos del Menu de Sonido
        DetenerSFXMenuSonido();

        //---> Detener la musica del Menu de Sonido
        DetenerMusicaMenuSonido();

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
