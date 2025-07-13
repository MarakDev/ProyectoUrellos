using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerCerraPuertaCementerio : MonoBehaviour
{
    [SerializeField] private GameObject sonnarCaida;

    private WaitForSeconds x;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            StartCoroutine(ActivarMonstruo());

            SoundManager.Instance.SeleeccionAudio(5, 2f, "Entorno");
            
            GameObject.Find("puerta_Verja").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("puerta_Verja.002").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("puerta_Verja").GetComponent<MeshCollider>().enabled = false;
            GameObject.Find("puerta_Verja.002").GetComponent<MeshCollider>().enabled = false;

            GameObject.Find("puerta_Verja Cerrada").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("puerta_Verja.002 Cerrada").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("puerta_Verja Cerrada").GetComponent<MeshCollider>().enabled = true;
            GameObject.Find("puerta_Verja.002 Cerrada").GetComponent<MeshCollider>().enabled = true;

            gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }

    private IEnumerator ActivarMonstruo()
    {

        CanvasManager.Instance.TextosTutorial[6].SetActive(true);
        x = new WaitForSeconds(6);
        yield return x;

        CanvasManager.Instance.TextosTutorial[6].SetActive(false);

        x = new WaitForSeconds(4);
        yield return x;

        //Rugido bestia
        SoundManager.Instance.SeleeccionAudio(5, 0.4f, "Monstruo");
        GameObject.Find("Monstruo").GetComponent<NavMeshAgent>().enabled = true;
        GameObject.Find("Monstruo").GetComponent<MonstruoIA>().enabled = true;

        Destroy(gameObject);
    }
}
