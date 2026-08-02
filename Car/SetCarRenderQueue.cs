using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCarRenderQueue : MonoBehaviour
{
    private void Start()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).GetChild(0).GetComponent<MeshRenderer>().material.renderQueue = 3002;
            }         
        }   
    }
}
