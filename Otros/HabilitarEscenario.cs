using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HabilitarEscenario : MonoBehaviour
{
    //---> Script para que una vez leida por primera vez el Cartel del Autorretrato se habilite
         //el segundo piso


    [Header("CONTADOR CARTELES")]
    // Declarar el diccionario con clave String y valor int
    public Dictionary<string, int> carteles = new Dictionary<string, int>();


    [Header("SUELO PRIMER PISO")]
    [SerializeField] private GameObject suelo_2;


    [Header("PARED PRIMER PISO")]
    [SerializeField] private GameObject segundaPared;


    [Header("COLLIDER")]
    [SerializeField] private GameObject colliderLibro;


    [Header("CARTEL FELCHA")]
    [SerializeField] private GameObject cartelFlechaTres;


    [Header("ESCALERAS")]
    [SerializeField] private GameObject escalera1;
    [SerializeField] private GameObject escalera2;





    private void Start()
    {
        //---> Desactivar al iniciar
        /*
            - Escalera 1
            - Escalera 2
            - Primer piso segunda pared
        */
        escalera1.SetActive(false);
        escalera2.SetActive(false);
        segundaPared.SetActive(false);


        //---> Añadir a la lista
        carteles.Add("Autorretrato", 0);
        carteles.Add("EscudoHabilidades", 0);
        carteles.Add("LibroExperiencia", 0);
    }



    private void Update()
    {
        //---> Invocar el Contador a cada frame
        Contador();
    }
    


    public void Contador()
    {
        
        foreach (KeyValuePair<string, int> cartel in carteles)
        {
            //---> Al recibir que se abrio por primera vez el Autorretrato
            if (cartel.Key == "Autorretrato" && cartel.Value == 1)
            {
                //---> Desactivar el suelo_2 para habilitar el acceso a las escaleras
                suelo_2.gameObject.SetActive(false);

                //---> Activar el Area de la Segunda pared del primer piso
                segundaPared.SetActive(true);

                //---> Para que solo active la escalera_1 al estar siempre desactivada la escalera_2
                if (!escalera2.gameObject.activeSelf)
                {
                    escalera1.gameObject.SetActive(true);
                }
            }

            //---> Al recibir que se abrio por primera vez el Libro Experiencia
            if (cartel.Key == "LibroExperiencia" && cartel.Value == 1)
            {
                //---> Activar el cartelFlechaTres
                cartelFlechaTres.SetActive(true);

                //---> Desactivar el collider del Area del libro Experiencia
                colliderLibro.SetActive(false);
            }
        }
    }
}
