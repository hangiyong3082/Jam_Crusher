using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText;
    [Header("순서대로")]
    [SerializeField] SkinManager[] skinManagers;
    [SerializeField] SkinShopItemsParent[] skinShopItemsParents;
    [SerializeField] GameObject _skinShopItem;
    [SerializeField] GameObject itemWatchingHelper;
    bool alreadyGenerateItems;
    GameObject[] equipedSkins = new GameObject[2];
    //cosmetic(보류)
    //[SerializeField] int maxNumOfCosmetics;
    //

    void Update()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }


    public void GenerateItems()
    {
        if (alreadyGenerateItems) return;

        foreach (var manager in skinManagers)
        {
            Transform parent = FindParent(manager);
            for (int i = 0; i < manager.skins.Count; i++)
            {
                SkinShopItem skinShopItem = Instantiate(_skinShopItem,parent).GetComponent<SkinShopItem>();
                skinShopItem.skinManager = manager;
            }

            for (int i = 0; i < 5; i++)
            {
                Instantiate(itemWatchingHelper, parent);
            }
        }
        alreadyGenerateItems = true;

        Transform FindParent(SkinManager skinManager)
        {
            foreach (var parent in skinShopItemsParents)
            {
                if (parent.skinType == skinManager.skinType)
                {
                    return parent.transform;
                }
            }
            return null;
        }
    }

    public void SetSkin()
    {
        foreach (var skinManager in skinManagers)
        {
            var player = GameObject.FindWithTag("Player");
            switch (skinManager.skinType)
            {
                case SkinType.Pattern:
                    player.GetComponent<MeshRenderer>().material = skinManager.GetSelectedSkin().pattern;
                    continue;
                case SkinType.Cosmetic:
                    try
                    {
                        Destroy(equipedSkins[0].gameObject);

                    }
                    catch { }
                    equipedSkins[0] = Instantiate(skinManager.GetSelectedSkin().cosmetic, player.transform);

                    continue;
                case SkinType.Aura:
                    try
                    {
                        Destroy(equipedSkins[1].gameObject);

                    }
                    catch { }
                    equipedSkins[1] = Instantiate(skinManager.GetSelectedSkin().aura, player.transform); 
                    //SetWearableSkin(skinManager, player, s);
                    continue;
                case SkinType.Theme:
                    continue;
            }
        }
        
    }
    void SetWearableSkin(SkinManager skinManager, GameObject player, GameObject selectedSkin)
    {
        int keyOfSelectedSkin = skinManager.skins.IndexOf(skinManager.GetSelectedSkin()); //skinManager 안에서의 위치
        if (!skinManager.IsUnlocked(keyOfSelectedSkin))
        {
            return;
        }
        int indexOfEquipedSkin = 0;
        if (skinManager.skinType == SkinType.Cosmetic) indexOfEquipedSkin = 0;
        else if (skinManager.skinType == SkinType.Aura) indexOfEquipedSkin = 1;
        print($"{skinManager.skinType} -> {indexOfEquipedSkin}");
        if (equipedSkins[indexOfEquipedSkin] == null)
        {
            equipedSkins[indexOfEquipedSkin] = selectedSkin;
            //print(equipedSkins[indexOfEquipedSkin].name + " 없어서 만들기");
        }
        else if (equipedSkins[indexOfEquipedSkin].GetComponent<ShopItemPrefab>().Name !=
            selectedSkin.GetComponent<ShopItemPrefab>().Name)
        {
            Destroy(equipedSkins[indexOfEquipedSkin].gameObject);
            equipedSkins[indexOfEquipedSkin] = selectedSkin;
            //print(equipedSkins[indexOfEquipedSkin].name + " 새로 만들기");
        }
    }
}
