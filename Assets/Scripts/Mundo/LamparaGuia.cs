using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamparaGuia : MonoBehaviour
{
    [SerializeField] private Light luz;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject ondas;

    private float degradado = 0;
    private bool unaVez = true;
    private void Update()
    {

        if (hitbox.GetComponent<DetectarHitbox>().EnteredTrigger)
        {
            if (unaVez)
            {
                float distancia = GameManager.Instance.DistanciaJugador_Sonido(this.gameObject);
                unaVez=false;
                SoundManager.Instance.SeleeccionAudio(8, distancia * 0.5f, "Entorno");
                Instantiate(ondas, transform.position, Quaternion.identity);
            }

            if(degradado < 5)
            {
                degradado += Time.deltaTime;

                luz.range = degradado;
            }
        }
               
    }
}
