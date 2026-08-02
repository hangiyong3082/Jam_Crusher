using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("°á·Ð : ¾È µÊ")]
public class TweenTest : MonoBehaviour
{
    Tweener test;

    private void Awake()
    {
        test = transform.DOShakePosition(1, new Vector3(0.03f, 0, 0.03f), randomness: 30, fadeOut: false).SetLoops(-1, LoopType.Restart).SetId("ShakeCar");
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DOTween.Play(test);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            DOTween.Kill("ShakeCar");
        }
    }
}
