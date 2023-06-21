using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarEscaleraAbajo : MonoBehaviour
{
    [Header("VARIABLES PUBLICAS")]
    [HideInInspector] public bool magoEnRango;





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SueloMago"))
        {
            magoEnRango = true;
        }
    }
}
