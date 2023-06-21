using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDeEscaleras : MonoBehaviour
{
    [Header("ESCALERAS")]
    [SerializeField] private GameObject _escalera1;
    [SerializeField] private GameObject _escalera2;


    [Header("REFERENCIAS DEL MAGO")]
    private SpriteRenderer _spriteRendererMago;
    private TrailRenderer _trailMago;





    private void Awake()
    {
        _spriteRendererMago = GameObject.FindGameObjectWithTag("Mago").GetComponent<SpriteRenderer>();
        _trailMago = GameObject.Find("TrailMago").GetComponent<TrailRenderer>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---> Cambiar Layer del mago
        _spriteRendererMago.sortingOrder = 11;
        _trailMago.sortingOrder = 10;

        if (collision.CompareTag("Mago"))
        {
            _escalera1.gameObject.SetActive(false);
            _escalera2.gameObject.SetActive(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        //---> Cambiar Layer del mago
        _spriteRendererMago.sortingOrder = 11;
        _trailMago.sortingOrder = 10;

        if (collision.CompareTag("Mago"))
        {
            _escalera2.gameObject.SetActive(false);
            _escalera1.gameObject.SetActive(true);
        }
    }
}
