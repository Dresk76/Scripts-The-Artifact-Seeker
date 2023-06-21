using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaEntreEscenas : MonoBehaviour
{
    public static LogicaEntreEscenas noDestruirEntreEscenas;





    private void Awake()
    {
        //---> Este código asegura que sólo hay un objeto en la escena que tiene el componente "LogicaEntreEscenas" y que el objeto persistirá entre diferentes escenas
        // //---> Si sólo hay un objeto con el componente "LogicaEntreEscenas", este código impide que el objeto sea destruido al cargar una nueva escena en Unity.
        // DontDestroyOnLoad(this.gameObject);
        if (LogicaEntreEscenas.noDestruirEntreEscenas == null)
        {
            LogicaEntreEscenas.noDestruirEntreEscenas = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
