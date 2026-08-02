using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinProductShopItem : MonoBehaviour
{
    [SerializeField] CoinProductManager coinProductManager;
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text costText;
    [SerializeField] int order;

    CoinProduct coinProduct;

    bool isPaid = false;

    private void Awake()
    {
        coinProduct = coinProductManager.items[order];
    }
    private void Start()
    {
        amountText.text = $"x{coinProduct.amount}";
        if(costText != null) costText.text = $"${coinProduct.cost}";
    }

    public void GetCoins()
    {
        PayForItem();
        if (isPaid)
        {
            CoinManager.Instance.AddCoins(coinProduct.amount);
            isPaid = false;
        }
    }

    void PayForItem()
    {

    }
}
