using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCoinRewardHandler : MonoBehaviour
{
    [SerializeField] DateChecker dateChecker;

    const string countKey = "AdWatchCount";
    int reward = 10;
    [HideInInspector] public int maxCount = 10;

    void Start()
    {
        CheckAndResetCount();
    }

    private void CheckAndResetCount()
    {
        if (dateChecker.isNewDate)
        {
            PlayerPrefs.SetInt(countKey, 0);
            PlayerPrefs.Save();
        }
    
    }

    public void OnAdWatchedSuccessfully()
    {
        int currentCount = PlayerPrefs.GetInt(countKey, 0);
        currentCount++;
        CoinManager.Instance.AddCoins(reward);
        MasterAudio.PlaySound("Game_CollectCoins");
        PlayerPrefs.SetInt(countKey, currentCount);
        PlayerPrefs.Save();
    }

    public int GetRemainingWatches()
    {
        int currentCount = PlayerPrefs.GetInt(countKey, 0);
        return maxCount - currentCount;
    }

    public bool CanWatchAd()
    {
        return GetRemainingWatches() > 0;
    }
}
