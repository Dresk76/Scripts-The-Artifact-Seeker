using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPuerta : MonoBehaviour
{
    [Header("CONDICIONES")]
    private bool _magoEnRango;


    [Header("TIEMPO PARA TRATAR DE ABRIR LA PUERTA")]
    [Range(0, 1f)][SerializeField] private float _tiempoEntreAtaques = 0.3f;
    [SerializeField] private float _tiempoSiguienteAtaque;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip puertaBloqueada;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenPuertaBloqueada = 0.4f;


    [Header("PUERTA")]
    [SerializeField] private Puerta puerta;


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("TRANSPARENCIA ATAQUES")]
    [SerializeField] private GameObject transparenciaPrimerAtaque;
    [SerializeField] private GameObject transparenciaSegundoAtaque;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    private void Update()
    {
        //---> Si esta activo el menu de Pausa no puede realizar nada en el update
        if(_controlDelTiempo.getEstaPausado())
            return;

        //---> Tiempo para poder volver a intentar abrir la puerta
        if (_tiempoSiguienteAtaque > 0)
        {
            _tiempoSiguienteAtaque -= Time.deltaTime;
        }


        if (_magoEnRango && Input.GetButtonDown("Fire1") && _tiempoSiguienteAtaque <= 0)
        {
            if (!puerta.GetEstadoDePuerta())
            {
                //---> Sonido de puerta bloqueada al estar en el rango y presionar el ataque 1
                _audioManager.PlayPuerta(puertaBloqueada, volumenPuertaBloqueada);

                //---> Tiempo para volver a intentar abrir la puerta
                _tiempoSiguienteAtaque = _tiempoEntreAtaques;
            }
        }
    }



    //---> Metodo para no atacar en el area indicada
    private void NoAtacar(bool noAtacar)
    {
        if (noAtacar)
        {
            _mago.noAtacar = true;
            transparenciaPrimerAtaque.SetActive(true);
            transparenciaSegundoAtaque.SetActive(true);
        }
        else
        {
            _mago.noAtacar = false;
            transparenciaPrimerAtaque.SetActive(false);
            transparenciaSegundoAtaque.SetActive(false);
        }
    }



    //---> Si el mago entra en el rango no puede atacar
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _magoEnRango = true;

            //---> Llamar metodo para no atacar
            NoAtacar(_magoEnRango);
        }
    }



    //---> Si el mago sale del rango puede volver a atacar
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            _magoEnRango = false;

            //---> Llamar metodo para no atacar
            NoAtacar(_magoEnRango);
        }
    }
}
