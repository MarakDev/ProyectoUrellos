using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EscenaFinalNivel : MonoBehaviour
{
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject campana;
    [SerializeField] private GameObject campanaSonnar;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject c;

    private bool unaVez = true;

    private float _degradadoFinDemo;

    private void Update()
    {
        if (hitbox.GetComponent<DetectarHitJugador>().EnteredTrigger)
        {
            if (unaVez)
            {
                unaVez = false;

                GameObject.Find("CamHolder").SetActive(false);
                GameObject.Find("Jugador").GetComponent<PlayerMovement>().enabled = false;
                GameObject.Find("Jugador").GetComponent<PlayerAbilities>().enabled = false;

                cam.SetActive(true);
         
                Instantiate(campanaSonnar, campana.transform.position, Quaternion.identity);
                SoundManager.Instance.SeleeccionAudio(11, 1f, "Entorno");

            }

            _degradadoFinDemo += Time.deltaTime;

            if(_degradadoFinDemo > 7)
            {
                c.SetActive(true);
                c.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.calaveras + " / 8 Calaveras Obtenidas";

                if(_degradadoFinDemo > 15)
                {
                    GameManager.Instance.NextScene(3);

                    c.SetActive(false);
                }

            }
        }
    }
}
