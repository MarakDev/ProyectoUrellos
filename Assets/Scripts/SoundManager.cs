using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] audiosJugador;
    [SerializeField] private AudioClip[] audiosMonstruo;
    [SerializeField] private AudioClip[] audiosMenus;
    [SerializeField] private AudioClip[] audiosEntorno;

    private AudioSource controlAudio;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            controlAudio = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SeleeccionAudio(int indice, float volumen, string tipo)
    {
        switch (tipo)
        {
            case "Jugador":
                controlAudio.PlayOneShot(audiosJugador[indice], volumen);
                break;

            case "Monstruo":
                controlAudio.PlayOneShot(audiosMonstruo[indice], volumen);
                break;

            case "Menu":
                controlAudio.PlayOneShot(audiosMenus[indice], volumen);
                break;

            case "Entorno":
                controlAudio.PlayOneShot(audiosEntorno[indice], volumen);
                break;

        }
        
    }

    public void Stop()
    {
        controlAudio.loop = false;
        controlAudio.Stop();
    }
}
