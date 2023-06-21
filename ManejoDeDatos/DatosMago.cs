
[System.Serializable] // Habilitar la serailizacion de datos de esta clase

public class DatosMago
{
    //---> DATOS A GUARDAR <---\\
    public float[] position = new float[2];





    //---> Crear constructor
    public DatosMago(Mago mago)
    {
        position[0] = mago.transform.position.x;
        position[1] = mago.transform.position.y;
    }
}
