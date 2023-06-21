using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class AreaDesactivarPasillo : MonoBehaviour
{
    [Header("CAMARAS")]
    [SerializeField] private CinemachineVirtualCamera camaraPasillo;
    [SerializeField] private CinemachineStateDrivenCamera camaraMago_1;


    [Header("REFERENCIA COLOR JOYSTICK")]
    private Color _colorAro;
    private Color _colorRelleno;


    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip fade;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenFade = 0.4f;


    [Header("REFERENCIAS DEL MAGO")]
    private Mago _mago;


    [Header("UI REFERENCIAS")]
    [SerializeField] private Image aroJoystick;
    [SerializeField] private Image rellenoJoystick;
    private TransicionCamaras _transicionCamaras;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
        _transicionCamaras = GameObject.Find("PanelTransicionCamaras").GetComponent<TransicionCamaras>();
    }



    private void Start()
    {
        //---> Desactivar al iniciar - CamaraMago1
        camaraMago_1.gameObject.SetActive(false);

        //---> Guardar el color del Joystick
        _colorAro = aroJoystick.color;
        _colorRelleno = rellenoJoystick.color;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.CompareTag("Mago"))
        {
            StartCoroutine(SalirDelPasillo());
        }
    }



    private IEnumerator SalirDelPasillo()
    {
        //---> Detener Musica del Intro
        _audioManager.StopMusic();

        //---> Retirar transparencia del Joystick
        aroJoystick.color = new Color(255f, 255f, 255f, 0f);
        rellenoJoystick.color = new Color(255f, 255f, 255f, 0f);

        //---> Reproducir sonido al realizar el fade
        _audioManager.PlaySFX(fade, volumenFade);

        //---> Hacer el Fade de salida de la TransicionCamaras
        _transicionCamaras.TerminarCambioCamara(true);

        //---> Tiempo para detener al Mago
        yield return new WaitForSecondsRealtime(0.3f);

        //---> Deshabilitar el movimiento del Mago
        _mago.puedeMoverse = false;
        _mago.velocidadObjetivo = Vector3.zero;
        _mago.rb2D.velocity = Vector2.zero;

        yield return new WaitForSecondsRealtime(0.5f);

        //---> Desactivar Camara del pasillo
        camaraPasillo.gameObject.SetActive(false);

        //---> Tiempo para que haga el cambio de camara de inmediato (cut) sin transicion
        yield return new WaitForSecondsRealtime(0.1f);

        //---> Restaurar transparencia del Joystick
        aroJoystick.color = _colorAro;
        rellenoJoystick.color = _colorRelleno;

        //---> Activar Camara Principal 1
        camaraMago_1.gameObject.SetActive(true);

        //---> Desactivar el BoxCollider2D para que solo ejecute una vez la corrutina SalirDelPasillo()
        this.GetComponent<BoxCollider2D>().enabled = false;

        //---> Habilitar el movimiento del Mago
        _mago.puedeMoverse = true;
    }
}
