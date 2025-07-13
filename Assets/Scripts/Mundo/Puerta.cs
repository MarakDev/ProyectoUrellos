using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float direccion = 1f;
    [SerializeField] private float maxRotacion = 1f;

    private bool isOpen = false;

    public void Open()
    {
        if (!isOpen)
        {
            SoundManager.Instance.SeleeccionAudio(3, 1f, "Entorno");
            isOpen = true;
            StartCoroutine(RotationOpen());
        }
    }

    private IEnumerator RotationOpen()
    {
        float time = 0;
        while(time < maxRotacion)
        {
            transform.Rotate(0, 0, Time.deltaTime * speed * direccion);
            yield return null;
            time = 360 - transform.rotation.eulerAngles.y;
        }

        gameObject.GetComponent<ObjetoInteractuable>().enabled = false;
    }

}
