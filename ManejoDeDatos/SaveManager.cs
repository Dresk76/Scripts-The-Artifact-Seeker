using UnityEngine;
// Permite trabajar con archivos
using System.IO;
/* 
    Permite el uso del formateador binario, que es lo que permite guardar los datos a binario 
    para luego guardarlos en el archivo.
*/
using System.Runtime.Serialization.Formatters.Binary;


//---> Guardado de datos a un archivo y cargado de datos desde el archivo
// Clase estatica porque no va a poder ser instanciaada sino que va a consistir de algunos metodos para guardar y cargar datos que se van a llamar desde otros scripts.
public static class SaveManager
{
    //---> Metodo de guardado debe ser statico lo cual permite llamar desde otro script sin ningun tipo de instancia
    public static void GuardarDatosMago(Mago mago)
    {
        //---> Guardar los datos de DatosMago()
        DatosMago datosMago = new DatosMago(mago);

        //---> Ruta del archivo de guardado
        string dataPath = Application.persistentDataPath + "/mago.save";

        //---> Guardar los datos en el archivo
        /*
            + dataPath: La ruta donde se va a trabajar
            + FileMode: Donde se define lo que se quiere hacer con el archivo(Crear-Abrir-Agregar datos)
        */
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);

        //---> Antes de guardar algun dato se necesita un formateador binario que permite convertir los datos a binario y guardarlos en el archivo
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        // Indicar como parametros: cual es el fileStream y cuales son los datos que se van a convertir a binario y guardar
        binaryFormatter.Serialize(fileStream, datosMago);

        //---> Cerrar el archivo
        fileStream.Close();
    }



    //---> Carga de datos
    public static DatosMago CargarDatosMago()
    {
        //---> Ruta del archivo de guardado
        string dataPath = Application.persistentDataPath + "/mago.save";

        //---> Validar si existe el archivo guardado
        if (File.Exists(dataPath))
        {
            //---> Leer los datos del archivo
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);

            //---> Tomar los datos desde el archivo y pasarlos a variables
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            // Guardar en una variable DatosMago los datos obtenidos,
            // como al ser Deserialize no se sabe de que tipo son las variables se hace un casteo al tipo (DatosMago)
            DatosMago datosMago = (DatosMago) binaryFormatter.Deserialize(fileStream);

            //---> Cerrar el archivo
            fileStream.Close();

            //---> Devolver los datos a quien haya llamado el metodo
            return datosMago;
        }
        else
        {
            //---> Si el archivo guardado no existe
            Debug.LogError("No se econtró el archivo de guardado.");
            return null;
        }
    }
}
