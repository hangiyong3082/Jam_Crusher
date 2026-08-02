using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdCounterText : MonoBehaviour
{
    [SerializeField] AdCoinRewardHandler manager;

    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        text.text = $"{manager.GetRemainingWatches()} / {manager.maxCount.ToString()}";
    }
}
