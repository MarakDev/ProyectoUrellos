using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenaTutorialEscenaCementerio : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            SoundManager.Instance.SeleeccionAudio(10, 1f, "Jugador");

            GameManager.Instance.NextScene(2);

        }
    }
}
