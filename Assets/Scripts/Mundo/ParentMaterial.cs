using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int num = transform.childCount;

        for(int i = 0; i < num; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponent<Renderer>().material = this.gameObject.GetComponent<Renderer>().material;
        }
        
    }

}
