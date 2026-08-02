using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

[RequireComponent(typeof(IAPListener))]
public class ThanksForBuying : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        text.color *= new Color(1, 1, 1, 0);
    }

    public void WhenConfirmed()
    {
        DOTween.Kill("ThanksForBuyingText");
        text.color += new Color(0, 0, 0, -text.color.a + 1);
        text.DOColor(text.color * new Color(1, 1, 1, 0), 3f).SetId("ThanksForBuyingText");       
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Playing)
        {
            gameObject.SetActive(false);
        }
    }

}
