using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuervos : MonoBehaviour
{

    [SerializeField] private GameObject[] cuervos;
    [SerializeField] private GameObject ondaCuervo;

    private float distanciaJugador;
    private float cooldown;
    private float cooldownRango = 15;

    private void Update()
    {
        cooldown += Time.deltaTime;

        if (cooldown > cooldownRango)
        {
            //Cuervos primera zona
            int random1 = Random.Range(0, 3);

            distanciaJugador = GameManager.Instance.DistanciaJugador_Sonido(cuervos[random1]);

            if (distanciaJugador > 0)//Distanci8a audible
            {
                SoundManager.Instance.SeleeccionAudio(9, distanciaJugador, "Entorno");
                Instantiate(ondaCuervo, cuervos[random1].transform.position, Quaternion.identity);
            }

            //Cuervos segunda zona
            int random2 = Random.Range(3, 6);

            distanciaJugador = GameManager.Instance.DistanciaJugador_Sonido(cuervos[random2]);

            if (distanciaJugador > 0)//Distanci8a audible
            {
                SoundManager.Instance.SeleeccionAudio(9, distanciaJugador, "Entorno");
                Instantiate(ondaCuervo, cuervos[random2].transform.position, Quaternion.identity);
            }

            //Cuervos tercera zona
            int random3 = Random.Range(6, 9);

            distanciaJugador = GameManager.Instance.DistanciaJugador_Sonido(cuervos[random3]);

            if (distanciaJugador > 0)//Distanci8a audible
            {
                SoundManager.Instance.SeleeccionAudio(9, distanciaJugador, "Entorno");
                Instantiate(ondaCuervo, cuervos[random3].transform.position, Quaternion.identity);
            }

            cooldownRango = Random.Range(15, 25);

            cooldown = 0;
        }
        
    }


}
