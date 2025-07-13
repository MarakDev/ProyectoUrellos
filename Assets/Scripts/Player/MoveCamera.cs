using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public GameObject posicionChasquido;
    public GameObject posicionPiedra;

    // Update is called once per frame

    
    void Update()
    {
        pChasquido();
        pPiedra();
        transform.position = cameraPosition.position;
    }

    private void pChasquido()
    {
        Vector3 cameraVector = Camera.main.transform.forward;
        cameraVector.y = 0;
        //Debug.DrawRay(transform.position, cameraVector * 1.5f, Color.red);
        //Debug.Log(cameraVector);

        GameObject j = GameObject.Find("Jugador");

        posicionChasquido.transform.position = j.transform.position + cameraVector * 1.5f + new Vector3(0, -1, 0);
    }

    private void pPiedra()
    {
        Vector3 cameraVector = Camera.main.transform.forward;
        //Debug.DrawRay(transform.position, cameraVector * 1.5f, Color.red);

        GameObject j = GameObject.Find("Jugador");

        posicionPiedra.transform.position = j.transform.position + cameraVector/2;
    }
}
