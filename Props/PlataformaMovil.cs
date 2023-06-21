using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("PLATAFORMA")]
    [SerializeField] private Transform[] puntosMoviento; // Puntos por los cuales se va a mover la plataforma
    [SerializeField] private float velocidadMovimento = 1f;
    private int siguientePlataforma = 1; // Siguiente punto al que se va a mover
    private bool ordenPlataformas = true; // Orden en el que se mueve segun los puntos


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("TIEMPO DE ESPERA DE LA PLATAFORMA")]
    [SerializeField] private float tiempoEspera = 1f; // Tiempo de espera en segundos
    [SerializeField] private float contadorT; // Tiempo transcurrido desde el último movimiento


    [Header("TRANSPARENCIA DASH")]
    [SerializeField] private GameObject transparenciaDash;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    private void Start()
    {
        //---> Iniciar el valor del contador
        contadorT = tiempoEspera;
    }



    private void Update()
    {
        //---> Comenzar a contar en reversa
        contadorT -= Time.deltaTime;

        //---> Si el orden de las plataforma es verdadero (que se va en orden ascendente) y
        // la siguiente plataforma + 1 es mayor que la cantidad de puntos invierta el orden de movimiento
        if (ordenPlataformas && siguientePlataforma + 1 >= puntosMoviento.Length)
        {
            ordenPlataformas = false;
        }

        //---> Condicion contraria a la anterior
        if (!ordenPlataformas && siguientePlataforma <= 0)
        {
            ordenPlataformas = true;
        }

        //---> Validar si la plataforma esta lo suficiente cerca del siguiente punto para cambiar la direccion 
        if (Vector2.Distance(transform.position, puntosMoviento[siguientePlataforma].position) < 0.1f) // Posicion de la plataforma y posicion del punto al que se esta moviendo
        {
            if (ordenPlataformas)
            {
                //---> Cambiar de direccion
                siguientePlataforma += 1;
            }
            else
            {
                //---> Cambiar de direccion
                siguientePlataforma -= 1;
            }

            //---> Actualizar el contador al cambiar de direccion
            contadorT = tiempoEspera;
        }


        //---> Mover la plataforma a la posicion deseada con MoveTowards
        /*
            1. Posicion de la plataforma 
            2. Posicion a donde se desea mover
            3. velocidad a la que se va a mover
        */
        if (contadorT <= 0)
        {
            //---> Mover la plataforma
            transform.position = Vector2.MoveTowards(transform.position, puntosMoviento[siguientePlataforma].position, velocidadMovimento * Time.deltaTime);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mago"))
        {
            //---> Tomar el transform de la plataforma y pasarselo al mago como objeto hijo
            collision.transform.SetParent(this.transform);

            //---> Restringir el ataque del Mago
            _mago.estaEnPlataforma = true;

            //---> Quitar la transparencia del Dash
            transparenciaDash.SetActive(true);
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        //---> Para que deje de ser hijo de la plataforma y se mueva por si mismo
        if (collision.gameObject.CompareTag("Mago"))
        {
            collision.transform.SetParent(null);

            //---> Habilitar el ataque del Mago
            _mago.estaEnPlataforma = false;

            //---> Quitar la transpare del Dash
            transparenciaDash.SetActive(false);
        }
    }
}
