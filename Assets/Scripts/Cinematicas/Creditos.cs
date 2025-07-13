using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Creditos : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(CreditosAnimacion());
    }


    private IEnumerator CreditosAnimacion()
    {
        
        yield return new WaitForSeconds(2);

        CanvasManager.Instance.Visualizador.SetActive(true);
        GameObject.Find("Cinematica").GetComponent<VideoPlayer>().Play();

        yield return new WaitForSeconds(80);

        CanvasManager.Instance.Visualizador.SetActive(false);
        CanvasManager.Instance.MenuPrincipal.SetActive(true);

        GameManager.Instance.NextScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
