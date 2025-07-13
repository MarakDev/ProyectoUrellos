using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerPlayer : MonoBehaviour
{

    public Transform esfera;
    public float rango;
    public float speed;
    private float speedInicial;
    public float empiezaFade;
    public float fade;

    private bool sonidoFlag;
    private Material material;

    void Start()
    {
        sonidoFlag = true;
        material = esfera.GetComponent<Renderer>().material;
        speedInicial = speed;
        esfera.localScale = Vector3.zero;
        material.SetFloat("_Opacidad", 1);
    }

    private void FixedUpdate()
    {
        
        if (gameObject.activeSelf == true && sonidoFlag)
        {
            sonidoFlag = false;
            SoundManager.Instance.SeleeccionAudio(0, 0.8f, "Jugador");
        }

        OpacidadOnda();

        AumentaRangoOnda();

        if (material.GetFloat("_Opacidad") <= 0)
        {

            speed = speedInicial;
            material.SetFloat("_Opacidad", 1);
            esfera.localScale = Vector3.zero;
            sonidoFlag = true;

            this.gameObject.SetActive(false);
        }
    }



    private void AumentaRangoOnda()
    {
        if (esfera.localScale.magnitude < rango - (rango/4))
        {  
            esfera.transform.localScale += Vector3.one * speed * Time.deltaTime;
            speed -= speed * Time.deltaTime;
        }
        else
        {
            esfera.transform.localScale += Vector3.one * (speed/200) * Time.deltaTime;
        }
            
    }

    private void OpacidadOnda()
    {
        if (esfera.localScale.magnitude > empiezaFade && material.GetFloat("_Opacidad") > 0)
        {
            float fadeAmount = material.GetFloat("_Opacidad") - (fade/10 * Time.deltaTime);
            
            material.SetFloat("_Opacidad", fadeAmount);
        }
    }

}
