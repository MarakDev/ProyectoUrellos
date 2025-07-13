using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class MonstruoIA : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private GameObject pasosMonstruo;
    [SerializeField] private GameObject rugidoMonstruoBusqueda;
    [SerializeField] private GameObject rugidoMonstruoBusquedaFinal;
    [SerializeField] private GameObject rugidoMonstruo;

    [SerializeField] private Transform[] posicionMonstruoMapa;
    [SerializeField] private Transform[] waypoint;
    [SerializeField] private LayerMask layerSonidoObjeto;
    [SerializeField] private LayerMask layerSonidoPlayer;
    [SerializeField] private LayerMask layerPlayer;

    private int waypointIndex;
    private Vector3 target;

    private bool detectaSonido;
    private bool enAccion;
    private bool detectaJugador;

    private Collider[] hitColliders;
    private Collider focoDeSonido;

    private float _timeSinceLastStep;
    private bool izqOrDer;

    private int zonaActual;

    //test
    private Color color = Color.red;

    public Estados state;

    private WaitForSeconds x;

    public enum Estados
    {
        Patrulla,
        Busqueda,
        Ataque
    }

    //Debug
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, 15);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 6);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,1,0), 2);
    }

    void ColoresDebug()
    {
        if(detectaJugador)
        {
            if (enAccion)
            {
                color = Color.red;
            }
            else
                color = Color.white;
        }
        else if(detectaSonido)
        {
            if (enAccion)
            {
                color = Color.green;
            }
            else
                color = Color.white;
        }
        else
            color = Color.cyan;

        
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        zonaActual = 0;
        waypointIndex = Random.Range(0, 5);
        UpdatePosicion();
    }

    private void Update()
    {
        if (!detectaSonido) //esfera que comprueba si hay sonidos en el rango escuchable
            CheckSonido();

        if(!detectaJugador)
            CheckPlayer();

        SonidosMonstruo_Pasos();

        ActivarModoBusqueda();
        ActivarModoAtque();

        if (Vector3.Distance(transform.position, target) < 1 && state != Estados.Ataque)
        {
            NewWaypoint();
            UpdatePosicion();
        }

        if (state == Estados.Ataque)
        {
            target = GameObject.Find("Jugador").transform.position;
            agent.SetDestination(target);
        }

        //Cambiazonas
        if(zonaActual != GameManager.Instance.zonaMonstruo)
        {
            ZonaMonstruo();
            NewWaypoint();
            UpdatePosicion();

            zonaActual = GameManager.Instance.zonaMonstruo;
        }

        MataJugador();
        //ColoresDebug();

    }

    private void ActivarModoBusqueda()
    {
        if (state == Estados.Busqueda && !enAccion)
        {
            agent.SetDestination(focoDeSonido.transform.position);
            detectaSonido = true;
            enAccion = true;
            StartCoroutine(CoolDownModoBusqueda());
        }
    }

    private void ActivarModoAtque()
    {
        if (state == Estados.Ataque && !detectaJugador)
        {
            agent.SetDestination(focoDeSonido.transform.position);
            detectaJugador = true;
            enAccion = true;
            StopCoroutine(CoolDownModoBusqueda());
            StartCoroutine(CoolDownModoAtaque());
        }
    }

    private void CheckSonido()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 12, layerSonidoObjeto);

        if (hitColliders.Length > 0)
        {
            focoDeSonido = ColliderMasCercano();
            state = Estados.Busqueda;

        }
    }

    private void CheckPlayer()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 4, layerSonidoPlayer);

        if (hitColliders.Length > 0)
        {
            focoDeSonido = ColliderMasCercano();
            state = Estados.Ataque;

        }
    }

    private void MataJugador()
    {
        hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0, 1, 0), 2, layerPlayer);

        if (hitColliders.Length > 0)
        {
            //Trigger animacion muerte
            //Game over
            SoundManager.Instance.Stop();

            SoundManager.Instance.SeleeccionAudio(7, 2f, "Monstruo");

            GameManager.Instance.GameOver();
        }
    }

    private Collider ColliderMasCercano()
    {
        Collider auxC = null;
        float minDistancia = 999999;
        foreach (Collider collider in hitColliders)
        {
            float distancia = Vector3.Distance(collider.transform.position, this.transform.position);

            if (distancia < minDistancia)
            {
                auxC = collider;
                minDistancia = distancia;
            }
        }
        return auxC;
    }

    void UpdatePosicion()
    {
        if(state == Estados.Patrulla && !enAccion) 
        {
            target = waypoint[waypointIndex].position;
            agent.SetDestination(target);
        }        
    }

    private void NewWaypoint()
    {
        int aux = 0;

        switch (GameManager.Instance.zonaMonstruo)
        {
            case 0:
                aux = Random.Range(0, 6);
                break;
            case 1:
                aux = Random.Range(6, 14);
                break;
            case 2:
                aux = Random.Range(14, 20);
                break;
        }
        
        if(waypointIndex != aux)
            waypointIndex = aux;

    }

    private void SonidosMonstruo_Pasos()
    {
        float distanciaJugador = GameManager.Instance.DistanciaJugador_Sonido(this.gameObject);

        //PASOS_Monstruo
        if (agent.velocity.magnitude > 0 && state != Estados.Ataque)
        {
            _timeSinceLastStep += Time.deltaTime;
            if (_timeSinceLastStep > 1)
            {
                _timeSinceLastStep = 0;

                if (izqOrDer)//paso izquierdo
                {
                    SoundManager.Instance.SeleeccionAudio(1, distanciaJugador, "Monstruo");
                    Instantiate(pasosMonstruo, transform.position, Quaternion.identity);
                    izqOrDer = false;
                }
                else //paso derecho
                {
                    SoundManager.Instance.SeleeccionAudio(2, distanciaJugador, "Monstruo");
                    Instantiate(pasosMonstruo, transform.position, Quaternion.identity);
                    izqOrDer = true;
                }

            }
        }
        else if(agent.velocity.magnitude > 0 && state == Estados.Ataque)
        {
            _timeSinceLastStep += Time.deltaTime;
            if (_timeSinceLastStep > 0.4f)
            {
                _timeSinceLastStep = 0;

                if (izqOrDer)//paso izquierdo
                {
                    SoundManager.Instance.SeleeccionAudio(1, distanciaJugador * 3, "Monstruo");
                    Instantiate(pasosMonstruo, transform.position, Quaternion.identity);
                    izqOrDer = false;
                }
                else //paso derecho
                {
                    SoundManager.Instance.SeleeccionAudio(2, distanciaJugador * 3, "Monstruo");
                    Instantiate(pasosMonstruo, transform.position, Quaternion.identity);
                    izqOrDer = true;
                }

            }
        }

    }

    private void ZonaMonstruo()
    {
        switch(GameManager.Instance.zonaMonstruo)
        {
            case 0:
                agent.Warp(posicionMonstruoMapa[0].transform.position);
                break;

            case 1:
                agent.Warp(posicionMonstruoMapa[1].transform.position);
                break;

            case 2:
                agent.Warp(posicionMonstruoMapa[2].transform.position);
                break;
        }
    }

    private IEnumerator CoolDownModoBusqueda()
    {
        agent.isStopped = true;

        x = new WaitForSeconds(2); //aviso jugador
        yield return x;

        float distanciaJugador = GameManager.Instance.DistanciaJugador_Sonido(this.gameObject);

        if (state == Estados.Busqueda)
        {
            Instantiate(rugidoMonstruoBusqueda, transform.position, Quaternion.identity);
            SoundManager.Instance.SeleeccionAudio(3, distanciaJugador, "Monstruo");

            x = new WaitForSeconds(0.5f); //cadenas
            yield return x;

            SoundManager.Instance.SeleeccionAudio(4, distanciaJugador, "Monstruo");
        }

        agent.isStopped = false;

        x = new WaitForSeconds(8); //tiempo de modo busqueda activo
        yield return x;

        if (!detectaJugador)
        {
            SoundManager.Instance.SeleeccionAudio(6, 0.8f, "Monstruo");
            Instantiate(rugidoMonstruoBusquedaFinal, transform.position, Quaternion.identity);

            state = Estados.Patrulla;
            enAccion = false;

            NewWaypoint();
            UpdatePosicion();

            agent.isStopped = true;
            x = new WaitForSeconds(1); //tiempo para volver activar el modo busqueda
            yield return x;

            agent.isStopped = false;
        }
        
        x = new WaitForSeconds(5); //tiempo para volver activar el modo busqueda
        yield return x;

        detectaSonido = false;
        
    }

    private IEnumerator CoolDownModoAtaque()
    {
        SoundManager.Instance.Stop();
        agent.isStopped = true;

        agent.angularSpeed = 700;
        agent.acceleration = 60;

        //Sonido rugido
        Instantiate(rugidoMonstruo, transform.position, Quaternion.identity);
        SoundManager.Instance.SeleeccionAudio(0, 2f, "Monstruo");

        x = new WaitForSeconds(1.5f);
        yield return x;

        agent.isStopped = false;
        agent.speed = 15;
        SoundManager.Instance.SeleeccionAudio(5, 2f, "Monstruo");
        //modo berserk

        x = new WaitForSeconds(10); //tiempo de busqueda del jugador
        yield return x;
        //no encuentra nada
        SoundManager.Instance.Stop();

        SoundManager.Instance.SeleeccionAudio(6, 0.8f, "Monstruo");
        Instantiate(rugidoMonstruoBusquedaFinal, transform.position, Quaternion.identity);

        state = Estados.Patrulla;
        enAccion = false;

        NewWaypoint();
        UpdatePosicion();
        agent.speed = 3;
        agent.angularSpeed = 200;
        agent.acceleration = 8;

        x = new WaitForSeconds(5); //tiempo hasta volver a activar el modo ataque
        yield return x;

        detectaJugador = false;
    }
}
