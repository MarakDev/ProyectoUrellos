using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EscenaInicial : MonoBehaviour
{
    [SerializeField] private GameObject camaraCam;
    [SerializeField] private GameObject Jugador;
    [SerializeField] private GameObject libro;
    [SerializeField] private Canvas canvas;

    private WaitForSeconds timing;
    private bool flag;
    private bool flag2;
    private bool flag3;
    void Start()
    {
        camaraCam.SetActive(true);
        Jugador.SetActive(false);
        libro.SetActive(false);
        flag = false;
        flag2 = false;
        flag3 = false;

        StartCoroutine(TransicionLibro());
    }


    IEnumerator TransicionLibro()
    {
        timing = new WaitForSeconds(4);//4
        yield return timing;

        SoundManager.Instance.SeleeccionAudio(1, 1f, "Entorno"); //Se empieza a caer el libro
        timing = new WaitForSeconds(0.5f);
        yield return timing;

        libro.SetActive(true);

        timing = new WaitForSeconds(4);//4
        yield return timing;
        SoundManager.Instance.SeleeccionAudio(9, 1f, "Jugador"); //Se levanta de la cama el personaje

        timing = new WaitForSeconds(15);//15
        yield return timing;
        Jugador.SetActive(true);
        Jugador.GetComponent<PlayerMovement>().enabled = false;

        camaraCam.SetActive(false);

        //Tutorial Click izquierdo vision sonar
        timing = new WaitForSeconds(1);
        flag = true;

        CanvasManager.Instance.TextosTutorial[0].SetActive(true);
    }

    IEnumerator TextoVisionSonar()
    {
        CanvasManager.Instance.TextosTutorial[0].SetActive(false);
        Jugador.GetComponent<PlayerMovement>().enabled = true;
        yield return timing;
        CanvasManager.Instance.TextosTutorial[1].SetActive(true);
        flag2 = true;
    }

    IEnumerator TextoWASD()
    {
        CanvasManager.Instance.TextosTutorial[1].SetActive(false);
        timing = new WaitForSeconds(2);
        yield return timing;
        CanvasManager.Instance.TextosTutorial[2].SetActive(true);
    }

    IEnumerator TextoPiedra()
    {
        timing = new WaitForSeconds(2);
        yield return timing;
        CanvasManager.Instance.TextosTutorial[3].SetActive(true);
        timing = new WaitForSeconds(4);
        yield return timing;
        CanvasManager.Instance.TextosTutorial[3].SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (flag)
            {
                flag = false;
                StartCoroutine(TextoVisionSonar());
            }
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (flag2)
            {
                flag2 = false;
                StartCoroutine(TextoWASD());
                flag3 = true;
            }     
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CanvasManager.Instance.TextosTutorial[1].SetActive(false);
            CanvasManager.Instance.TextosTutorial[2].SetActive(false);
        }

        if(GameManager.Instance.PoderTirarPiedra())
        {
            if(flag3)
            {
                flag3 = false;
                StartCoroutine(TextoPiedra());
            }
            
        }
    }


}
