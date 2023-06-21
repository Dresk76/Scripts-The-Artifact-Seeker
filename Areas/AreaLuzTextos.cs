using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLuzTextos : MonoBehaviour
{
    [Header("LUZ")]
    [SerializeField] private GameObject luz;





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Activar la luz del objeto
            luz.gameObject.SetActive(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Desactivar la luz del objeto
            luz.gameObject.SetActive(false);
        }
    }
}
