using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjetoCaidaLibro : MonoBehaviour
{
    [SerializeField] GameObject ondaPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {

            SoundManager.Instance.SeleeccionAudio(2, 1f, "Entorno");
            Instantiate(ondaPrefab, transform.position, Quaternion.identity);
        }
    }
}
