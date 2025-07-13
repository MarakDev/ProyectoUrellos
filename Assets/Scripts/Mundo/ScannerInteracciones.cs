using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerInteracciones : MonoBehaviour
{

    public Transform esfera;
    public float rango;
    public float speed;
    public float fade;
    public Vector3 valorInicial;
    public Vector3 direccionEscalar;

    private Material material;

    void Start()
    {
        material = esfera.GetComponent<Renderer>().material;
        esfera.localScale = valorInicial;
        material.SetFloat("_Opacidad", 1);
    }

    // Update is called once per frame
    void Update()
    {
        OpacidadOnda();
        AumentaRangoOnda();

        if (material.GetFloat("_Opacidad") <= 0)
            Destroy(gameObject);
    }

    private void AumentaRangoOnda()
    {
        if (esfera.localScale.magnitude < rango )
        {
            esfera.transform.localScale += direccionEscalar * speed * Time.deltaTime;
            speed -= speed * Time.deltaTime;
        }

    }

    private void OpacidadOnda()
    {
        if (material.GetFloat("_Opacidad") > 0)
        {
            float fadeAmount = material.GetFloat("_Opacidad") - (fade / 10 * Time.deltaTime);

            material.SetFloat("_Opacidad", fadeAmount);
        }
    }
}
