using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskCar : MonoBehaviour
{
    [SerializeField] GameObject[] maskObj;

    private void Start()
    {
        foreach (GameObject i in maskObj)
        {
            i.GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }
}
