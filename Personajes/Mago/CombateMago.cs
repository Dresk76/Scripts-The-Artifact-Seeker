using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateMago : MonoBehaviour
{
    [Header("AREA DE ATAQUE")]
    [SerializeField] private Transform hitBox;
    [SerializeField] private float radioAtaque = 0.42f;
    [SerializeField] private int dañoAtaque = 1;
    [SerializeField] private bool dibujarCirculo; // Para dibujar el circulo de ataque por pantalla


    [Header("REBOTE POR GOLPE")]
    [SerializeField] private Vector2 velocidadRebote; // X = 1.5 Y = 4
    [SerializeField] private float fuerzaRebote = 1.5f;


    [Header("REFERENCIA CONTROL DE TIEMPO")]
    private ControlDelTiempo _controlDelTiempo;


    [Header("REFERENCIAS")]
    private SaludMagoOscuro _saludMagoOscuro;
    private Rigidbody2D _rb2D;





    private void Awake()
    {
        _controlDelTiempo = GameObject.Find("ControlDelTiempo").GetComponent<ControlDelTiempo>();
        _saludMagoOscuro = GameObject.FindGameObjectWithTag("MagoOscuro").GetComponent<SaludMagoOscuro>();
        _rb2D = GetComponent<Rigidbody2D>();
    }



    public void Ataque()
    {
        //---> Arreglo de objetos que se tocan cuando se ataca
        Collider2D [] objetos = Physics2D.OverlapCircleAll(hitBox.position, radioAtaque);

        foreach (Collider2D collision in objetos)
        {
            //---> Solo hacer el contacto con el HurtBox del Mago Oscuro
            if (collision.gameObject.layer == LayerMask.NameToLayer("Hurt"))
            {
                //---> Hasta que sea 1 ya que primero el Mago hace el golpe y despues de valida
                if (_saludMagoOscuro.GetVidaActual() == 1)
                {
                    StartCoroutine(AtaqueCamaraLenta());
                }

                //---> Obtener la matriz de contactos de la colisión
                ContactPoint2D[] contactos = new ContactPoint2D[1];
                collision.GetContacts(contactos);

                //---> Iterar sobre los contactos para obtener la información de cada contacto
                foreach (ContactPoint2D contacto in contactos)
                {
                    //---> Obtener punto de contacto y la normal
                    Vector2 puntoDeContacto = contactos[0].point;
                    Vector2 normal = contactos[0].normal;

                    //---> Mandar el daño al enemigo con el punto de contacto y la normal
                    _saludMagoOscuro.TomarDaño(dañoAtaque, puntoDeContacto, normal);
                }
            }
        }
    }



    //---> Disminuir la velocidad de la escena al realizar el ultimo ataque
    private IEnumerator AtaqueCamaraLenta()
    {
        _controlDelTiempo.InicioControlTiempo();
        yield return new WaitForSecondsRealtime(1f);
        _controlDelTiempo.ParoControlTiempo();
    }



    //---> Rebote del Mago al ser atacado
    public void ReboteGolpe(Vector2 puntoGolpe, Vector2 normal)
    {
        //---> Calcular la nueva velocidad
        Vector2 nuevaVelocidad = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);

        //---> Aplicar la fuerza de rebote
        _rb2D.velocity = nuevaVelocidad;

        //---> Aplicar la fuerza de rebote en la dirección normal al punto de contacto
        _rb2D.AddForce(normal * fuerzaRebote, ForceMode2D.Impulse);
    }



    private void OnDrawGizmos()
    {
        if (dibujarCirculo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position, radioAtaque);
        }
    }
}
