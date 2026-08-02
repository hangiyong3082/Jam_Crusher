using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedUpTextAnim : MonoBehaviour
{
    public void Work()
    {
        DOTween.Kill("SpeedUpTextAnim", true);
        transform.GetComponent<TMP_Text>().DOFade(1, 0.7f).SetEase(Ease.OutBounce).SetId("SpeedUpTextAnim");
        transform.GetComponent<TMP_Text>().DOFade(0, 0.7f).SetDelay(2).SetEase(Ease.OutBounce).SetId("SpeedUpTextAnim");
    }
}
