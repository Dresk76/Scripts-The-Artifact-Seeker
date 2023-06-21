[System.Serializable] // Habilitar la serailizacion de datos de esta clase

public class DatosHastaCombate
{
    //---> DATOS A GUARDAR <---\\
    public float[] positionMago = new float[2];
    public int vidaActual;
    public float[] positionMagoOscuro = new float[2];
    public float contadorAutorretrado;
    public float contadorEscudoHabilidades;
    public float contadorLibroExperiencia;





    //---> Crear constructor
    public DatosHastaCombate(Mago mago, SaludMago saludMago, MagoOscuro magoOscuro, Dialogo dialogo)
    {
        positionMago[0] = mago.transform.position.x;
        positionMago[1] = mago.transform.position.y;
        vidaActual = saludMago.GetVidaActual();
        positionMagoOscuro[0] = magoOscuro.transform.position.x;
        positionMagoOscuro[1] = magoOscuro.transform.position.y;
        contadorAutorretrado = dialogo.GetContadorAutorretrado();
        contadorEscudoHabilidades = dialogo.GetContadorEscudoHabilidades();
        contadorLibroExperiencia = dialogo.GetContadorLibroExperiencia();
    }
}
