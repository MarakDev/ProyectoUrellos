using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    [SerializeField] private float efectoDuracion;

    private Material[] materialesObjeto;
    private Material matBrillante;
    private Material matOscuro;

    private bool enAnimacion;

    private WaitForSeconds delay;

    private void Start()
    {
        materialesObjeto = this.gameObject.GetComponent<Renderer>().materials;
        matOscuro = materialesObjeto[0];
        matBrillante = materialesObjeto[1];
        enAnimacion = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15 && this.isActiveAndEnabled) //Layer EscanerInt
        {
            if(!enAnimacion)
            {
                enAnimacion = true;
                this.gameObject.GetComponent<Renderer>().material = materialesObjeto[1];
                StartCoroutine(animacionBrillo());
            }
        }

    }

    IEnumerator animacionBrillo()
    {

        float degradado = 0;
        while ( degradado < 1.2f)
        {
            materialesObjeto[1].SetColor("_EmissionColor", new Color(0, degradado/1.5f, degradado));
            yield return null;

            degradado += Time.deltaTime;
        }

        delay = new WaitForSeconds(efectoDuracion);
        yield return delay;

        while (degradado > 0)
        {
            materialesObjeto[1].SetColor("_EmissionColor", new Color(0, degradado/1.5f, degradado));

            yield return null;

            degradado -= Time.deltaTime;

        }

        materialesObjeto[0] = matOscuro;
        materialesObjeto[1] = matBrillante;
        this.gameObject.GetComponent<Renderer>().materials = materialesObjeto;

        enAnimacion = false;

    }

}
