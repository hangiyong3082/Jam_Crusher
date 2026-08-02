using UnityEngine;

public enum AdRewardType
{
    Coin,
    QuickStart
}
public class AdShower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AdRewardType adRewardType;

    public void ShowAd()
    {
        InterstitialAd interstitialAd = FindAnyObjectByType<InterstitialAd>();
        interstitialAd.AdRewardType = adRewardType;
        interstitialAd.ShowAd();
    }
}
