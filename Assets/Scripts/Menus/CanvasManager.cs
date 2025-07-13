using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private WaitForSeconds delay = new WaitForSeconds(3);

    public static CanvasManager Instance;

    public GameObject MenuPrincipal;
    public GameObject Opciones;
    public GameObject[] TextosTutorial;
    public GameObject CalaveraTXT;
    public GameObject GameOver;
    public GameObject Visualizador;

    private void Awake()
    {
        if (Instance == null)
        {
            //Crea la instancia
            Instance = this;

            //Cargar todos los elementos en el CanvasManager
            MenuPrincipal = GameObject.Find("MenuPrincipal");
            Opciones = GameObject.Find("Opciones");

            TextosTutorial = new GameObject[7];

            TextosTutorial[0] = GameObject.Find("PanelSonnar");
            TextosTutorial[1] = GameObject.Find("PanelWASD");
            TextosTutorial[2] = GameObject.Find("PanelE");
            TextosTutorial[3] = GameObject.Find("PanelPiedra");
            TextosTutorial[4] = GameObject.Find("PanelAgacharse");
            TextosTutorial[5] = GameObject.Find("ConsejoMonstruo");
            TextosTutorial[6] = GameObject.Find("ConsejoFarolillos");

            CalaveraTXT = GameObject.Find("Calaveras");
            GameOver = GameObject.Find("GameOver");

            Visualizador = GameObject.Find("Visualizador");

            //Desactivar todo

            //MenuPrincipal.SetActive(false);
            Opciones.SetActive(false);

            foreach (GameObject tutorial in TextosTutorial)
            {
                tutorial.SetActive(false);
            }

            CalaveraTXT.SetActive(false);
            GameOver.SetActive(false);

            Visualizador.SetActive(false);

            //Aseguramos no romperlo entre escenas
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void Start()
    {
        Opciones.SetActive(false);
    }

    public void ActualizaColecionables()
    {
        StartCoroutine(TextoAviso());

        CalaveraTXT.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.calaveras + " / 8 \n Calaveras Coleccionables";

    }

    private IEnumerator TextoAviso()
    {
        CalaveraTXT.SetActive(true);
        yield return delay;
        CalaveraTXT.SetActive(false);

    }
}
