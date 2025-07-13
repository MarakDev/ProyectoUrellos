using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameOverScreen : MonoBehaviour
{
    public void BotonRestart()
    {
        SoundManager.Instance.SeleeccionAudio(0, 1f, "Menu");

        GameManager.Instance.restartLevel = true;

        GameManager.Instance.NextScene(2);

        CanvasManager.Instance.GameOver.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void BotonSalirMenu()
    {
        SoundManager.Instance.SeleeccionAudio(0, 1f, "Menu");
        
        CanvasManager.Instance.GameOver.SetActive(false);

        GameManager.Instance.NextScene(0);

        CanvasManager.Instance.MenuPrincipal.SetActive(true);

    }

}
