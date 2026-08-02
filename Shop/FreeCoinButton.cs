using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeCoinButton : MonoBehaviour
{
    [SerializeField] AdCoinRewardHandler manager;

    Button button;

    private void Awake()
    {
        button = transform.GetComponentInChildren<Button>();
    }

    private void Start()
    {
        button.enabled = manager.GetRemainingWatches() > 0;
    }
}
