using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItemSelector
{
    [SerializeField] public SkinType skinType;
    [SerializeField] public int skinIndex;
}

public class SelectedShopItemLocalizer : MonoBehaviour
{
    [SerializeField] List<ShopItemSelector> shopItems;
    [SerializeField] Transform[] viewportsByType = new Transform[4];

    bool isCompleted = false;

    void DoLocalize()
    {
        foreach (var item in shopItems)
        {
            Transform viewportByType = viewportsByType[(int)item.skinType];
            SkinShopItem skinShopItem = viewportByType.GetChild(item.skinIndex).GetComponent<SkinShopItem>();
            skinShopItem.DoLocalize();
        }
    }
}
