using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarHitbox : MonoBehaviour
{
    public bool EnteredTrigger = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
        {
            EnteredTrigger = true;
        }
    }
}
