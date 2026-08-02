using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShopAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScaleY(0, 0);
        transform.DOScaleY(1, 0.3f).SetDelay(0.1f);
    }
    private void OnDisable()
    {
        transform.DOScaleY(0, 0);

    }
}
