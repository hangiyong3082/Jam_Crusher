using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    [SerializeField] int renderQueue;

    private void Awake()
    {
        GetComponent<MeshRenderer>().material.renderQueue = renderQueue;
        
    }

    private void Start()
    {
        
    }
}
