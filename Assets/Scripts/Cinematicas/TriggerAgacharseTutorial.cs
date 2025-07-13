using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAgacharseTutorial : MonoBehaviour
{
    private WaitForSeconds timing;
    private bool unaVez = true;

    private void OnTriggerEnter(Collider other)
    {
        if (unaVez)
            StartCoroutine(TextoPiedra());
    }

    IEnumerator TextoPiedra()
    {
        unaVez = false;
        if(CanvasManager.Instance.TextosTutorial[3].activeInHierarchy)
            CanvasManager.Instance.TextosTutorial[3].SetActive(false);

        CanvasManager.Instance.TextosTutorial[4].SetActive(true);
        timing = new WaitForSeconds(3);
        yield return timing;
        CanvasManager.Instance.TextosTutorial[4].SetActive(false);
    }

}
