using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingScannerPlayer : MonoBehaviour
{
    public static PoolingScannerPlayer instancia;

    public GameObject[] prefabScanner;

    int recarga = 3;
    List<GameObject> scannerLista;

    private void Awake()
    {
        instancia = this;

        scannerLista = new List<GameObject>(recarga);

        for(int i = 0; i< recarga; i++)
        {
            GameObject scannerInstancia = Instantiate(prefabScanner[i]);
            scannerInstancia.transform.SetParent(transform);
            scannerInstancia.SetActive(false);
            scannerLista.Add(scannerInstancia);
        }

    }
    public GameObject GetEsfera()
    {
        for(int i = 0; i< recarga; i++)
        {
            if (!scannerLista[i].activeInHierarchy)
            {
                scannerLista[i].SetActive(true);
                return scannerLista[i];
            }
        }
        return null;    
    }

}
