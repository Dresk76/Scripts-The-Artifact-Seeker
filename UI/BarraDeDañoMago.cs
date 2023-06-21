using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeDañoMago : MonoBehaviour
{
    [Header("UI REFERENCIAS")]
    [SerializeField] private Slider barraDeDaño;





    public virtual void CambiarDañoMaxima(int vidaMaxima)
    {
        //---> El maximo valor del slider es la vida maxima del Mago
        barraDeDaño.maxValue = vidaMaxima;
    }



    public virtual void CambiarDañoActual(int cantidadVida)
    {
        //---> Cambiar el valor actual del slider al tomar vidas
        barraDeDaño.value = cantidadVida;
    }



    public void InicializarBarraDeDaño(int vidaMaxima, int cantidadVida)
    {
        //---> Inicializar la barra de vida
        CambiarDañoMaxima(vidaMaxima);
        CambiarDañoActual(cantidadVida);
    }
}
