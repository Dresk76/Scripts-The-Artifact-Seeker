using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVidaMago : MonoBehaviour
{
    [Header("UI REFERENCIAS")]
    [SerializeField] private Slider barraDeVida;





    public virtual void CambiarVidaMaxima(int vidaMaxima)
    {
        //---> El maximo valor del slider es la vida maxima del Mago
        barraDeVida.maxValue = vidaMaxima;
    }



    public virtual void CambiarVidaActual(int cantidadVida)
    {
        //---> Cambiar el valor actual del slider al tomar vidas
        barraDeVida.value = cantidadVida;
    }



    public void InicializarBarraDeVida(int vidaMaxima, int cantidadVida)
    {
        //---> Inicializar la barra de vida
        CambiarVidaMaxima(vidaMaxima);
        CambiarVidaActual(cantidadVida);
    }
}
