using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------- AUDIO SOURCE -----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXsource;
    [SerializeField] private AudioSource menuPausaSource;
    [SerializeField] private AudioSource magoSource;
    [SerializeField] private AudioSource magoOscuroSource;
    [SerializeField] private AudioSource fuegoSource;
    [SerializeField] private AudioSource puertaSource;
    [SerializeField] private AudioSource areaSonidoSource;
    [SerializeField] private AudioSource boomSource;
    [SerializeField] private AudioSource portalSource;
    [SerializeField] private AudioSource dialogoSource;
    [SerializeField] private AudioSource latidoSource;
    [SerializeField] private AudioSource enterUISource;
    [SerializeField] private AudioSource pestanasUISource;
    [SerializeField] private AudioSource musicMenuInicialUISource;
    [SerializeField] private AudioSource musicMenuSonidoUISource;
    [SerializeField] private AudioSource SFXmenuSonidoUISource;


    [Header("CURVA DE ANIMACION")]
    [SerializeField] private AnimationCurve animationCurve;





    //---> Metodo para reproducir los sonidos por (musicSource)
    public void PlayMusic(AudioClip clip, float volume, bool loop)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = loop;

        musicSource.Play();
    }



    //---> Metodo para pausar la musica al estar en el menu de Pausa
    public void PauseMusic(bool _estaPausado)
    {
        if (_estaPausado)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();
        }
    }



    //---> Metodo para detener los sonidos de (musicSource)
    public void StopMusic()
    {
        musicSource.Stop();
    }



    //---> Metodo para detener la Musica Gradualmente
    public IEnumerator FadeOutMusic(float volumenInicial, float volumenObjetivo, float lerpDuration)
    {
        //---> Guardar el volumen inicial para no pasarlo como parametro
        // float volumenInicial = musicSource.volume;

        //---> Llevar la cuenta del tiempo transcurrido contando desde 0
        float tiempoTranscurrido = 0f;

        //---> Mientras el tiempo transcurrido sea menor que la duracion de la transicion
        while(tiempoTranscurrido < lerpDuration)
        {
            /*
                A medida que transcurre el tiempo la división va a pasar de 0 a 1 haciendo que la
                interpolacion del valor inicial al valor final transcurra en (lerpDuration) segundos
                con un Curva de animacion
            */
            musicSource.volume = Mathf.Lerp(volumenInicial, volumenObjetivo, animationCurve.Evaluate(tiempoTranscurrido / lerpDuration));

            //---> Ir sumando a la variable (tiempoTranscurrido) el tiempo de cada frame
            tiempoTranscurrido += Time.deltaTime;

            //---> Detener la ejecución de la Corrutina hasta el proximo frame
            yield return null;
        }

        //---> Para que al llegar al final se detenga la Musica
        // musicSource.Stop();
        musicSource.volume = volumenObjetivo;
    }



    //---> Metodo para reproducir los sonidos por (SFXsource)
    public void PlaySFX(AudioClip clip, float volume)
    {
        SFXsource.clip = clip;
        SFXsource.volume = volume;

        SFXsource.Play();
    }



    //---> Metodo para pausar los efectos al estar en el menu de Pausa
    public void PauseSFX(bool _estaPausado)
    {
        if (_estaPausado)
        {
            SFXsource.Pause();
        }
        else
        {
            SFXsource.UnPause();
        }
    }



    //---> Metodo para detener los sonidos de (SFXsource)
    public void StopSFX()
    {
        SFXsource.Stop();
    }



    //---> Metodo para reproducir los sonidos por (menuPausaSource)
    public void PlayMenuPausa(AudioClip clip, float volume)
    {
        menuPausaSource.clip = clip;
        menuPausaSource.volume = volume;

        menuPausaSource.Play();
    }



    //---> Metodo para reproducir los sonidos por (SFXsource)
    public void PlayMago(AudioClip clip, float volume)
    {
        magoSource.clip = clip;
        magoSource.volume = volume;

        magoSource.Play();
    }



    //---> Metodo para pausar los efectos al estar en el menu de Pausa
    public void PauseMago(bool _estaPausado)
    {
        if (_estaPausado)
        {
            magoSource.Pause();
        }
        else
        {
            magoSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (magoOscuroSource)
    public void PlayMagoOscuro(AudioClip clip, float volume)
    {
        magoOscuroSource.clip = clip;
        magoOscuroSource.volume = volume;

        magoOscuroSource.Play();
    }



    //---> Metodo para pausar los efectos al estar en el menu de Pausa
    public void PauseMagoOscuro(bool _estaPausado)
    {
        if (_estaPausado)
        {
            magoOscuroSource.Pause();
        }
        else
        {
            magoOscuroSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (fuegoSource)
    public void PlayFuego(AudioClip clip, float volume)
    {
        fuegoSource.clip = clip;
        fuegoSource.volume = volume;

        fuegoSource.Play();
    }



    //---> Metodo para pausar los efectos del fuego al estar en el menu de Pausa
    public void PauseFuego(bool _estaPausado)
    {
        if (_estaPausado)
        {
            fuegoSource.Pause();
        }
        else
        {
            fuegoSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (puertaSource)
    public void PlayPuerta(AudioClip clip, float volume)
    {
        puertaSource.clip = clip;
        puertaSource.volume = volume;

        puertaSource.Play();
    }



    //---> Metodo para pausar los efectos de las Puertas al estar en el menu de Pausa
    public void PausePuerta(bool _estaPausado)
    {
        if (_estaPausado)
        {
            puertaSource.Pause();
        }
        else
        {
            puertaSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (areaSonidoSource)
    public void PlayAreaSonido(AudioClip clip, float volume)
    {
        areaSonidoSource.clip = clip;
        areaSonidoSource.volume = volume;

        areaSonidoSource.Play();
    }



    //---> Metodo para pausar los efectos de las Areas de Soniso al estar en el menu de Pausa
    public void PauseAreaSonido(bool _estaPausado)
    {
        if (_estaPausado)
        {
            areaSonidoSource.Pause();
        }
        else
        {
            areaSonidoSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (boomSource)
    public void PlayBoom(AudioClip clip, float volume)
    {
        boomSource.clip = clip;
        boomSource.volume = volume;

        boomSource.Play();
    }



    //---> Metodo para pausar el efecto del Boom al estar en el menu de Pausa
    public void PauseBoom(bool _estaPausado)
    {
        if (_estaPausado)
        {
            boomSource.Pause();
        }
        else
        {
            boomSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (portalSource)
    public void PlayPortal(AudioClip clip, float volume)
    {
        portalSource.clip = clip;
        portalSource.volume = volume;

        portalSource.Play();
    }



    //---> Metodo para pausar el efecto del portal al estar en el menu de Pausa
    public void PausePortal(bool _estaPausado)
    {
        if (_estaPausado)
        {
            portalSource.Pause();
        }
        else
        {
            portalSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (dialogoSource)
    public void PlayDialogo(AudioClip clip, float volume)
    {
        dialogoSource.clip = clip;
        dialogoSource.volume = volume;

        dialogoSource.Play();
    }



    //---> Metodo para pausar los efectos de las Puertas al estar en el menu de Pausa
    public void PauseDialogo(bool _estaPausado)
    {
        if (_estaPausado)
        {
            dialogoSource.Pause();
        }
        else
        {
            dialogoSource.UnPause();
        }
    }



    //---> Metodo para reproducir los sonidos por (latidoSource)
    public void PlayLatido(AudioClip clip, float volume, bool loop)
    {
        latidoSource.clip = clip;
        latidoSource.volume = volume;
        latidoSource.loop = loop;

        latidoSource.Play();
    }



    //---> Metodo para detener los sonidos de (latidoSource)
    public void StopLatido()
    {
        latidoSource.Stop();
    }



    //---> Metodo para reproducir los sonidos por (enterUISource)
    public void PlayEnterUI(AudioClip clip, float volume)
    {
        enterUISource.clip = clip;
        enterUISource.volume = volume;

        enterUISource.Play();
    }



    //---> Metodo para reproducir los sonidos por (pestanasUISource)
    public void PlayPestanasUI(AudioClip clip, float volume)
    {
        pestanasUISource.clip = clip;
        pestanasUISource.volume = volume;

        pestanasUISource.Play();
    }



    //---> Metodo para reproducir los sonidos por (musicMenuSonidoUISource)
    public void PlayMusicMenuSonidoUI(AudioClip clip, float volume, bool loop)
    {
        musicMenuSonidoUISource.clip = clip;
        musicMenuSonidoUISource.volume = volume;
        musicMenuSonidoUISource.loop = loop;

        musicMenuSonidoUISource.Play();
    }



    //---> Metodo para detener los sonidos de (musicMenuSonidoUISource)
    public void StopMusicMenuSonidoUI()
    {
        musicMenuSonidoUISource.Stop();
    }



    //---> Metodo para reproducir los efectos del Menu de sonido por (SFXmenuSonidoUISource)
    public void PlaySFXMenuSonidoUI(AudioClip clip, float volume, bool loop)
    {
        SFXmenuSonidoUISource.clip = clip;
        SFXmenuSonidoUISource.volume = volume;
        SFXmenuSonidoUISource.loop = loop;

        SFXmenuSonidoUISource.Play();
    }



    //---> Metodo para detener los efectos de (SFXmenuSonidoUISource)
    public void StopSFXMenuSonidoUI()
    {
        SFXmenuSonidoUISource.Stop();
    }



    //---> Metodo para reproducir los sonidos por (musicMenuInicialUISource)
    public void PlayMusicMenuInicialUI(AudioClip clip, float volume, bool loop)
    {
        musicMenuInicialUISource.clip = clip;
        musicMenuInicialUISource.volume = volume;
        musicMenuInicialUISource.loop = loop;

        musicMenuInicialUISource.Play();
    }



    //---> Metodo para pausar la musica del Menu Inicial
    public void ControlMusicMenuInicialUI(bool _estaPausado, float volume)
    {
        if (_estaPausado)
        {
            musicMenuInicialUISource.volume = 0f;
        }
        else
        {
            musicMenuInicialUISource.volume = volume;
        }
    }



    //---> Metodo para detener los sonidos de (musicMenuInicialUISource)
    public void StopMusicMenuInicialUI()
    {
        musicMenuInicialUISource.Stop();
    }
}
