using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//---> ENUMS PARA HACER PRUEBAS CON LOS CONTROLES DE PC Y MOBILE <---\\
public enum ControllerType
{
    PC,
    MOBILE
}


public class GameManager : MonoBehaviour
{
    [Header("REFERENCIAS")]
    public ControllerType controllerType;
    [SerializeField] private GameObject PcController;
    [SerializeField] private GameObject mobileController;


    [Header("REFERENCIAS MAGO")]
    private Mago _mago;





    private void Awake()
    {
        _mago = GameObject.FindGameObjectWithTag("Mago").GetComponent<Mago>();
    }



    private void Start()
    {
        ControllerSetup();
    }



    private void Update()
    {
        ControllerSetup();
    }



    public void ControllerSetup()
    {
        //---> Para que al momento de iniciar el portafolio detecte si es movil o pc
        if (controllerType == ControllerType.PC)
        {
            mobileController.SetActive(false);
            PcController.SetActive(true);
        }
        else if (controllerType == ControllerType.MOBILE)
        {
            PcController.SetActive(false);
            mobileController.SetActive(true);
        }

        //---> Para cada vez que se cambie el control aca se cambie en el script del Mago tambien
        //---> ACTIVAR AL MOMENTO DE HACER PRUEBAS PARA PC Y MOBILE
        // _mago.controllerType = controllerType;
    }
}
