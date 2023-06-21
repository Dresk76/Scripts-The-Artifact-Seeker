using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Particulas_Sonido : MonoBehaviour
{
    [Header("ACTIVAR PARTICULAS")]
    [SerializeField] private ParticleSystem[] particulas;


    [Header("ACTIVAR SONIDOS")]
    [SerializeField] private AudioSource[] sonidos;


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;





    private void Awake()
    {
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
    }



    private void Update()
    {
        //---> Si esta activo el menu de Pausa no puede realizar nada en el update
        if(_controlDelTiempo.getEstaPausado())
        {
            PausarSonidos(true);
        }
        else
        {
            PausarSonidos(false);
        }
    }



    //---> Metodo para activar los sonidos del area
    private void ActivarSonidos(bool activar)
    {
        //---> Recorrer y activar cada sonido al estar el Mago en el rango
        for (int i = 0; i < sonidos.Length; i++)
        {
            if (activar)
            {
                sonidos[i].Play();
            }
            else
            {
                sonidos[i].Stop();
            }
        }
    }



    //---> Metodo para pausar los sonidos del area al pausar el portafolio
    private void PausarSonidos(bool pausa)
    {
        //---> Recorrer y activar cada sonido al estar el Mago en el rango
        for (int i = 0; i < sonidos.Length; i++)
        {
            if (pausa)
            {
                sonidos[i].Pause();
            }
            else
            {
                sonidos[i].UnPause();
            }
        }
    }



    //---> Metodo para activar las particulas del area
    private void ActivarParticulas(bool activar)
    {
        foreach (ParticleSystem particula in particulas)
        {
            if (activar)
            {
                particula.Play();
            }
            else 
            {
                particula.Stop();
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Activar las particulas
            ActivarSonidos(true);
            //---> Activar los sonidos de las particulas
            ActivarParticulas(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Desactivar las particulas
            ActivarSonidos(false);
            //---> Desactivar los sonidos de las particulas
            ActivarParticulas(false);
        }
    }
}
