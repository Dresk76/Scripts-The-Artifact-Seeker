using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaIntro : MonoBehaviour
{
    [Header("REFERENCIAS AUDIO")]
    [SerializeField] private AudioClip musicIntro;
    private AudioManager _audioManager;


    [Header("VOLUMEN AUDIO")]
    [Range(0, 1f)][SerializeField] private float volumenMusicIntro = 0.3f;





    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }



    //---> Al Mago entrar estar en el Box Collider 2D reproducir la musica del Intro y desactivarse el Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mago"))
        {
            //---> Reproducir la Musica del Intro
            _audioManager.PlayMusic(musicIntro, volumenMusicIntro, true);

            //---> Desactivar el BoxCollider2D
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
