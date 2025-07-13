using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Cooldown Scaner Jugador")]
    public float cooldown_Scanner = 1.5f;

    [Header("Cooldown Scaner Pisadas")]
    public float cooldown_footstep;
    public GameObject scannerFootstepPrefab;

    [Header("Cooldown Scaner Caida")]
    public float cooldown_fall;
    public GameObject scannerFallPrefab;

    [Header("Cooldown Roca")]
    public float cooldown_rock;
    public GameObject rockPrefab;

    [Header("")]
    [SerializeField] private float maxUseDistance = 4f;
    [SerializeField] private LayerMask layerPuerta;
    [SerializeField] private LayerMask layerCalavera;

    [SerializeField] GameObject cameraAux;
    PlayerMovement Jugador;

    private bool cooldownCamFlag = true;
    private bool cooldownFootstepFlag = true;
    private bool cooldownFallFlag = true;
    private bool cooldownThrowRock = true;

    private WaitForSeconds corrutineDelay;

    private void Start()
    {
        Jugador = GameObject.Find("Jugador").GetComponent<PlayerMovement>();
    }
    void Update()
    {
        //Debug.Log(canScann);
        SonnarActivador();

        ScannerFootstep();

        ScannerFall();

        ThrowRock();

        OnUse();
    }

    private void SonnarActivador()
    {
        GameObject chasquido = GameObject.Find("PosicionChasquido");

        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownCamFlag)
        {
            GameObject objetoScanner = PoolingScannerPlayer.instancia.GetEsfera();
            
            objetoScanner.transform.position = chasquido.transform.position;

            StartCoroutine(CooldownScanner(cooldown_Scanner));
        }
    }

    IEnumerator CooldownScanner(float coolDown)
    {
        corrutineDelay = new WaitForSeconds(coolDown);

        cooldownCamFlag = false;
        yield return corrutineDelay;
        cooldownCamFlag = true;
    }

    private void ScannerFootstep()
    {
        if (Jugador.state == PlayerMovement.MovementState.run && cooldownFootstepFlag && GetComponent<Rigidbody>().velocity.magnitude > 4)
        {
            Instantiate(scannerFootstepPrefab, transform.position + new Vector3(0,-1,0), Quaternion.identity);
            StartCoroutine(CooldownScanner_Footstep(cooldown_footstep));
        }
    }

    IEnumerator CooldownScanner_Footstep(float coolDown)
    {
        corrutineDelay = new WaitForSeconds(coolDown);

        cooldownFootstepFlag = false;
        yield return corrutineDelay;
        cooldownFootstepFlag = true;
    }

    private void ScannerFall()
    {
        if (cooldownFallFlag && Jugador.jugadorCaidaSuelo_Flag)
        {
            Instantiate(scannerFallPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            StartCoroutine(CooldownScanner_Fall(cooldown_footstep));
        }
    }

    IEnumerator CooldownScanner_Fall(float coolDown)
    {
        corrutineDelay = new WaitForSeconds(coolDown);

        cooldownFallFlag = false;
        yield return corrutineDelay;
        cooldownFallFlag = true;
    }

    private void ThrowRock()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1) && cooldownThrowRock && GameManager.Instance.PoderTirarPiedra())
        {
            SoundManager.Instance.SeleeccionAudio(8, 1f, "Jugador");

            Camera camera = GameObject.Find("CamHolder").GetComponent<Camera>();
            GameObject posicionPiedra = GameObject.Find("PosicionPiedra");

            Vector3 cameraVector = Camera.main.transform.forward;
            cameraVector.y += 0.75f;

            GameObject auxObjeto = Instantiate(rockPrefab, posicionPiedra.transform.position, Quaternion.identity);
            auxObjeto.GetComponent<Rigidbody>().AddForce(cameraVector * 10, ForceMode.Impulse);

            StartCoroutine(CooldownRock(cooldown_rock));
        }
    }


    IEnumerator CooldownRock(float coolDown)
    {
        corrutineDelay = new WaitForSeconds(coolDown);

        cooldownThrowRock = false;
        yield return corrutineDelay;
        cooldownThrowRock = true;
    }


    public void OnUse()
    {
        //Interaccionando con una puerta
        if(Physics.Raycast(cameraAux.transform.position, cameraAux.transform.forward, out RaycastHit hit, maxUseDistance, layerPuerta) && Input.GetKeyDown(KeyCode.E))
        {
            if(hit.collider.TryGetComponent<Puerta>(out Puerta puerta))
            {
                puerta.Open();
            }
        }

        if(Physics.Raycast(cameraAux.transform.position, cameraAux.transform.forward, out RaycastHit hit2, maxUseDistance, layerCalavera) && Input.GetKeyDown(KeyCode.E))
        {
            if (hit2.collider.TryGetComponent<Calavera>(out Calavera c))
            {
                c.RecogerCalavera();
            }
        }
    }

}
