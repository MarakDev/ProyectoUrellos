using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private Sprite animacion;

    private Image image;
    private Color c;

    private void Start()
    {
        SoundManager.Instance.SeleeccionAudio(3, 0.6f, "Menu");
    }

    public void Jugar()
    {
        //animacion del fondo y tal
        SoundManager.Instance.Stop();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(AnimacionEntrada());

    }

    IEnumerator AnimacionEntrada()
    {

        GameObject.Find("MenuPrincipal/Button").GetComponent<Image>().enabled = false;
        GameObject.Find("MenuPrincipal/Button1").GetComponent<Image>().enabled = false;
        GameObject.Find("MenuPrincipal/Button2").GetComponent<Image>().enabled = false;
        GameObject.Find("MenuPrincipal/Button3").GetComponent<Image>().enabled = false;
        GameObject.Find("MenuPrincipal/Titulo").GetComponent<TextMeshProUGUI>().enabled = false;

        SoundManager.Instance.SeleeccionAudio(2, 1f, "Menu");

        image = GetComponent<Image>();

        c = new Color(0.7f, 0.7f, 0.7f, 1f);

        image.color = c;
        image.sprite = animacion;

        yield return new WaitForSeconds(4);

        //Reproduce el video

        CanvasManager.Instance.Visualizador.SetActive(true);

        GameObject.Find("Cinematica").GetComponent<VideoPlayer>().Play();

        yield return new WaitForSeconds(58);

        CanvasManager.Instance.Visualizador.SetActive(false);

        GameManager.Instance.NextScene(1);

        GameObject.Find("MenuPrincipal/Button").GetComponent<Image>().enabled = true;
        GameObject.Find("MenuPrincipal/Button1").GetComponent<Image>().enabled = true;
        GameObject.Find("MenuPrincipal/Button2").GetComponent<Image>().enabled = true;
        GameObject.Find("MenuPrincipal/Button3").GetComponent<Image>().enabled = true;
        GameObject.Find("MenuPrincipal/Titulo").GetComponent<TextMeshProUGUI>().enabled = true;

        CanvasManager.Instance.MenuPrincipal.SetActive(false);

    }

    public void Opciones()
    {
        SoundManager.Instance.SeleeccionAudio(0, 1f, "Menu");
        CanvasManager.Instance.MenuPrincipal.SetActive(false);
        CanvasManager.Instance.Opciones.SetActive(true);

    }

    public void Creditos()
    {
        SoundManager.Instance.Stop();

        SoundManager.Instance.SeleeccionAudio(0, 1f, "Menu");
        CanvasManager.Instance.MenuPrincipal.SetActive(false);
        GameManager.Instance.NextScene(3);
    }

    public void Salir()
    {
        SoundManager.Instance.Stop();

        SoundManager.Instance.SeleeccionAudio(0, 1f, "Menu");
        Application.Quit();
    }

}
