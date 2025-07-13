using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calavera : MonoBehaviour
{
    public bool estaRecogida;
    public void RecogerCalavera()
    {
        //Suena recoger calavera        
        SoundManager.Instance.SeleeccionAudio(1, 1f, "Menu");
        estaRecogida = true;

        GameManager.Instance.CuentaColleccionables();

        CanvasManager.Instance.ActualizaColecionables();

        this.gameObject.SetActive(false);
    }

    
}
