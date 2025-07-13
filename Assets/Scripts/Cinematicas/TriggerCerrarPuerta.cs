using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCerrarPuerta : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            GameObject aux = GameObject.Find("puerta2");
            aux.transform.Rotate(0, 0, 100);
            SoundManager.Instance.SeleeccionAudio(4, 6f, "Entorno");

            GameManager.Instance.pPiedra = true;

            Destroy(this);
        }
    }
}
