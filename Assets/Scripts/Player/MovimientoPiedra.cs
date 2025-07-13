using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovimientoPiedra : MonoBehaviour
{
    [SerializeField] float fuerza = 35;
    [SerializeField] GameObject ondasPrefab;

    Vector3 gravedad = new Vector3(0, -9.81f, 0);
    Rigidbody rb;
    Vector3 cameraVector;

    private bool firstContact = true;
    private int touch = 0;

    void Start()
    {
        Camera camera = GameObject.Find("CamHolder").GetComponent<Camera>();
        cameraVector = Camera.main.transform.forward;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        gravedad.y +=+ gravedad.y * 1.5f * Time.deltaTime;

    }

    void FixedUpdate()
    {
        if (firstContact)
        {
            rb.AddForce(cameraVector * fuerza, ForceMode.Force);
            rb.AddForce(gravedad, ForceMode.Force);

            if (rb.velocity.y < -1000) //Control crash por -infinity
                Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 && rb.velocity.magnitude > 1)
        {
            float distancia = GameManager.Instance.DistanciaJugador_Sonido(this.gameObject);

            if (distancia > 0.1f && touch < 3)
            {
                SoundManager.Instance.SeleeccionAudio(0, distancia, "Entorno");
                Instantiate(ondasPrefab, transform.position, Quaternion.identity);
                touch++;

            }else
                SoundManager.Instance.SeleeccionAudio(0, 0.1f, "Entorno");

            firstContact = false;
            Destroy(this.gameObject, 6);
        }

    }
}
