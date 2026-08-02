using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    public void AddCoinsForIAP(int amount)
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + amount);
        MasterAudio.PlaySound("Shop_Purchase");
    }

    public void AddCoins(int amount, float delayTime = 0)
    {
        void Work()
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + amount);
        }

        if (delayTime != 0)
        {
            StartCoroutine(IAddCoins(delayTime));
        }
        else
        {
            Work();
        }
        IEnumerator IAddCoins(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            Work();
        }
    }

    public void AddCoinsWith3DEffect(int amount, Vector3 position = default, float addingCoinsDelayTime = 0)
    {
        AddCoins(amount, addingCoinsDelayTime);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(GameManager.Instance.coinParticle, position, GameManager.Instance.coinParticle.transform.rotation);
        }
    }
}
