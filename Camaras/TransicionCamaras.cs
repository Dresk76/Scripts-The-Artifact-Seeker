using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransicionCamaras : MonoBehaviour
{
    [Header("REFERENCIAS")]
    private Animator _animator;





    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }



    public void IniciarCambioCamara(bool iniciar)
    {
        if (iniciar)
        {
            _animator.SetTrigger("Iniciar");
        }
    }



    public void TerminarCambioCamara(bool terminar)
    {
        if (terminar)
        {
            _animator.SetTrigger("Terminar");
        }
    }

}
