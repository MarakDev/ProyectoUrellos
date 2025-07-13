using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerCaidaCementerio : MonoBehaviour
{
    [SerializeField] private GameObject sonnarCaida;
    [SerializeField] private GameObject sonnarMonstruo;

    private WaitForSeconds x;

    private void Start()
    {
        if (GameManager.Instance.restartLevel)
        {
            GameObject.Find("Jugador").transform.position = new Vector3(13, 15, -9);
            GameObject.Find("Jugador").GetComponent<PlayerMovement>().enabled = true;
            GameObject.Find("Jugador").GetComponent<PlayerAbilities>().enabled = true;
            GameObject.Find("Monstruo").GetComponent<NavMeshAgent>().enabled = true;
            GameObject.Find("Monstruo").GetComponent<MonstruoIA>().enabled = true;

            GameManager.Instance.RecuentoCalaveras();

            GameManager.Instance.restartLevel = false;
        }
        else
        {
            GameManager.Instance.CargaCalaveras();
            GameObject.Find("Jugador").GetComponent<PlayerMovement>().enabled = false;
            GameObject.Find("Jugador").GetComponent<PlayerAbilities>().enabled = false;
            GameObject.Find("Monstruo").GetComponent<NavMeshAgent>().enabled = false;
            GameObject.Find("Monstruo").GetComponent<MonstruoIA>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            SoundManager.Instance.SeleeccionAudio(11, 1f, "Jugador");
            Instantiate(sonnarCaida, transform.position, Quaternion.identity);

            StartCoroutine(EmpezarNivel());
        }
    }

    private IEnumerator EmpezarNivel()
    {
        x = new WaitForSeconds(3);
        yield return x;

        SoundManager.Instance.SeleeccionAudio(7, 3f, "Jugador"); //Audio del pj levantandose
        
        x = new WaitForSeconds(0.8f);
        yield return x;
        //Tutorial Jugador

        //Rugido bestia
        SoundManager.Instance.SeleeccionAudio(0, 0.2f, "Monstruo");
        Instantiate(sonnarMonstruo, transform.GetChild(0).transform.position, Quaternion.identity);

        CanvasManager.Instance.TextosTutorial[5].SetActive(true);

        x = new WaitForSeconds(8);
        yield return x;

        GameObject.Find("Jugador").GetComponent<PlayerMovement>().enabled = true;
        GameObject.Find("Jugador").GetComponent<PlayerAbilities>().enabled = true;

        CanvasManager.Instance.TextosTutorial[5].SetActive(false);

        Destroy(gameObject);

    }
}
