using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZonas : MonoBehaviour
{
    [SerializeField] private int zona;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            
            CambiaZona();
        }
    }

    public void CambiaZona()
    {
        GameManager.Instance.zonaMonstruo = zona;
        Debug.Log("zonaMonstruo" + GameManager.Instance.zonaMonstruo);
    }

}
