using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramas : MonoBehaviour
{
    [SerializeField] private GameObject sonidoJugador;
    [SerializeField] private GameObject sonidoMonstruo;

    private bool puedePisarJ;
    private bool puedePisarM;

    private WaitForSeconds x;

    private void Start()
    {
        puedePisarJ = true;
        puedePisarM = true;   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (puedePisarJ)
            {
                puedePisarJ = false;
                SoundManager.Instance.SeleeccionAudio(7, 1f, "Entorno");
                Instantiate(sonidoJugador, other.gameObject.transform.position, Quaternion.identity);

                StartCoroutine(CooldownPisadaRamasJugador());
            }
        }

        if(other.gameObject.layer == 20) 
        {
            if (puedePisarM)
            {
                float distanciaJugador = GameManager.Instance.DistanciaJugador_Sonido(other.gameObject);

                puedePisarM = false;
                SoundManager.Instance.SeleeccionAudio(7, distanciaJugador * 3, "Entorno");
                Instantiate(sonidoMonstruo, other.gameObject.transform.position, Quaternion.identity);

                StartCoroutine(CooldownPisadaRamasMonstruo());
            }
        }
    }

    private IEnumerator CooldownPisadaRamasJugador()
    {
        x = new WaitForSeconds(20);
        yield return x;
        puedePisarJ = true;
    }
    private IEnumerator CooldownPisadaRamasMonstruo()
    {
        x = new WaitForSeconds(20);
        yield return x;
        puedePisarM = true;
    }
}
