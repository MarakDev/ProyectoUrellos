using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool pPiedra;
    public int calaveras;
    private int[][] calaverasObjeto;

    public int sceneNum;
    public bool restartLevel;

    public int zonaMonstruo;

    public float sensibilidad;

    private bool estadoPausa;

    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
            pPiedra = false;
            sceneNum = 0;
            sensibilidad = 1;
            calaveras = 0;

            zonaMonstruo = 0;

            estadoPausa = false;
            restartLevel = false;
            DontDestroyOnLoad(this);

        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(sceneNum != 0 && sceneNum != 3)
        {
            Pausa();
        }
        
    }

    public void NextScene(int n)
    {
        sceneNum = n;

        SceneManager.LoadScene(sceneNum);
    }

    public bool PoderTirarPiedra()
    {
        if (pPiedra)
            return true;
        else
            return false;
    }

    public float DistanciaJugador_Sonido(GameObject sonido)
    {
        Vector3 distanciaJugador = GameObject.Find("Jugador").transform.position;

        float distancia = (Vector3.Distance(sonido.transform.position, distanciaJugador));

        //distancia audible (mas alla de 25 unidades no se ven ondas, pero si se puede escuchar)
        distancia = (25 - distancia) / 25;

        if(distancia < 0)
            distancia = 0;

        return distancia;
    }

    public void CargaCalaveras()
    {
        calaverasObjeto = new int[7][];

        for (int i = 0; i < 7; i++)
        {
            calaverasObjeto[i] = new int[2];

            calaverasObjeto[i][0] = i; //numero de la calavera

            calaverasObjeto[i][1] = 0; //no esta recogida, 0 no esta, 1 esta recogida
        }
    }
    public void CuentaColleccionables()
    { 
        if(sceneNum == 2)
        {
            for (int i = 0; i < 7; i++)
            {
                if (GameObject.Find("Coleccionables").transform.GetChild(i).gameObject.GetComponent<Calavera>().estaRecogida)
                {
                    calaverasObjeto[i][1] = 1;
                }
            }
        }
            
        calaveras++;
    }

    public void RecuentoCalaveras()
    {
        for(int i = 0; i < 7; i++)
        {
            if (calaverasObjeto[i][1] == 1)
            {
                GameObject.Find("Coleccionables").transform.GetChild(i).gameObject.SetActive(false);
            }
                
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameObject.Find("Jugador").SetActive(false);
        GameObject.Find("Monstruo").GetComponent<MonstruoIA>().enabled = false;
        GameObject.Find("CamHolder").GetComponent<MoveCamera>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CanvasManager.Instance.GameOver.SetActive(true);

    }

    public void Pausa()
    {

        if (Input.GetKeyUp(KeyCode.Escape) && !estadoPausa)
        {
            estadoPausa = true;
            Time.timeScale = 0;

            SoundManager.Instance.Stop();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CanvasManager.Instance.Opciones.SetActive(true);
        }

    }

    public void DesPausa()
    {
        if (estadoPausa)
        {
            estadoPausa = false;
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            CanvasManager.Instance.Opciones.SetActive(false);
        }
    }



}
