using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Links : MonoBehaviour
{
    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip redSocial;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenRedSocial = 0.15f;


    [Header("NOMBRES PDF")]
    [SerializeField] private string pdfSpanish;
    [SerializeField] private string pdfEnglish;


    [Header("REFERENCIA OBJETOS ENTRE ESCENAS")]
    private ObjetosEntreEscenas _objetosEntreEscenas;


    [Header("REFERENCIAS DEL MAGO")]
    private Animator _animatorMago;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _objetosEntreEscenas = GameObject.FindGameObjectWithTag("ObjetosEntreEscenas").GetComponent<ObjetosEntreEscenas>();
        _animatorMago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Animator>();
    }



    //---> Metodo para abrir las Redes
    public void OpenRedes(string enlace)
    {
        //---> Para que haga un ataque al Abrir un link de una Red Social
        _animatorMago.SetTrigger("Invocar");

        //---> Abrir link en el explorador
        Application.OpenURL(enlace);

        //---> Sonido al abrir una red social
        _audioManager.PlaySFX(redSocial, volumenRedSocial);
    }



    //---> Metodo para abrir los PDFs
    public void OpenPDF()
    {
        //---> Pasar para abrir el PDF dependiendo si esta en idioma Español o Ingles
        string pdf = (_objetosEntreEscenas.idioma == 0) ? pdfSpanish : pdfEnglish;

        //---> Para que haga un ataque al Abrir el PDF
        _animatorMago.SetTrigger("Invocar");

        //---> Abrir link en el explorador que esta en los servidores de GitHub
        Application.OpenURL(pdf);

        //---> Sonido al abrir una red social
        _audioManager.PlaySFX(redSocial, volumenRedSocial);
    }
}
