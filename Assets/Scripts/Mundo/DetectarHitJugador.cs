using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarHitJugador : MonoBehaviour
{
    public bool EnteredTrigger = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            EnteredTrigger = true;
        }
    }
}
