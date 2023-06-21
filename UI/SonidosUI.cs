using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidosUI : MonoBehaviour
{
    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip enter;
    [SerializeField] private AudioClip portafolio;
    [SerializeField] private AudioClip opciones;
    [SerializeField] private AudioClip pestanas;
    [SerializeField] private AudioClip check;
    [SerializeField] private AudioClip flechas;
    [SerializeField] private AudioClip volver;
    [SerializeField] private AudioClip creditos;
    [SerializeField] private AudioClip salir;
    [SerializeField] private AudioClip redes;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenEnter = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenPortafolio = 0.8f;
    [Range(0, 1f)][SerializeField] private float volumenOpciones = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenPestanas = 0.45f;
    [Range(0, 1f)][SerializeField] private float volumenCheck = 0.4f;
    [Range(0, 1f)][SerializeField] private float volumenFlechas = 0.6f;
    [Range(0, 1f)][SerializeField] private float volumenVolver = 1f;
    [Range(0, 1f)][SerializeField] private float volumenCreditos = 0.5f;
    [Range(0, 1f)][SerializeField] private float volumenSalir = 0.55f;
    [Range(0, 1f)][SerializeField] private float volumenRedes = 0.7f;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }



    public void SonidoEnter()
    {
        _audioManager.PlayEnterUI(enter, volumenEnter);
    }



    public void SonidoPortafolio()
    {
        _audioManager.PlaySFX(portafolio, volumenPortafolio);
    }



    public void SonidoOpciones()
    {
        _audioManager.PlaySFX(opciones, volumenOpciones);
    }



    public void SonidoPestanas()
    {
        _audioManager.PlayPestanasUI(pestanas, volumenPestanas);
    }



    public void SonidoCheck()
    {
        _audioManager.PlaySFX(check, volumenCheck);
    }



    public void SonidoFlechas()
    {
        _audioManager.PlaySFX(flechas, volumenFlechas);
    }



    public void SonidoVolver()
    {
        _audioManager.PlaySFX(volver, volumenVolver);
    }



    public void SonidoCreditos()
    {
        _audioManager.PlaySFX(creditos, volumenCreditos);
    }



    public void SonidoSalir()
    {
        _audioManager.PlaySFX(salir, volumenSalir);
    }



    public void SonidoRedes()
    {
        _audioManager.PlaySFX(redes, volumenRedes);
    }
}
