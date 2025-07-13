using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class OpcionesScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nVolumen;
    [SerializeField] private TextMeshProUGUI nSens;
    public AudioMixer audioMixer;

    public void Start()
    {
        nVolumen.text = ("100%");
        nSens.text = ("10");
    }
    public void SetVolume (float volume)
    {
        nVolumen.text = ((volume * 100).ToString("F0") + "%");

        audioMixer.SetFloat("volume", Mathf.Log10 (volume) * 20);

        SoundManager.Instance.SeleeccionAudio(1, 1f, "Menu");
    }

    public void SetSensibilidad( float sens )
    {
        nSens.text = ((sens * 10).ToString("F0"));
        GameManager.Instance.sensibilidad = sens;
    }

    public void ReturnMenu()
    {

        SoundManager.Instance.SeleeccionAudio(0, 1f, "Menu");

        if (GameManager.Instance.sceneNum == 0)
        {
            
            CanvasManager.Instance.Opciones.SetActive(false);
            CanvasManager.Instance.MenuPrincipal.SetActive(true);
        }
        else
            GameManager.Instance.DesPausa();

        
    }
}
