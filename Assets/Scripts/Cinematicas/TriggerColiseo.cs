using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColiseo : MonoBehaviour
{

    [SerializeField] private Light luz;
    [SerializeField] private Light luz2;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject hitbox2;
    [SerializeField] private GameObject hitbox3;
    [SerializeField] private GameObject hitbox4;
    [SerializeField] private GameObject ondas;

    private float degradado = 0;
    private bool unaVez = true;
    private void Update()
    {

        if (hitbox.GetComponent<DetectarHitJugador>().EnteredTrigger || hitbox2.GetComponent<DetectarHitJugador>().EnteredTrigger ||
            hitbox3.GetComponent<DetectarHitJugador>().EnteredTrigger || hitbox4.GetComponent<DetectarHitJugador>().EnteredTrigger)
        {
            if (unaVez)
            {
                float distancia = GameManager.Instance.DistanciaJugador_Sonido(this.gameObject);

                unaVez = false;
                SoundManager.Instance.SeleeccionAudio(10, distancia * 0.75f, "Entorno");
                Instantiate(ondas, luz.transform.position, Quaternion.identity);
            }

            if (degradado < 8)
            {
                degradado += Time.deltaTime;

                luz.range = degradado;
                luz2.range = degradado;
            }
        }

    }

}
