using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeSonido : MonoBehaviour
{
    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip sonidoDeArea;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    //---> Area de cofre en volumen 0.55f
    [Range(0, 1f)][SerializeField] private float volumensonidoDeArea = 0.18f;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
             //---> Desactivar el BoxCollider2D para que solo se reprodusca una vez
            this.GetComponent<BoxCollider2D>().enabled = false;

            //---> Sonido al entrar en el area
            _audioManager.PlayAreaSonido(sonidoDeArea, volumensonidoDeArea);
        }
    }
}
